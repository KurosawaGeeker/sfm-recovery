using ExposureUnnoticed2.Object3D.NPC.Script.State;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcController
	{
		private static int ida;

		public int id;

		private CharacterController characterController;

		private NpcStateModel stateModel;

		private NpcFoundStrangerState npcFoundStrangerState;

		private NpcFadeOutState npcFadeOutState;

		private NpcConbiniState npcConbiniState;

		private NpcFashionClerkSubState fashionClerkSubState;

		private bool isInCameraArea;

		private float animatorSpeedCache = 1f;

		private float lastCalcIsInCameraAreaTime;

		public bool isDisplay = true;

		private float lastCalcDisplayTime;

		private int a;

		public bool IsExistObject => Transform;

		public bool IsAlive => NpcComponent;

		public Transform Transform { get; set; }

		public GameObject GameObject { get; set; }

		public bool IsVisible => Nca.AvatarComponent.IsVisible && isInCameraArea;

		public bool IsInCameraArea => isInCameraArea;

		public float StrangenessValue => Nca.StrangenessManager.Strangeness.Value;

		public bool IsRiseStrangeness { get; set; }

		public Transform EyePosition => Nca.PlayerSearcher.transform;

		public bool IsWatchingStranger => Nca.PlayerSearcher.IsPlayerFound && stateModel.IsAlreadyFoundStranger;

		public bool IsPlayerFound => Nca.PlayerSearcher.IsPlayerFound;

		public float PlayerDistance { get; private set; }

		public NpcComponent NpcComponent { get; }

		public NpcComponentAccessor Nca { get; private set; }

		public NpcController(NpcComponent npcComponent)
		{
			// Original complete .ctor RVA0x426ED0: preserve SDK ordering and re-read original component field.
			NpcComponent = npcComponent;
			GameObject = NpcComponent.gameObject;
			Transform = NpcComponent.transform;
		}

		public void Initialize(PlayerController player, NpcSaveData npcSaveData, bool isRandomPathSpawn, NpcStateModel.FixedType type)
		{
			id = ida;
			ida = unchecked(ida + 1);
			NpcComponent.id = id;
			characterController = NpcComponent.GetComponent<CharacterController>();
			stateModel = new NpcStateModel();
			stateModel.CurrentFixedType = type;
			npcFoundStrangerState = new NpcFoundStrangerState();
			npcFadeOutState = new NpcFadeOutState();
			npcConbiniState = new NpcConbiniState();
			fashionClerkSubState = new NpcFashionClerkSubState();
			Nca = new NpcComponentAccessor(this, NpcComponent, stateModel, player);
			Nca.Initialize();
			Nca.NavAgent.avoidancePriority = UnityEngine.Random.Range(0, 100);
			Nca.AnimationEventReceiver.Initialize(Nca);
			npcFoundStrangerState.Initialize(this, Nca);
			npcFadeOutState.Initialize(Nca);
			npcConbiniState.Initialize(Nca);
			Nca.LookAtController.InitializeForNpc(Nca);
			Nca.StateModel.MoveSpeedScale = (UnityEngine.Random.value * 0.4f + 1f) - 0.3f;
			Nca.StateModel.isSumahoWalk = type == NpcStateModel.FixedType.None && UnityEngine.Random.value <= 0.1f;
			Nca.StateModel.TurnNoisyLevel = UnityEngine.Random.Range(0.4f, 0.7f);
			Nca.StateModel.StrangeLookAt = UnityEngine.Random.Range(0.3f, 0.5f);
			Nca.StrangenessManager.Strangeness.CurrentReduceParSecond = UnityEngine.Random.Range(0.1f, 0.4f);
			GameObject phone = Nca.NpcComponent.Smartphone.gameObject;
			phone.SetActive(Nca.StateModel.isSumahoWalk);
			GameObject light = Nca.NpcComponent.SmartphoneLight.gameObject;
			bool activeLight = !global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime && (int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType != 11 && (int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType != 7;
			light.SetActive(activeLight);
			Nca.Animator.speed = Nca.StateModel.MoveSpeedScale;
			Nca.PlayerSearcher.RegisterTraceTransform(Nca.LookAtController.Head);
			Nca.StateModel.IsActiveSpermCollider = true;
			if (type == NpcStateModel.FixedType.None)
			{
			    Nca.StateModel.RouteData = global::ExposureUnnoticed2.Object3D.RoutePoint.BaseRouteBuilder.Instance.CreateRoute(Nca.StateModel.isFemale);
			    if (npcSaveData == null)
			    {
			        if (isRandomPathSpawn) Nca.StateModel.RouteData.ProceedRandom();
			        Transform appear = Nca.StateModel.RouteData.AppearPoint.transform;
			        Vector3 position = appear.position;
			        Quaternion rotation = appear.rotation;
			        Warp(position, rotation);
			        if ((int)Nca.StateModel.RouteData.AppearPoint.Type == 1) Nca.NpcSoundController.PlaySe((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)53);
			        if ((int)Nca.StateModel.RouteData.AppearPoint.Type == 4) Nca.NpcSoundController.PlaySe((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)71);
			        if ((int)Nca.StateModel.RouteData.AppearPoint.Type == 5) Nca.NpcSoundController.PlaySe((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)71);
			        if ((int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType == 6) global::ExposureUnnoticed2.Object3D.ScenePlops.Conbini.ConbiniController.Instance.PlayEnterSoundIfInCondition();
			    }
			    else Warp(npcSaveData.Position, npcSaveData.Rotation);
			}
			else if (type == NpcStateModel.FixedType.Conbini)
			{
			    Nca.StateModel.RouteData = global::ExposureUnnoticed2.Object3D.RoutePoint.BaseRouteBuilder.Instance.CreateRoute2();
			    Transform appear = Nca.StateModel.RouteData.AppearPoint.transform;
			    Vector3 position = appear.position;
			    Quaternion rotation = appear.rotation;
			    Warp(position, rotation);
			    if ((int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType == 7) fashionClerkSubState.Initialize(Nca);
			}
			Nca.NpcLocomotion.SetActiveNavAgent(true);
			SmoothFloat sound = stateModel.SoundCancelAreaScale;
			sound.Current = 1f;
			sound.Target = 1f;
			global::ExposureUnnoticed2.ObjectUI.NpcUi.NpcUiView.Create(Nca);
			global::ExposureUnnoticed2.ObjectUI.InGame.NpcDirectIconPanel.NpcDirectionPanelView.Register(this);
			Scheduler.DelayFlame(() => Nca.NpcAnimationManager.SetSpeed(1f), 3);
			EventManager.AddListener<TimeStopEvent>(OnTimeStopEvent, false);
		}

		public void AdditionalInitializeForFixAction(NpcStateModel.FixedType type)
		{
            NpcController npc = this;
            if (type != NpcStateModel.FixedType.Conbini) npc.Nca.NavAgent.enabled = false;
            if (type == NpcStateModel.FixedType.Talk || type == NpcStateModel.FixedType.Car || type == NpcStateModel.FixedType.Pinpon)
            {
                if (Nca.StateModel.IsHeadSet)
                {
                    Nca.StateModel.IsHeadSet = false;
                    Nca.AvatarComponent.Headset.gameObject.SetActive(false);
                }
                if (type == NpcStateModel.FixedType.Pinpon) Nca.StateModel.IsHidePinponNpc = true;
            }
            Scheduler.DelayFlame(() =>
            {
                npc.Nca.NpcAnimationManager.SetSpeed(0f);
                if (type == NpcStateModel.FixedType.SitBook)
                {
                    Nca.NpcComponent.Book.gameObject.SetActive(true);
                    Nca.NpcAnimationManager.SetSitBook();
                    Nca.NpcAnimationManager.SetIsSit(true);
                }
                if (type == NpcStateModel.FixedType.SitSumaho)
                {
                    Nca.NpcComponent.Smartphone.gameObject.SetActive(true);
                    Nca.NpcAnimationManager.SetSitSumaho();
                    Nca.NpcAnimationManager.SetIsSit(true);
                }
                if (type == NpcStateModel.FixedType.SitSleep)
                {
                    Nca.NpcAnimationManager.SetSitSleep();
                    Nca.NpcAnimationManager.SetIsSit(true);
                }
                if (type == NpcStateModel.FixedType.Conbini)
                {
                    npc.Nca.NpcAnimationManager.SetSpeed(1f);
                    npc.Nca.NavAgent.enabled = true;
                }
                if (type == NpcStateModel.FixedType.Talk)
                {
                    NpcComponentAccessor rotationNca = Nca;
                    Transform rotationTransform = Transform;
                    NpcStateModel rotationState = rotationNca.StateModel;
                    Quaternion rotation = rotationTransform.rotation;
                    if (rotationState == null) throw new System.NullReferenceException();
                    rotationState.initTalkRotation = rotation;
                }
            }, 3);
        }

		public void OnUpdateAlsoGameOver()
		{
			if (Nca.StateModel.IsDestroy) return;
			if (0.5f < Time.unscaledTime - lastCalcIsInCameraAreaTime)
			{
			    lastCalcIsInCameraAreaTime = Time.unscaledTime;
			    GameObject cameraObject = ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.BaseCameraController.gameObject;
			    GameObject npcObject = Nca.GameObject;
			    Vector3 direction = ExternalMethodManager.Direct(cameraObject, npcObject);
			    Vector3 forward = ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.BaseCameraController.transform.forward;
			    float dot = (direction.y * forward.y + direction.x * forward.x) + direction.z * forward.z;
			    isInCameraArea = 0.3d <= (double)dot;
			}
		}

		public void OnFixedUpdate()
		{
            NpcStateModel.State current = stateModel.CurrentState;
            if (current == NpcStateModel.State.Idle || current == NpcStateModel.State.TurnAround)
                Nca.NpcLocomotion.OnFixedUpdate();
            else if (current == NpcStateModel.State.Interact)
            {
                int interact = (int)stateModel.CurrentRouteInteractType;
                if (interact == 12 || interact == 13 || interact == 14)
                    Nca.NpcLocomotion.OnFixedUpdate();
            }
            else if (current == NpcStateModel.State.TraceSexPlayer && !global::ExposureUnnoticed2.Scripts.InGame.SexManager.Instance.IsSexMode)
                Nca.NpcLocomotion.OnFixedUpdate();
        }

		public void OnUpdateOnTimeStop()
		{
			npcFadeOutState.UpdateFadeIn();
			Nca.NpcAnimatorFpsController.OnUpdate();
		}

		public void OnUpdateEarly()
		{
			NpcStateModel destination = stateModel;
			Vector3 position = Nca.Transform.position;
			if (destination == null) throw new System.NullReferenceException();
			destination.CurrentPosition = position;
		}

		public void OnUpdate()
		{
			if (Nca.StateModel.IsDestroy) return;
			a = unchecked(a + 1);
			if (ExposureUnnoticed2.Scripts.InGame.ObjectCulling.ObjectCullerManager.Instance != null)
			{
			    bool farInMallDay = false;
			    if ((int)ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType == 11 && ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime)
			    {
			        Vector3 playerPosition = PlayerController.Instance.Pca.PlayerState.CacheForNpcPosition;
			        Vector3 npcPosition = stateModel.CurrentPosition;
			        float dx = playerPosition.x - npcPosition.x;
			        float dy = playerPosition.y - npcPosition.y;
			        float dz = playerPosition.z - npcPosition.z;
			        farInMallDay = 1600f <= (dy * dy + dx * dx) + dz * dz;
			    }
			    if (farInMallDay) isDisplay = false;
			    else if (0.5f < Time.unscaledTime - lastCalcDisplayTime)
			    {
			        lastCalcDisplayTime = Time.unscaledTime;
			        isDisplay = !ExposureUnnoticed2.Scripts.InGame.ObjectCulling.ObjectCullerManager.Instance.IsCulling(NpcComponent);
			    }
			}
			if (Nca.StateModel.CurrentFixedType == NpcStateModel.FixedType.Pinpon) isDisplay = !Nca.StateModel.IsHidePinponNpc;
			if (!ExposureUnnoticed2.Scripts.InGame.SexManager.Instance.IsSexMode || ExposureUnnoticed2.Scripts.InGame.SexManager.Instance.ActiveTraceSexPlayerNpc != this)
			    Nca.AvatarComponent.MeshParent.SetActive(isDisplay);
			Nca.PlayerSearcher.gameObject.SetActive(isDisplay);
			if (!isDisplay && a % 3 != 0) return;
			NpcStateModel distanceDestination = stateModel;
			GameObject npcObject = Nca.GameObject;
			GameObject playerObject = PlayerController.Instance.Pca.GameObject;
			float distance = ExternalMethodManager.DistSqrt(npcObject, playerObject);
			if (distanceDestination == null) throw new System.NullReferenceException();
			distanceDestination.PlayerDist = distance;
			Nca.PlayerSearcher.OnUpdate();
			Nca.StrangenessManager.OnUpdate();
			Nca.NpcMiscController.OnUpdate();
			npcFadeOutState.UpdateFadeIn();
			Nca.NpcAnimatorFpsController.OnUpdate();
			Nca.NpcSoundController.OnUpdate();
			PlayerDistance = ExternalMethodManager.DistSqrt(Nca.Player.gameObject, GameObject);
			if (Nca.StateModel.CurrentFixedType == NpcStateModel.FixedType.Car)
			{
			    Nca.AvatarComponent.MeshParent.SetActive(false);
			    goto Threshold;
			}
			if (Nca.StateModel.CurrentFixedType == NpcStateModel.FixedType.SitSleep)
			{
			    Nca.NpcSleepFixState.OnUpdate(false);
			    goto Threshold;
			}
			NpcStateModel.FixedType fixedType = Nca.StateModel.CurrentFixedType;
			if (fixedType == NpcStateModel.FixedType.Talk || fixedType == NpcStateModel.FixedType.SitBook || fixedType == NpcStateModel.FixedType.SitSumaho || fixedType == NpcStateModel.FixedType.Pinpon)
			{
			    switch (stateModel.CurrentState)
			    {
			        case NpcStateModel.State.Idle:
			            Nca.NpcAnimationManager.SetSpeed(0f);
			            if (Nca.StateModel.CurrentFixedType == NpcStateModel.FixedType.Talk)
			            {
			                Nca.NpcAnimationManager.SetSpeed(0f);
			                Transform destination = Nca.Transform;
			                Quaternion rotation = Nca.Transform.rotation;
			                Quaternion target = Nca.StateModel.initTalkRotation;
			                float delta = Time.fixedDeltaTime * 3f;
			                destination.rotation = Quaternion.Lerp(rotation, target, delta);
			            }
			            goto Threshold;
			        case NpcStateModel.State.FoundStranger: goto Found;
			        case NpcStateModel.State.TurnAround: goto Turn;
			        default: goto Finish;
			    }
			}
			if (Nca.StateModel.CurrentFixedType == NpcStateModel.FixedType.Conbini)
			{
			    if ((int)ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType != 7)
			    {
			        npcConbiniState.OnUpdate();
			        switch (stateModel.CurrentState)
			        {
			            case NpcStateModel.State.Idle: Nca.NpcLocomotion.OnUpdate(); goto Threshold;
			            case NpcStateModel.State.FoundStranger: goto Found;
			            case NpcStateModel.State.TurnAround: goto Turn;
			            case NpcStateModel.State.Interact:
			                Nca.NpcInteractState.OnUpdate();
			                stateModel.IsJustOnEndInteract = false;
			                goto Threshold;
			            default: goto Finish;
			        }
			    }
			    switch (stateModel.CurrentState)
			    {
			        case NpcStateModel.State.Idle: fashionClerkSubState.OnUpdate(); goto Threshold;
			        case NpcStateModel.State.FoundStranger: goto Found;
			        case NpcStateModel.State.TurnAround: goto Turn;
			        case NpcStateModel.State.Interact: goto Threshold;
			        default: goto Finish;
			    }
			}
			if (Nca.StateModel.CurrentFixedType != NpcStateModel.FixedType.None) goto Finish;
			switch (stateModel.CurrentState)
			{
			    case NpcStateModel.State.Idle:
			        Nca.NpcLocomotion.OnUpdate();
			        Nca.NpcForeignObjectLooker.OnUpdate();
			        goto Threshold;
			    case NpcStateModel.State.FoundStranger: goto Found;
			    case NpcStateModel.State.TurnAround: goto Turn;
			    case NpcStateModel.State.FadeOut: npcFadeOutState.OnUpdate(); goto Finish;
			    case NpcStateModel.State.Stagger: Nca.NpcStaggerState.OnUpdate(); goto Finish;
			    case NpcStateModel.State.Interact:
			        Nca.NpcInteractState.OnUpdate();
			        Nca.NpcForeignObjectLooker.OnUpdate();
			        stateModel.IsJustOnEndInteract = false;
			        goto Threshold;
			    case NpcStateModel.State.WaitToken: Nca.NpcWaitTokenState.OnUpdate(); goto Finish;
			    case NpcStateModel.State.TraceSexPlayer: Nca.NpcTraceSexPlayerState.OnUpdate(); goto Finish;
			    default: goto Finish;
			}
			Found:
			npcFoundStrangerState.OnUpdate();
			goto Finish;
			Turn:
			Nca.NpcTurnAroundState.OnUpdate();
			Threshold:
			ChangeFoundStrangerIfInCondition();
			Finish:
			Nca.NpcAnimationManager.OnUpdate();
			Nca.NpcCollider.OnUpdate();
			Nca.StateModel.IsJustOnEndChairSitDown = false;
		}

		public void OnAnimatorMove()
		{
		}

		private void ChangeFoundStrangerIfInCondition()
		{
			if (Nca.StrangenessManager.Strangeness.Value >= 1f && !stateModel.IsAlreadyFoundStranger)
			{
			    ChangeState(NpcStateModel.State.FoundStranger);
			    stateModel.IsAlreadyFoundStranger = true;
			}
		}

		public void OnLateUpdate()
		{
			if (15f < stateModel.PlayerDist) return;
			Nca.LookAtController.OnLateUpdate();
			Nca.PlayerSearcher.OnLateUpdate();
		}

		public void ChangeState(NpcStateModel.State nextState)
		{
			switch (stateModel.CurrentState)
			{
			    case NpcStateModel.State.Idle:
			        Nca.LookAtController.DisableLookAt(LookAtController.LookAtLayer.NpcForeignObject);
			        Nca.NpcComponent.Smartphone.gameObject.SetActive(false);
			        break;
			    case NpcStateModel.State.FoundStranger:
			        npcFoundStrangerState.OnExit();
			        break;
			    case NpcStateModel.State.TurnAround:
			        Nca.NpcTurnAroundState.OnExit();
			        break;
			    case NpcStateModel.State.Interact:
			        Nca.NpcInteractState.OnExit();
			        break;
			    case NpcStateModel.State.TraceSexPlayer:
			        Nca.NpcTraceSexPlayerState.OnExit();
			        break;
			}
			stateModel.BeforeState = stateModel.CurrentState;
			stateModel.CurrentState = nextState;
			NpcStateModel enteringState = stateModel;
			float enteringTime = Time.time;
			if (enteringState is null) throw new System.NullReferenceException();
			enteringState.stateInTime = enteringTime;
			switch (nextState)
			{
			    case NpcStateModel.State.Idle:
			        Nca.NpcLocomotion.OnEnter();
			        Nca.NpcAnimationManager.SetSpeed(1f);
			        Nca.NpcComponent.Smartphone.gameObject.SetActive(Nca.StateModel.isSumahoWalk);
			        break;
			    case NpcStateModel.State.FoundStranger:
			        Nca.NpcAnimationManager.SetSpeed(0f);
			        npcFoundStrangerState.OnEnter();
			        break;
			    case NpcStateModel.State.TurnAround:
			        Nca.NpcTurnAroundState.OnEnter();
			        break;
			    case NpcStateModel.State.FadeOut:
			        npcFadeOutState.OnEnter();
			        break;
			    case NpcStateModel.State.Stagger:
			        Nca.NpcAnimationManager.SetSpeed(0f);
			        Nca.NpcStaggerState.OnEnter();
			        break;
			    case NpcStateModel.State.Interact:
			        Nca.NpcAnimationManager.SetSpeed(0f);
			        Nca.NpcLocomotion.SetActiveNavAgent(false);
			        Nca.NpcInteractState.OnEnter();
			        Nca.StateModel.IsComeBackInteractFromFoundStranger = false;
			        break;
			    case NpcStateModel.State.WaitToken:
			        Nca.NpcAnimationManager.SetSpeed(0f);
			        Nca.NpcWaitTokenState.OnEnter();
			        break;
			    case NpcStateModel.State.TraceSexPlayer:
			        Nca.NpcLocomotion.OnEnter();
			        Nca.NpcAnimationManager.SetSpeed(1f);
			        Nca.NpcTraceSexPlayerState.OnEnter();
			        break;
			}
		}

		public void FirePinponAction(bool isOpen, float animSpeed, bool isMirror)
		{
		}

		public void OnGameOver()
		{
// Complete original RVA 0x429F30; PE/typed identity is audited in this owned stage.
Nca.Animator.speed = 0f;
}

		public void Warp(Vector3 position, Quaternion rotation)
		{
			Nca.Transform.position = position;
			Nca.Transform.rotation = rotation;
			Nca.NavAgent.Warp(position);
		}

		public void Destroy()
		{
            Nca.Dispose();
            EventManager.RemoveListener<TimeStopEvent>(OnTimeStopEvent);
            if (NpcComponent) UnityEngine.Object.Destroy(NpcComponent.gameObject);
        }

		public void Push(Vector3 pushV, bool isFixed = false)
		{
		}

		public void SetSoundCancelScale(float volume, bool isInstantly)
		{
		}

		public void HitSpermOnSkin()
		{
		}

		private void OnTimeStopEvent(TimeStopEvent evt)
		{
		}
	}
}
