using System.Collections.Generic;
using ExposureUnnoticed2.Master.Stage;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.InGame;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcManager : MonoBehaviour, IUpdater, ILateUpdater
	{
		[SerializeField]
		private GameObject[] npcPrefabs;

		[SerializeField]
		private Transform sightAreaParent;

		[SerializeField]
		private GameObject sightAreaPrefab;

		public List<NpcController> ExistNpcList = new List<NpcController>();

		public Dictionary<GameObject, NpcController> ExistNpcGameObjectDict = new Dictionary<GameObject, NpcController>();

		private PlayerController player;

		private NpcSpawnInterval npcSpawnInterval = new NpcSpawnInterval();

		private bool initialized;

		private bool isLoadedData;

		private int fixedNpcCount;

		private int spawnNum;

		private MStageRecord rCurrentStage;

		private List<NpcController> removeList = new List<NpcController>();

		public static NpcManager Instance { get; private set; }

		public bool IsActivationSlow { get; private set; }

		public bool IsActivationBaretaSlow { get; private set; }

		public bool IsMaxStrangeSomeOne { get; private set; }

		public bool IsRiseStrangeInCameraSomeOne { get; private set; }

		public bool IsRiseStrange { get; private set; }

		public bool IsSomeOnePlayerFoundAndVisibleInCameraSomeOne { get; private set; }

		public bool IsSomeOnePlayerFound { get; private set; }

		public bool IsWatchingStranger { get; private set; }

		public float MaxWatchingNpcStrangeness { get; private set; }

		public void Initialize(PlayerController player)
		{
			// Original RVA 0x42F130; its unindexed native helper436590 writes the original Instance property.
			this.player = player;
			Instance = this;
			initialized = true;
		}

		private void OnDestroy()
		{
			// Original RVA 0x42F1A0: initialized guard, clear Instance, dispose every live controller.
			if (!initialized) return;
			Instance = null;
			foreach (NpcController npc in ExistNpcList) npc.Destroy();
		}

		public void OnLoadStage()
		{
			// Recovered from GameAssembly.dll RVA 0x42F300
			fixedNpcCount = 0;
			RepopNpc();
		}

		public void OnLoadData()
		{
			// Complete original RVA 0x42F310..0x42F758; original closure callback0x432DF0.
			isLoadedData = true;
			foreach (NpcSaveData npcData in global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.NpcSaveDataList)
			{
			    if (!npcData.IsExist) continue;
			    if (global::ExposureUnnoticed2.Object3D.ScenePlops.Elevator.ElevatorController.Instance
			        && global::ExposureUnnoticed2.Object3D.ScenePlops.Elevator.ElevatorController.Instance.PushBounds.Contains(npcData.Position))
			        continue;
			    Scheduler.DelayUnscaledTime(() =>
			    {
			        NpcManager manager = this;
			        if (manager is null) throw new System.NullReferenceException();
			        manager.InstantiateNpc(npcData, false, NpcStateModel.FixedType.None);
			    }, UnityEngine.Random.value);
			}
		}

		public void RemoveNpc(NpcController npc)
		{
		}

		public void RepopNpc()
		{
			foreach (NpcController npc in ExistNpcList) npc.Destroy();
			ExistNpcList.Clear();
			ExistNpcGameObjectDict.Clear();
			spawnNum = 0;
			if (!ExposureUnnoticed2.Object3D.RoutePoint.BaseRouteBuilder.Instance) return;
			rCurrentStage = MStage.Get(InGameManager.Instance.CurrentStageController.StageType);
			npcSpawnInterval.OnLoadStage(rCurrentStage);
			float scale = 1f;
			if ((int)rCurrentStage.StageType == 11)
			    scale = GameState.OptionData.NpcNumScaleMallDaytime * 0.01f;
			float number;
			if (GameState.GameStateData.IsDaytime)
			    number = rCurrentStage.NpcNum * scale;
			else
			    number = rCurrentStage.NpcNumNight;
			if ((int)GameState.GameStateData.DifficultType != 2)
			    number *= 0.7f;
			if (rCurrentStage.NpcNumRandom && !rCurrentStage.NpcNumRandomInitExist) return;
			for (int i = 0; i < (int)number; ++i)
			    Scheduler.DelayUnscaledTime(() => InstantiateNpc(null, true), UnityEngine.Random.value);
		}

		public void RegisterDestroy(NpcController npc)
		{
			// Original RVA 0x42FE10; NpcManager.OnUpdate consumes the deferred list.
			removeList.Add(npc);
		}

		private NpcController InstantiateNpc(NpcSaveData npcSaveData = null, bool isRandomPathSpawn = false, NpcStateModel.FixedType fixedType = NpcStateModel.FixedType.None)
		{
			if (global::ExposureUnnoticed2.Object3D.RoutePoint.BaseRouteBuilder.Instance == null) return null;
			NpcComponent component = UnityEngine.Object.Instantiate(npcPrefabs[0], transform).GetComponent<NpcComponent>();
			NpcController controller = component.CreateController();
			controller.Initialize(player, npcSaveData, isRandomPathSpawn, fixedType);
			ExistNpcList.Add(controller);
			ExistNpcGameObjectDict.Add(controller.GameObject, controller);
			return controller;
		}

		public NpcController CreateFixedNpc(NpcStateModel.FixedType type)
		{
			NpcController npc = InstantiateNpc(null, false, type);
			npc.AdditionalInitializeForFixAction(type);
			++fixedNpcCount;
			return npc;
		}

		public void OnFixedUpdate()
		{
            if (InGameManager.Instance.TempInGameState.IsTimeStop) return;
            if (InGameManager.Instance.TempInGameState.IsPlayingSomePerformance) return;
            if (global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.IsOpenFade) return;
            if (global::ExposureUnnoticed2.ObjectUI.SleepBlackPanel.SleepBlackPanelView.IsOpen) return;
            if (InGameManager.Instance.TempInGameState.IsGameOver) return;
            foreach (NpcController npc in ExistNpcList) npc.OnFixedUpdate();
        }

		public void OnUpdate()
		{
			if (rCurrentStage == null) return;
			IsMaxStrangeSomeOne = false;
			IsSomeOnePlayerFound = false;
			IsWatchingStranger = false;
			MaxWatchingNpcStrangeness = 0f;
			if (InGameManager.Instance.TempInGameState.IsTimeStop)
			    foreach (NpcController npc in ExistNpcList) npc.OnUpdateOnTimeStop();
			if (InGameManager.Instance.TempInGameState.IsTimeStop) return;
			if (InGameManager.Instance.TempInGameState.IsPlayingSomePerformance) return;
			if (ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.IsOpenFade) return;
			if (ExposureUnnoticed2.ObjectUI.SleepBlackPanel.SleepBlackPanelView.IsOpen) return;
			if (InGameManager.Instance.TempInGameState.IsGameOver) return;
			if (!rCurrentStage.NpcNumRandom)
			{
			    if (spawnNum > 0)
			    {
			        InstantiateNpc(null, false, NpcStateModel.FixedType.None);
			        spawnNum = unchecked(spawnNum - 1);
			    }
			}
			else if (npcSpawnInterval.IsSpawn(ExistNpcList.Count - fixedNpcCount))
			    InstantiateNpc(null, false, NpcStateModel.FixedType.None);
			removeList.Clear();
			foreach (NpcController npc in ExistNpcList)
			{
			    if (!npc.GameObject.activeInHierarchy) removeList.Add(npc);
			    else npc.OnUpdateEarly();
			}
			foreach (NpcController npc in ExistNpcList)
			{
			    if (!npc.GameObject.activeInHierarchy) continue;
			    npc.OnUpdateAlsoGameOver();
			    if (InGameManager.Instance.TempInGameState.IsGameOver) continue;
			    npc.OnUpdate();
			    IsMaxStrangeSomeOne = 1f <= npc.Nca.StrangenessManager.Strangeness.Value || IsMaxStrangeSomeOne;
			    bool wasWatching = IsWatchingStranger;
			    bool watching = npc.IsWatchingStranger;
			    IsWatchingStranger = wasWatching || watching;
			    bool wasFound = IsSomeOnePlayerFound;
			    bool found = false;
			    if (npc.Nca.PlayerSearcher.IsPlayerFound)
			    {
			        if (!npc.Nca.NpcChairSubState.IsCurrentChairSleep) found = true;
			        else found = !(npc.Nca.StateModel.StrangeAddScaleSleep <= 0f);
			    }
			    IsSomeOnePlayerFound = wasFound || found;
			    if (IsWatchingStranger)
			    {
			        float value = npc.Nca.StrangenessManager.Strangeness.Value;
			        if (MaxWatchingNpcStrangeness < value) MaxWatchingNpcStrangeness = npc.Nca.StrangenessManager.Strangeness.Value;
			    }
			}
			foreach (NpcController npc in removeList)
			{
			    ExistNpcList.Remove(npc);
			    ExistNpcGameObjectDict.Remove(npc.GameObject);
			    npc.Destroy();
			    spawnNum = unchecked(spawnNum + 1);
			}
			IsActivationSlow = false;
			IsActivationBaretaSlow = false;
			IsRiseStrangeInCameraSomeOne = false;
			IsRiseStrange = false;
			IsSomeOnePlayerFoundAndVisibleInCameraSomeOne = false;
			foreach (NpcController npc in ExistNpcList)
			{
			    if (npc.IsVisible && npc.Nca.PlayerSearcher.IsPlayerFound) IsSomeOnePlayerFoundAndVisibleInCameraSomeOne = true;
			    if (npc.IsRiseStrangeness)
			    {
			        if (npc.IsVisible && !npc.Nca.StateModel.IsRiseStrangeFromOnlyDiscomfort) IsRiseStrangeInCameraSomeOne = true;
			        if (npc.IsRiseStrangeness && !npc.Nca.StateModel.IsRiseStrangeFromOnlyDiscomfort) IsRiseStrange = true;
			    }
			    if (0.7f <= npc.Nca.StrangenessManager.Strangeness.Value && npc.IsRiseStrangeness && !npc.Nca.StateModel.IsRiseStrangeFromOnlyDiscomfort) IsActivationSlow = true;
			    if (1f <= npc.Nca.StrangenessManager.Strangeness.Value)
			    {
			        IsActivationSlow = true;
			        IsActivationBaretaSlow = true;
			    }
			}
		}

		public void OnLateUpdate()
		{
			foreach (NpcController npc in ExistNpcList) npc.OnLateUpdate();
		}

		public void OnGameOverEvent()
		{
// Complete original RVA 0x431420; PE/typed identity is audited in this owned stage.
foreach (var npc in ExistNpcList) npc.OnGameOver();
}

		public (float, NpcController, NpcController) GetNearNpcDistance(Transform playerTransform)
		{
			return default;
		}

		public float GetMinPureStrange()
		{
			return 0f;
		}

		public int CalcInRangeNpcNum(float distance)
        {
            int count = 0;
            foreach (var npc in ExistNpcList)
            {
                if (distance >= global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DistSqrt(player.gameObject, npc.GameObject)) ++count;
            }
            return count;
        }

		public NpcController GetNearOtherNpc(NpcController self)
		{
            NpcController nearest = null;
            float nearestSquared = float.MaxValue;
            foreach (NpcController other in ExistNpcList)
            {
                if (other == self) continue;
                UnityEngine.Vector3 delta = other.Nca.StateModel.CurrentPosition - self.Nca.StateModel.CurrentPosition;
                float squared = delta.sqrMagnitude;
                if (squared < nearestSquared)
                {
                    nearest = other;
                    nearestSquared = squared;
                }
            }
            return nearest;
        }

		public NpcComponent SearchNpcController(GameObject go)
		{
			return null;
		}

		public void ApplyGameState()
		{
			// Complete original RVA 0x4320D0..0x43254A. Preserve original fixed-NPC indexing, rereads and SDK write ordering.
			if (!isLoadedData) return;
			int count = unchecked(ExistNpcList.Count - fixedNpcCount);
			while (count > global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.NpcSaveDataList.Count)
			    global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.NpcSaveDataList.Add(new NpcSaveData());
			int skipped = 0;
			for (int i = 0; i < global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.NpcSaveDataList.Count; i = unchecked(i + 1))
			{
			    if (i < count)
			    {
			        if (ExistNpcList[i].NpcComponent.NpcController.Nca.StateModel.CurrentFixedType == NpcStateModel.FixedType.None)
			        {
			            global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.NpcSaveDataList[i].IsExist = true;
			            NpcSaveData positionReceiver = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.NpcSaveDataList[i];
			            Vector3 position = ExistNpcList[unchecked(i - skipped)].Transform.position;
			            if (positionReceiver is null) throw new System.NullReferenceException();
			            positionReceiver.Position = position;
			            NpcSaveData rotationReceiver = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.NpcSaveDataList[i];
			            Quaternion rotation = ExistNpcList[unchecked(i - skipped)].Transform.rotation;
			            if (rotationReceiver is null) throw new System.NullReferenceException();
			            rotationReceiver.Rotation = rotation;
			        }
			        else skipped = unchecked(skipped + 1);
			    }
			    else global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.NpcSaveDataList[i].IsExist = false;
			}
		}

		public void PushInElevator(Bounds pushBounds, Vector3 pushV, bool isFixed = false)
		{
		}

		public GameObject CreateSightArea()
		{
			// Original complete RVA0x4328F0; genuine original manager fields and Unity generic Instantiate path.
			return UnityEngine.Object.Instantiate(sightAreaPrefab, sightAreaParent);
		}

		public void CollectMaxStrangeNpc(List<NpcController> list)
		{
		}
	}
}
