using ExposureUnnoticed2.Master.Stage;
using ExposureUnnoticed2.Object3D.NPC.Script;
using ExposureUnnoticed2.Object3D.NpcCar;
using ExposureUnnoticed2.Object3D.RoutePoint;
using ExposureUnnoticed2.Object3D.ScenePlops.Conbini;
using ExposureUnnoticed2.Object3D.ScenePlops.Elevator;
using ExposureUnnoticed2.Object3D.ScenePlops.TalkPoint;
using ExposureUnnoticed2.Object3D.ScenePlops.WallFog;
using ExposureUnnoticed2.Object3D.ToiletManager;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.Sound;
using ExposureUnnoticed2.Scripts.Tools;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class StageController : MonoBehaviour
	{
		[SerializeField]
		private BaseRouteBuilder baseRouteBuilder;

		[SerializeField]
		private BgmController bgmController;

		[SerializeField]
		private SkySetting daytimeSetting;

		[SerializeField]
		private SkySetting nightSetting;

		[SerializeField]
		private GameObject daytimeLight;

		[SerializeField]
		private GameObject nightLight;

		[SerializeField]
		private ReflectionProbeManager reflectionProbeManager;

		[SerializeField]
		private WallFogController[] wallFogControllers;

		[SerializeField]
		private TalkNpcManager talkNpcManager;

		[SerializeField]
		private NpcCarManager npcCarManager;

		[SerializeField]
		private SitNpcManager sitNpcManager;

		[SerializeField]
		private StandNpcManager standNpcManager;

		[SerializeField]
		private PinponNpcManager pinponNpcManager;

		[SerializeField]
		private ConbiniController conbiniController;

		[SerializeField]
		private ToiletManager toiletManager;

		[SerializeField]
		private ElevatorController elevatorController;

		[SerializeField]
		private Corevale_SwapLightmap lightmapChanger;

		[SerializeField]
		private GameObject[] mirrors;

		[SerializeField]
		public StageRandomPositionManager ItemRandomPositionManager;

		[SerializeField]
		public StageRandomPositionManager PlayerRandomPositionManager;

		[SerializeField]
		public PortalManager PortalManager;

		private bool isAlreadyInit;

		[field: SerializeField]
		public StageType StageType { get; private set; }

		[field: SerializeField]
		public Terrain Terrain { get; private set; }

		public GameObject[] Mirrors => mirrors;

		private void Start()
        {
            if (baseRouteBuilder) baseRouteBuilder.Initialize();
            Scheduler.DelayFlame(() =>
            {
                if (ItemRandomPositionManager) ItemRandomPositionManager.Initialize();
                if (PlayerRandomPositionManager) PlayerRandomPositionManager.Initialize();
                if (PortalManager) PortalManager.Initialize();
                UpdateReleaseFastTravel();
                InGameManager.Instance.CurrentStageController = this;
                if (!InGameManager.Instance.TempInGameState.IsFirstLoadStage)
                {
                    Events.CompleteTransitionStageEvent.StageController = this;
                    EventManager.Broadcast(Events.CompleteTransitionStageEvent);
                }
                InGameManager.Instance.TempInGameState.IsFirstLoadStage = false;
                InGameManager.Instance.InGameReferencer.MirrorManager.ReflectiveObjects = mirrors;
                InGameManager.Instance.InGameReferencer.MirrorManager.InitializeMirror();
                if (bgmController) bgmController.Initialize();
                Scheduler.DelayFlame(() =>
                {
                    if (talkNpcManager) talkNpcManager.Initialize();
                    if (npcCarManager) npcCarManager.Initialize();
                    if (sitNpcManager) sitNpcManager.Initialize();
                    if (standNpcManager) standNpcManager.Initialize();
                    if (conbiniController) conbiniController.Initialize();
                    if (elevatorController) elevatorController.Initialize();
                    if (pinponNpcManager) pinponNpcManager.Initialize();
                    if (toiletManager) toiletManager.Initialize();
                    if (lightmapChanger) lightmapChanger.SwapLightmapData(GameState.GameStateData.IsDaytime, true);
                    UpdateByDaytimeNight(true);
                    isAlreadyInit = true;
                }, 1);
            }, 1);
            EventManager.AddListener<ChangeTimeEvent>(OnChangeTime);
        }

		private void Update()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnChangeTime(ChangeTimeEvent evt)
		{
		}

		public Color CurrentFogColor()
		{
			return default;
		}

		public void UpdateByDaytimeNight(bool isInitialize = false)
		{
		}

		private void UpdateReleaseFastTravel()
		{
            // Recovered complete original native body.
            bool messageAdded = StageType == StageType.Apart;
            foreach (ExposureUnnoticed2.Master.FastTravel.RFastTravel record in ExposureUnnoticed2.Master.FastTravel.MFastTravel.CreateListByStage(StageType))
            {
                if (!GameState.GameStateData.ReleaseFastTravel.Contains(record.Id))
                {
                    GameState.GameStateData.ReleaseFastTravel.Add(record.Id);
                    if (!messageAdded)
                    {
                        ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(
                            Localizer.Trans("ui_fast_travel_add"),
                            ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType.Log);
                        messageAdded = true;
                    }
                }
            }
        }

		public Transform GetSpawnPoint(StageType fromStageType)
		{
// Complete original native body; retain original callback and failure order.
var transition = InGameManager.Instance.StageTransController;
if (transition is null) throw new System.NullReferenceException();
return PortalManager.GetSpawnPoint(fromStageType, transition.LastPortalId);
}

		public Transform GetEscapeTarget()
		{
		    // Original RVA 0x4A5150 inlines PortalManager.GetEscapeTarget (RVA 0x49A490).
		    // Preserve first-array-element selection and null/empty-array failures.
		    return PortalManager.GetEscapeTarget();
		}

		public float CurrentFogDensity()
		{
			return 0f;
		}
	}
}
