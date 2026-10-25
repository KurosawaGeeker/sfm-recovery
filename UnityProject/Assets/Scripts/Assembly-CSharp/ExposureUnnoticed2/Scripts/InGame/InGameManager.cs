using ExposureUnnoticed2.Object3D.SpermBullet;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.Mission;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame
{
	[RequireComponent(typeof(InGameReferencer))]
	[RequireComponent(typeof(EventMethodManager))]
	public class InGameManager : MonoBehaviour, IUpdater, ILateUpdater
	{
		public static InGameManager Instance;

		private static bool isExist;

		private EventMethodManager eventMethodManager;

		private GameStateChecker gameStateChecker;

		private InGameEventListener inGameEventListener;

		private InteractManager interactManager;

		private SpermDecalManager spermDecalManager;

		private StatisticsManager statisticsManager;

		public DroppedItemManager DroppedItemManager { get; private set; }

		public InGameReferencer InGameReferencer { get; private set; }

		public TemporaryInGameState TempInGameState { get; private set; }

		public MissionManager MissionManager { get; private set; }

		public StageController CurrentStageController { get; set; }

		public VisibilityFogController VisibilityFogController { get; private set; }

		public DroneManager DroneManager { get; private set; }

		public RandomMissionManager RandomMissionManager { get; private set; }

		public StageTransController StageTransController { get; private set; }

		public GameStateApplier GameStateApplier { get; private set; }

		public SexManager SexManager { get; private set; }

		private void Start()
		{
			// Recovered from GameAssembly.dll RVA 0x495DC0
			if (isExist)
			{
			    UnityEngine.Object.Destroy(gameObject);
			    return;
			}
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			isExist = true;
			Instance = this;
			if (!global::ExposureUnnoticed2.Scripts.OtherScene.SpringboardLoadEmpty.IsPathSpringboardOnce)
			    SaveManager.Load(SaveRapper.SaveDataType.NewGame, 0);
			Application.targetFrameRate = GameState.OptionData.MaxFrameRate;
			DebugController.FirstInitialize();
			StageTransController = new StageTransController();
			DroppedItemManager = new DroppedItemManager();
			gameStateChecker = new GameStateChecker();
			GameStateApplier = new GameStateApplier();
			TempInGameState = new TemporaryInGameState();
			inGameEventListener = new InGameEventListener();
			interactManager = new InteractManager();
			MissionManager = new MissionManager();
			VisibilityFogController = new VisibilityFogController();
			DroneManager = new DroneManager();
			RandomMissionManager = new RandomMissionManager();
			spermDecalManager = new SpermDecalManager();
			statisticsManager = new StatisticsManager();
			InGameReferencer = GetComponent<InGameReferencer>();
			InGameReferencer.Initialize();
			InGameReferencer.Player = UnityEngine.Object.Instantiate(
			    global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.Player,
			    transform).GetComponent<global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController>();
			eventMethodManager = GetComponent<EventMethodManager>();
			eventMethodManager.Initialize(InGameReferencer);
			VisibilityFogController.Initialize();
			interactManager.Initialize();
			MissionManager.Initialize();
			inGameEventListener.Initialize(GameStateApplier);
			GameStateApplier.Initialize(InGameReferencer, this);
			InGameReferencer.InGameUiManager.InitializeBeforePlayer();
			InGameReferencer.Player.Initialize(InGameReferencer.FaceCameraController,
			    InGameReferencer.TpCameraController,
			    InGameReferencer.Player.PlayerAvatarObjectReferencer.FPCameraController,
			    InGameReferencer.BaseCameraController, DroppedItemManager);
			InGameReferencer.SelfCameraParent.transform.parent = InGameReferencer.Player.Pca.Transform;
			InGameReferencer.SelfCameraParent.transform.localPosition = Vector3.zero;
			InGameReferencer.SelfCameraParent.transform.localRotation = Quaternion.identity;
			InGameReferencer.FaceCameraController.Initialize();
			InGameReferencer.InGameUiManager.Initialize(InGameReferencer.Player);
			InGameReferencer.BaseCameraController.Initialize(InGameReferencer.Player,
			    InGameReferencer.Player.Pca.PlayerState, InGameReferencer.TpCameraController,
			    InGameReferencer.Player.PlayerAvatarObjectReferencer.FPCameraController,
			    InGameReferencer.InventoryCameraController);
			InGameReferencer.NpcManager.Initialize(InGameReferencer.Player);
			InGameReferencer.GameOverController.Initialize();
			InGameReferencer.SlowAssistManager.Initialize();
			InGameReferencer.UiPlayerController.Initialize();
			DroneManager.Initialize();
			RandomMissionManager.Initialize();
			spermDecalManager.Initialize();
			InGameReferencer.UiOptionApplyer.Initialize(InGameReferencer.FaceCameraController.gameObject,
			    InGameReferencer.BodyCameraObject);
			InGameReferencer.InGameGraphicsOptionApplyer.Initialize();
			SexManager.Initialize();
			gameStateChecker.Initialize(InGameReferencer.NpcManager);
			Events.CursorVisibleEvent.IsVisible = false;
			EventManager.Broadcast(Events.CursorVisibleEvent);
			if (GameObject.Find("Level").GetComponent<StageController>().StageType == GameState.GameStateData.CurrentStageType)
			    InGameReferencer.Player.Warp(GameState.GameStateData.PlayerPosition, GameState.GameStateData.PlayerRotation);
			else
			    GameState.GameStateData.CurrentStageType = GameObject.Find("Level").GetComponent<StageController>().StageType;
			InGameReferencer.TpCameraController.ResetToPlayerDirect();
			Scheduler.DelayFlame(() =>
			{
			    // Original <Start>b__52_0 RVA 0x498AF0.
			    if (DebugController.IsForceCreateNpc)
			        InGameReferencer.NpcManager.OnLoadStage();
			    else
			        InGameReferencer.NpcManager.OnLoadData();
			    global::ExposureUnnoticed2.ObjectUI.StageNamePanel.StageNamePanelView.Open(
			        global::ExposureUnnoticed2.Master.Stage.MStage.Get(CurrentStageController.StageType));
			    if (GameState.GameStateData.CurrentStageType == (global::ExposureUnnoticed2.Master.Stage.StageType)0)
			        Instance.InGameReferencer.Player.Pca.Mental.RecoveryMental();
			}, 2);
			InputRebindSaveManager.Instance.Load();
			DebugController.Initialize();
			global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.CensorPussy.SetFloat(
			    "_Pixelation", SystemManager.IsFanza ? 0.01f : 0.0069f);
			global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.CensorChinpo.SetFloat(
			    "_Pixelation", SystemManager.IsFanza ? 0.01f : 0.0043f);
			if (SystemManager.IsFanza)
			{
			    var camera = InGameReferencer.BodyCameraObject.GetComponent<UnityEngine.Camera>();
			    camera.cullingMask = camera.cullingMask | (1 << LayerAccessor.CensorFaceCamera);
			}
		}

		private void OnDestroy()
		{
            // Original RVA 0x497E90: only the owning Unity instance disposes its graph.
            if (this != Instance) return;
            inGameEventListener?.Dispose();
            interactManager?.Dispose();
            MissionManager?.Dispose();
            InGameReferencer?.GameOverController?.Dispose();
            DroppedItemManager?.Dispose();
            // The referencer is optional here; an existing referencer's Player is required.
            InGameReferencer?.Player.Dispose();
            InGameReferencer?.SlowAssistManager.Dispose();
            // Original strict suffix: preserve required receivers and call order.
            InGameReferencer.UiPlayerController.Dispose();
            VisibilityFogController.Dispose();
            spermDecalManager.Dispose();
            SexManager.Dispose();
		}

		public void OnUpdate()
		{
			// Recovered from GameAssembly.dll RVA 0x4985B0
			gameStateChecker.OnUpdate();
			VisibilityFogController.OnUpdate();
			DroneManager.OnUpdate();
			RandomMissionManager.OnUpdate();
			spermDecalManager.OnUpdate();
			statisticsManager.OnUpdate();
		}

		public void OnLateUpdate()
		{
			// Recovered from GameAssembly.dll RVA 0x498910
			MissionManager.OnLateUpdate();
			GameStateApplier.OnLateUpdate(false);
			InGameReferencer.GameOverController.OnLateUpdate();
		}

		public void OnExitInGameEvent()
		{
			// Recovered from GameAssembly.dll RVA 0x498960
			isExist = false;
			UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, UnityEngine.SceneManagement.SceneManager.GetActiveScene());
		}
	// Recovered original member RVA 0x498A20: .ctor
		public InGameManager()
        {
            SexManager = new SexManager();
        }
	}
}
