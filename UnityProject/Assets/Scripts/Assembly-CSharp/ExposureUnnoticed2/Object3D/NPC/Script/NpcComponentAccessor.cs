using ExposureUnnoticed2.Object3D.NPC.Script.State;
using ExposureUnnoticed2.Object3D.NPC.Script.State.Toilet;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using UnityEngine;
using UnityEngine.AI;

namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcComponentAccessor
	{
		public Transform Transform { get; }

		public GameObject GameObject { get; }

		public NpcLocomotion NpcLocomotion { get; }

		public NavMeshAgent NavAgent { get; }

		public AudioSource AudioSource { get; }

		public NpcController NpcController { get; }

		public NpcStateModel StateModel { get; }

		public LookAtController LookAtController { get; } = new LookAtController();

		public NpcComponent NpcComponent { get; }

		public NpcMiscController NpcMiscController { get; } = new NpcMiscController();

		public NpcStrangenessManager StrangenessManager { get; } = new NpcStrangenessManager();

		public PlayerSearcher PlayerSearcher { get; }

		public PlayerController Player { get; }

		public NpcTurnAroundState NpcTurnAroundState { get; } = new NpcTurnAroundState();

		public CapsuleCollider CapsuleCollider { get; }

		public NpcAnimatorFpsController NpcAnimatorFpsController { get; } = new NpcAnimatorFpsController();

		public Animator Animator { get; }

		public NpcAnimationEventReceiver AnimationEventReceiver { get; }

		public NpcAnimationManager NpcAnimationManager { get; } = new NpcAnimationManager();

		public Rigidbody Rigidbody { get; }

		public NpcCollider NpcCollider { get; }

		public NpcStaggerState NpcStaggerState { get; } = new NpcStaggerState();

		public NpcAvatarComponent AvatarComponent { get; }

		public NpcCollisionCorrecter NpcCollisionCorrecter { get; } = new NpcCollisionCorrecter();

		public NpcInteractState NpcInteractState { get; } = new NpcInteractState();

		public NpcSoundController NpcSoundController { get; } = new NpcSoundController();

		public NpcWaitTokenState NpcWaitTokenState { get; } = new NpcWaitTokenState();

		public NpcAvatarSelector NpcAvatarSelector { get; } = new NpcAvatarSelector();

		public NpcSleepFixState NpcSleepFixState { get; } = new NpcSleepFixState();

		public NpcChairSubState NpcChairSubState { get; } = new NpcChairSubState();

		public NpcToiletSubState NpcToiletSubState { get; } = new NpcToiletSubState();

		public NpcElevatorSubState NpcElevatorSubState { get; } = new NpcElevatorSubState();

		public NpcForeignObjectLooker NpcForeignObjectLooker { get; } = new NpcForeignObjectLooker();

		public NpcFashionRoomSubState NpcFashionRoomSubState { get; } = new NpcFashionRoomSubState();

		public NpcLaundrySubState NpcLaundrySubState { get; } = new NpcLaundrySubState();

		public NpcTraceSexPlayerState NpcTraceSexPlayerState { get; } = new NpcTraceSexPlayerState();

		public NpcComponentAccessor(NpcController npcController, NpcComponent nc, NpcStateModel stateModel, PlayerController player)
		{
			// Original complete .ctor RVA0x424270. All original helpers initialize before parameter bindings/SDK reads.
			NpcController = npcController;
			Player = player;
			NpcComponent = nc;
			Transform = nc.transform;
			GameObject = nc.gameObject;
			StateModel = stateModel;
			NpcLocomotion = new NpcLocomotion();
			GameObject sightArea = NpcManager.Instance.CreateSightArea();
			if (sightArea is null) throw new System.NullReferenceException();
			PlayerSearcher = sightArea.GetComponent<PlayerSearcher>();
			NavAgent = nc.GetComponent<NavMeshAgent>();
			AudioSource = nc.GetComponent<AudioSource>();
			Rigidbody = nc.GetComponent<Rigidbody>();
			CapsuleCollider = nc.GetComponentInChildren<CapsuleCollider>();
			NpcCollider = nc.GetComponentInChildren<NpcCollider>();
			NpcAvatarSelector selector = NpcAvatarSelector;
			if (selector is null) throw new System.NullReferenceException();
			selector.CreateAvatar(this);
			GameObject avatar = nc.AvaterObject;
			if (avatar is null) throw new System.NullReferenceException();
			AvatarComponent = avatar.GetComponent<NpcAvatarComponent>();
			GameObject animatorObject = NpcComponent.AvaterObject;
			if (animatorObject is null) throw new System.NullReferenceException();
			Animator = animatorObject.GetComponent<Animator>();
			GameObject receiverObject = Animator.gameObject;
			if (receiverObject is null) throw new System.NullReferenceException();
			AnimationEventReceiver = receiverObject.GetComponent<NpcAnimationEventReceiver>();
		}

		public void Initialize()
		{
			NpcLocomotion.Initialize(this);
			NpcMiscController.Initialize(this);
			StrangenessManager.Initialize(this);
			PlayerSearcher.Initialize(this);
			NpcTurnAroundState.Initialize(this);
			NpcAnimatorFpsController.Initialize(this);
			NpcAnimationManager.Initialize(this);
			NpcStaggerState.Initialize(this);
			AvatarComponent.Initialize(this);
			NpcCollisionCorrecter.Initialize(this);
			NpcInteractState.Initialize(this);
			NpcSoundController.Initialize(this);
			NpcWaitTokenState.Initialize(this);
			NpcSleepFixState.Initialize(this);
			NpcChairSubState.Initialize(this);
			NpcToiletSubState.Initialize(this);
			NpcElevatorSubState.Initialize(this);
			NpcForeignObjectLooker.Initialize(this);
			NpcFashionRoomSubState.Initialize(this);
			NpcLaundrySubState.Initialize(this);
			NpcTraceSexPlayerState.Initialize(this);
		}

		public void Dispose()
		{
            StrangenessManager.Dispose();
            if (PlayerSearcher) UnityEngine.Object.Destroy(PlayerSearcher.gameObject);
        }
	}
}
