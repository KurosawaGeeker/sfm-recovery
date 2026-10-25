using ExposureUnnoticed2.Object3D.Camera;
using ExposureUnnoticed2.Object3D.NPC.Script;
using ExposureUnnoticed2.Scripts.InGame;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerController : MonoBehaviour, IUpdater, ILateUpdater
	{
		public static PlayerController Instance;

		[SerializeField]
		public PlayerAvatarObjectReferencer PlayerAvatarObjectReferencer;

		private InputResolver inputResolver;

		private PlayerClassAccessor pca;

		public FaceCameraController faceCameraController { get; private set; }

		[field: SerializeField]
		public GameObject BodyCameraObject { get; private set; }

		public PlayerClassAccessor Pca => pca;

		public PlayerStateModel PlayerState => pca.PlayerState;

		public Transform StrangeSightPoint => pca.PlayerAvatarObjectReferencer.StrangeSightPoint;

		public Transform StrangeSightPoint2 => pca.PlayerAvatarObjectReferencer.Hip;

		public Transform FaceCameraTarget => pca.PlayerAvatarObjectReferencer.FaceCameraTarget;

		public MentalManager Mental => pca.Mental;

		public StaminaManager Stamina => pca.Stamina;

		public PlayerHandcuffsController HandcuffsController => pca.PlayerHandcuffsController;

		public Transform CameraTarget => pca.PlayerAvatarObjectReferencer.CameraTarget;

		public void Initialize(FaceCameraController faceCameraController, TPCameraController tpCameraController, FPCameraController fpCameraController, BaseCameraController baseCameraController, DroppedItemManager droppedItemManager)
		{
			// Recovered from GameAssembly.dll RVA 0x643550
			Instance = this;
			this.faceCameraController = faceCameraController;
			inputResolver = new InputResolver();
			pca = new PlayerClassAccessor(this, droppedItemManager);
			pca.Initialize();
			pca.PlayerLocomotion.Initialize(tpCameraController, fpCameraController, baseCameraController, pca);
			ChangeLighting();
			foreach (AudioSource source in pca.PlayerObjectReferencer.AudioSources)
			{
			    foreach (AudioSource component in source.gameObject.GetComponents<AudioSource>())
			        component.outputAudioMixerGroup = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.AudioMixerEnvironment;
			}
			global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayFlame(() =>
			{
			    // Original <Initialize>b__30_0 RVA 0x645ED0.
			    global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ChangeBodyEvent);
			    pca.PlayerBodyCustomizeAdjuster.AdjustMoveSpeed();
			}, 1);
		}

		public void OnUpdate()
		{
			// SOURCE DRAFT: original complete 643A10..644FDE; eight inlined owners are candidate dependencies.
			InputData inputData = inputResolver.Resolve();
			pca.PlayerState.IsFixedPositionRotation = false;
			pca.PlayerState.IsFixedWarpPositionRotation = false;

			// Both destination state receivers are captured before the NPC callee can mutate PCA.
			var capturedNearState = pca.PlayerState;
			var capturedMissionState = pca.PlayerState;
			var nearest = InGameManager.Instance.InGameReferencer.NpcManager.GetNearNpcDistance(pca.Transform);
			if (capturedNearState is null) throw new System.NullReferenceException();
			capturedNearState.NearNpc = nearest.Item2;
			if (capturedMissionState is null) throw new System.NullReferenceException();
			capturedMissionState.NearNpcForMission = nearest.Item3;

			var capturedDistanceState = pca.PlayerState;
			float nearestDistance;
			if (pca.PlayerState.NearNpc is null)
			    nearestDistance = float.MaxValue;
			else
			{
			    // This is the ORIGINAL GameObject overload 4B2360, not Transform overload4B26E0.
			    var npcObject = pca.PlayerState.NearNpc.GameObject;
			    var playerObject = pca.GameObject;
			    System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
			    nearestDistance = global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DistSqrt(npcObject, playerObject);
			}
			if (capturedDistanceState is null) throw new System.NullReferenceException();
			capturedDistanceState.NearNpcDist = nearestDistance;

			if (pca.PlayerState.NearNpc is null)
			{
			    pca.PlayerState.IsNearNpcForMission = false;
			    pca.PlayerState.IsNearNpcForMissionSecondary = false;
			}
			else
			{
			    var capturedElevator = global::ExposureUnnoticed2.Object3D.ScenePlops.Elevator.ElevatorController.Instance;
			    System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(UnityEngine.Object).TypeHandle);
			    // Unity alive check only for Elevator. NPC references above use CLR null semantics.
			    if (capturedElevator != null)
			    {
			        var elevatorStateReceiver = global::ExposureUnnoticed2.Object3D.ScenePlops.Elevator.ElevatorController.Instance;
			        var elevatorState = elevatorStateReceiver.CurrentElevatorState;
			        if ((int)elevatorState == 4 || (int)elevatorState == 0)
			        {
			            var playerBoundsReceiver = global::ExposureUnnoticed2.Object3D.ScenePlops.Elevator.ElevatorController.Instance;
			            Vector3 playerPosition = pca.Transform.position;
			            if (playerBoundsReceiver is null) throw new System.NullReferenceException();
			            bool playerContained = playerBoundsReceiver.PushBounds.Contains(playerPosition);
			            var npcBoundsReceiver = global::ExposureUnnoticed2.Object3D.ScenePlops.Elevator.ElevatorController.Instance;
			            Vector3 npcPosition = pca.PlayerState.NearNpc.Transform.position;
			            if (npcBoundsReceiver is null) throw new System.NullReferenceException();
			            bool npcContained = npcBoundsReceiver.PushBounds.Contains(npcPosition);
			            if (playerContained != npcContained)
			            {
			                pca.PlayerState.IsNearNpcForMission = false;
			                pca.PlayerState.IsNearNpcForMissionSecondary = false;
			                goto CommonManagers;
			            }
			        }
			    }
			    if (pca.PlayerState.NearNpcForMission is null)
			    {
			        pca.PlayerState.IsNearNpcForMission = false;
			        pca.PlayerState.IsNearNpcForMissionSecondary = false;
			    }
			    else
			    {
			        Vector3 playerPosition = pca.Transform.position;
			        Vector3 missionPosition = pca.PlayerState.NearNpcForMission.Transform.position;
			        float dx = playerPosition.x - missionPosition.x;
			        float dz = playerPosition.z - missionPosition.z;
			        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
			        // Outlined native magnitude3D4630 receives Vector3(dx,+0,dz).
			        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(System.Math).TypeHandle);
			        float horizontalDistance = (float)System.Math.Sqrt((double)((0f + dx * dx) + dz * dz));
			        Vector3 freshPlayerPosition = pca.Transform.position;
			        Vector3 freshMissionPosition = pca.PlayerState.NearNpcForMission.Transform.position;
			        float verticalDistance = System.Math.Abs(freshPlayerPosition.y - freshMissionPosition.y);
			        pca.PlayerState.IsNearNpcForMission = horizontalDistance < 1.5f && verticalDistance < 2f;
			        pca.PlayerState.IsNearNpcForMissionSecondary = horizontalDistance < 3f && verticalDistance < 2f;
			    }
			}

			CommonManagers:
			pca.Stamina.OnUpdate();
			pca.MoistureManager.OnUpdate();
			pca.PlayerEcstasyController.OnUpdate();
			pca.PlayerStrangenessController.OnUpdate();
			pca.PlayerMiscProcess.OnUpdate(inputData);
			switch ((int)pca.PlayerState.CurrentState)
			{
			    case 0:
			        if (!InGameManager.Instance.TempInGameState.IsGameOver)
			        {
			            pca.PlayerLocomotion.OnUpdate(inputData);
			            pca.PlayerPeeController.OnUpdate(inputData);
			            pca.PlayerClothesAction.OnUpdate(inputData);
			            // Independently drafted original64E5B0 preserves a SECOND game-over read.
			            pca.PlayerHandcuffsController.OnUpdate(inputData);
			            pca.PlayerAdultGoodsController.OnUpdate();
			        }
			        // Original Idle action runs even when the first game-over read is true.
			        pca.PlayerAction.OnUpdate(inputData);
			        break;
			    case 1:
			        pca.PlayerBaretaState.OnUpdate();
			        break;
			    case 2:
			        pca.PlayerBaretaEscapeState.OnUpdate();
			        break;
			    // Unknown integer states still execute the complete common suffix.
			}
			pca.PlayerReinforceEffectManager.OnUpdate(); // Proven native noop, required CLR receiver check retained.
			pca.PlayerClothes.OnUpdate(); // Independently drafted original6409F0, real IClothesController.OnUpdate dispatch.
			pca.PlayerHeartBeat.OnUpdate();
			pca.PlayerFaceController.OnUpdate();
			pca.PlayerTitController.OnUpdate();
			pca.PlayerFaceShadowOnlyController.OnUpdate();
			pca.PlayerPantsController.OnUpdate();
			pca.PlayerFootStepController.OnUpdate();
			pca.PlayerShrinkWakiController.OnUpdate();
			pca.PlayerBreathController.OnUpdate();
			pca.PlayerBodyMaterialController.OnUpdate();
			pca.FutanariController.OnUpdate();
			pca.PlayerShiofukiController.OnUpdate();
			pca.BodyPaintController.OnUpdate();
			pca.PistonMachineController.OnUpdate();
			pca.PlayerMoveCalculator.OnUpdate();
			pca.PlayerRpBonusCalculator.OnUpdate();
			pca.PlayerAvatarOffsetManager.OnUpdate(); // Proven native noop, required CLR receiver check retained.
			var transientState = pca.PlayerState;
			if (transientState is null) throw new System.NullReferenceException();
			transientState.TransPick = false;
			transientState.TransPickClothes = false;
			transientState.TransAction = (global::ExposureUnnoticed2.Master.Action.ActionType)(-1);
			pca.PlayerAnimationManager.OnUpdate(inputData);
			pca.PlayerAction.OnFrameEnd();
			// Capture destination receiver before each real SDK or facade callee. PCA re-reads are deliberate.
			var positionState = pca.PlayerState;
			Vector3 cachedPosition = pca.Transform.position;
			if (positionState is null) throw new System.NullReferenceException();
			positionState.CacheForNpcPosition = cachedPosition;
			var forwardState = pca.PlayerState;
			Vector3 cachedForward = pca.Transform.forward;
			if (forwardState is null) throw new System.NullReferenceException();
			forwardState.CacheForNpcForward = cachedForward;
			var exposedState = pca.PlayerState;
			var exposed = PlayerFacade.Instance.GetExposedBodyPartsByClothesAndCostume();
			if (exposedState is null) throw new System.NullReferenceException();
			exposedState.CacheForNpcExposedBodyParts = exposed;
		}

		public void OnFixedUpdate()
		{
			// Recovered from GameAssembly.dll RVA 0x644FE0
			// Original644FE0: preserve early return and null dereference order; no Unity fake-null checks.
			if (pca.PlayerState.CurrentState != PlayerStateModel.PlayerState.Idle) return;
			if (global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.IsGameOver) return;
			// Original re-reads pca here and tail-jumps to the complete Locomotion fixed frame.
			pca.PlayerLocomotion.OnFixedUpdate();
		}

		public void ChangeState(PlayerStateModel.PlayerState nextState)
		{
// Complete original native body; original gates, state order and failure prefixes retained.
if (pca.PlayerState.CurrentState == PlayerStateModel.PlayerState.Bareta)
    pca.PlayerBaretaState.OnExit();
pca.PlayerState.CurrentState = nextState;
if (nextState == PlayerStateModel.PlayerState.Bareta)
{
    var navAgent = pca.NavAgent;
    int stage = (int)GameState.GameStateData.CurrentStageType;
    int mask = stage == 10 || unchecked((uint)(stage - 11)) < 2u ? 8 : -1;
    if (navAgent is null) throw new System.NullReferenceException();
    navAgent.areaMask = mask;
    pca.PlayerAnimationManager.SetEcstasy(false);
    pca.PlayerBaretaState.OnEnter();
}
else if (nextState == PlayerStateModel.PlayerState.BaretaEscape)
{
    pca.PlayerAnimationManager.SetEcstasy(false);
    pca.NavAgent.updatePosition = true;
    pca.NavAgent.updateRotation = true;
}
}

		public void Dispose()
		{
            // Original RVA 0x6453C0 uses Unity object equality, then CLR-null PCA.
            if (this != Instance) return;
            pca?.Dispose();
		}

		public void ChangeLighting()
		{
			// Recovered from GameAssembly.dll RVA 0x645510
			pca.PlayerReceiveShadowChanger.SetReceiveShadow(GameState.GameStateData.IsDaytime);
		}

		public void OnGameOverEvent()
		{
// Complete original RVA 0x645580; PE/typed identity is audited in this owned stage.
pca.Animator.speed = 0f;
}

		public void Warp(Vector3 position, Quaternion rotation)
		{
			// Recovered from GameAssembly.dll RVA 0x6455F0
			pca.CharacterController.enabled = false;
			transform.position = position;
			transform.rotation = rotation;
			pca.CharacterController.enabled = true;
			pca.Rigidbody.velocity = Vector3.zero;
			pca.PlayerLocomotion.OnWarp();
			pca.PlayerMoveCalculator.OnWarp();
		}

		public void OnEmergencyEscape()
		{
		}

		public void OnFoundStrangerEvent(NpcController foundNpc)
		{
// Complete original native body; original gates, state order and failure prefixes retained.
static void Action()
{
    global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayScaledTime(() =>
    {
        GameState.GameStateData.PlayerLife--;
        GameState.GameStateData.PlayerLife = Mathf.Max(0, GameState.GameStateData.PlayerLife);
    }, 0.8f);
}
if (GameStateFacade.IsActivePassiveSkill((global::ExposureUnnoticed2.Master.Skill.SkillType)21) ||
    GameStateFacade.IsActivePassiveSkill((global::ExposureUnnoticed2.Master.Skill.SkillType)22)) return;
if (pca.PlayerState.timeStopStrangeMaxNpcList.Contains(foundNpc)) return;
var baretaState = pca.PlayerBaretaState;
if (baretaState is null) throw new System.NullReferenceException();
if (baretaState.FoundNpc is null) baretaState.FoundNpc = foundNpc;
if (pca.PlayerState.CurrentState == PlayerStateModel.PlayerState.Bareta) return;
if (GameStateFacade.IsActivePassiveSkill((global::ExposureUnnoticed2.Master.Skill.SkillType)12))
    global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayCondition(() => { Action(); ChangeState(PlayerStateModel.PlayerState.Bareta); },
        () => global::ExposureUnnoticed2.Scripts.InGame.SlowAssistManager.Instance.currentState == global::ExposureUnnoticed2.Scripts.InGame.SlowAssistManager.State.Normal, 10f);
else
{
    Action();
    ChangeState(PlayerStateModel.PlayerState.Bareta);
}

}

		public void OnLateUpdate()
		{
pca.PlayerToeHeelController.OnLateUpdate();
pca.PlayerKneeAdjuster.OnLateUpdate();
pca.LookAtController.OnLateUpdate();
faceCameraController.OnLateUpdate();
pca.PlayerFaceController.OnLateUpdate();
pca.PlayerBodyCustomizeAdjuster.OnLateUpdate();
pca.FutanariController.OnLateUpdate();
pca.PlayerAvatarOffsetManager.OnLateUpdate();
pca.PlayerBreathController.OnLateUpdate();
pca.PlayerState.EscalatorPushVThisFrame = Vector3.zero;
}

		public void OnLateUpdateAfterCamera()
		{
		}
	}
}
