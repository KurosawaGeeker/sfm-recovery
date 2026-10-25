using ExposureUnnoticed2.Master.Stage;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Portal
{
	public class PortalController : MonoBehaviour
	{
		[SerializeField]
		private StageType stageType;

		[SerializeField]
		private int id;

		[SerializeField]
		private GameObject effectObject;

		[SerializeField]
		private GameObject effectObjectNoEntry;

		[SerializeField]
		private bool isHideInNight;

		[SerializeField]
		private bool isHideTrial;

		private float lastChangeColorTime;

		private bool isInArea;

		[field: SerializeField]
		public Transform SpawnPoint { get; private set; }

		public StageType StageType => stageType;

		public int Id => id;

		public bool CanEntryByRank { get; private set; }

		private void Start()
		{
            EventManager.AddListener<ChangeTimeEvent>(OnChangeTime, false);
            Scheduler.DelayFlame(() => OnChangeTime(null), 1);
            if (isHideTrial && SystemManager.IsTrial) gameObject.SetActive(false);
		}

		private void OnDestroy()
		{
            EventManager.RemoveListener<ChangeTimeEvent>(OnChangeTime);
		}

		private void OnTriggerEnter(Collider collider)
		{
            if (isInArea) return;
            if (collider.gameObject != global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.GameObject) return;
            if (global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.IsGameOver) return;
            if (global::ExposureUnnoticed2.Scripts.InGame.SexManager.Instance.IsSexMode) return;
            isInArea = true;
            var stage = MStage.Get(stageType);
            int actionType = (int)global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CurrentAction.Type;
            if (actionType > 10029 && actionType < 10099) return;
            if (global::ExposureUnnoticed2.Scripts.InGame.SceneProcessManager.LaundryManager.Instance != null && global::ExposureUnnoticed2.Scripts.InGame.SceneProcessManager.LaundryManager.Instance.IsInClothes())
            {
                global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("log_portal_cant_move_washer"));
                return;
            }
            if (global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CurrentRank.Rank < stage.EnterRankLimit)
            {
                global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("log_portal_cant_move_rank", Localizer.Trans(stage.StageNameKey)));
                return;
            }
            if (global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CurrentClothes.ClothesType == global::ExposureUnnoticed2.Master.Clothes.ClothesType.None && global::ExposureUnnoticed2.Scripts.InGame.DroppedItemManager.Instance.GetDropClothesInSameArea() == null && global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType == StageType.Apart)
            {
                global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("log_portal_cant_move_naked_externalCoat"));
                return;
            }
            if (global::ExposureUnnoticed2.Scripts.InGame.DroneManager.Instance.IsActiveMission)
            {
                global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("log_portal_cant_move_drone"));
                return;
            }
            if (global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.IsTimeStop)
            {
                global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("log_portal_cant_move_timestop"));
                return;
            }
            if (global::ExposureUnnoticed2.Scripts.InGame.RandomMissionManager.Instance.IsActiveMission)
            {
                global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(Localizer.Trans("log_portal_cant_move_random_mission"));
                return;
            }
            global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.StageTransController.TransStageIfInCondition(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType, stageType, id);
		}

		private void OnTriggerExit(Collider collider)
		{
            if (collider.gameObject != global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.GameObject) return;
            if (!global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.IsGameOver) isInArea = false;
		}

		private void Update()
		{
            if (!CanEntryByRank && Time.time - lastChangeColorTime >= 1f)
            {
                lastChangeColorTime = Time.time;
                OnChangeTime(null);
            }
		}

		private void OnChangeTime(ChangeTimeEvent evt)
		{
            var stage = MStage.Get(stageType);
            CanEntryByRank = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CurrentRank.Rank >= stage.EnterRankLimit;
            if (CanEntryByRank)
            {
                effectObjectNoEntry.SetActive(false);
                effectObject.SetActive(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime || !isHideInNight);
            }
            else
            {
                effectObject.SetActive(false);
                effectObjectNoEntry.SetActive(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime || !isHideInNight);
            }
		}
	}
}
