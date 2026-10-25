using ExposureUnnoticed2.Object3D.Player.Scripts.Costume;
using ExposureUnnoticed2.Object3D.Player.Scripts.Other;
using ExposureUnnoticed2.Scripts.InGame;
using UnityEngine;
using UnityEngine.AI;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerClassAccessor
	{
		public Animator Animator { get; }

		public Transform Transform { get; }

		public Transform AvatorTransform { get; }

		public GameObject GameObject { get; }

		public CharacterController CharacterController { get; }

		public Rigidbody Rigidbody { get; }

		public AudioSource AudioSource { get; }

		public PlayerAnimationManager PlayerAnimationManager { get; }

		public PlayerController Player { get; }

		public PlayerLocomotion PlayerLocomotion { get; }

		public PlayerStateModel PlayerState { get; }

		public PlayerAction PlayerAction { get; }

		public PlayerAvatarObjectReferencer PlayerAvatarObjectReferencer { get; }

		public PlayerObjectReferencer PlayerObjectReferencer { get; }

		public PlayerClothes PlayerClothes { get; }

		public CapsuleCollider CapsuleCollider { get; }

		public PlayerHairController PlayerHairController { get; }

		public PlayerHoldInHand PlayerHoldInHand { get; }

		public DroppedItemManager DroppedItemManager { get; }

		public PlayerMoistureManager MoistureManager { get; private set; }

		public PlayerSoundManager PlayerSoundManager { get; }

		public PlayerPeeController PlayerPeeController { get; }

		public PlayerClothesAction PlayerClothesAction { get; }

		public MentalManager Mental { get; }

		public StaminaManager Stamina { get; }

		public NavMeshAgent NavAgent { get; }

		public PlayerBaretaState PlayerBaretaState { get; }

		public PlayerBaretaEscapeState PlayerBaretaEscapeState { get; }

		public PlayerHandcuffsController PlayerHandcuffsController { get; }

		public PlayerReceiveShadowChanger PlayerReceiveShadowChanger { get; }

		public PlayerAdultGoodsController PlayerAdultGoodsController { get; }

		public PlayerToeHeelController PlayerToeHeelController { get; }

		public PlayerAnimationParameter PlayerAnimationParameter { get; }

		public PlayerCostumeController PlayerCostumeController { get; }

		public LookAtController LookAtController { get; }

		public PlayerFaceController PlayerFaceController { get; }

		public PlayerEcstasyController PlayerEcstasyController { get; }

		public PlayerTitController PlayerTitController { get; }

		public PlayerFaceShadowOnlyController PlayerFaceShadowOnlyController { get; }

		public PlayerStrangenessController PlayerStrangenessController { get; }

		public PlayerPantsController PlayerPantsController { get; }

		public PlayerFacade PlayerFacade { get; }

		public PlayerMiscProcess PlayerMiscProcess { get; }

		public PlayerAnimationReceiver PlayerAnimationReceiver { get; }

		public PlayerBodyCustomizeAdjuster PlayerBodyCustomizeAdjuster { get; }

		public PlayerAvatarOffsetManager PlayerAvatarOffsetManager { get; }

		public PlayerFootStepController PlayerFootStepController { get; }

		public PlayerHeartBeat PlayerHeartBeat { get; }

		public PlayerRpBonusCalculator PlayerRpBonusCalculator { get; }

		public PlayerKneeAdjuster PlayerKneeAdjuster { get; }

		public PlayerShrinkWakiController PlayerShrinkWakiController { get; }

		public PlayerBreathController PlayerBreathController { get; }

		public PlayerBodyMaterialController PlayerBodyMaterialController { get; }

		public PlayerCosplayManager PlayerCosplayManager { get; }

		public PlayerShoesController PlayerShoesController { get; }

		public PlayerReinforceEffectManager PlayerReinforceEffectManager { get; }

		public PlayerBoneScaleAdjustManager PlayerBoneScaleAdjustManager { get; }

		public FutanariController FutanariController { get; }

		public PlayerShiofukiController PlayerShiofukiController { get; }

		public BodyPaintController BodyPaintController { get; }

		public PistonMachineController PistonMachineController { get; }

		public PlayerMoveCalculator PlayerMoveCalculator { get; }

		public PlayerClassAccessor(PlayerController player, DroppedItemManager droppedItemManager)
		{
			// Recovered from GameAssembly.dll RVA 0x63BC80
			PlayerLocomotion = new PlayerLocomotion();
			GameObject = player.gameObject;
			Animator = player.gameObject.GetComponentInChildren<Animator>();
			AudioSource = player.gameObject.GetComponent<AudioSource>();
			Player = player;
			Transform = player.transform;
			PlayerAnimationParameter = player.gameObject.GetComponentInChildren<PlayerAnimationParameter>();
			PlayerAvatarObjectReferencer = player.gameObject.GetComponentInChildren<PlayerAvatarObjectReferencer>();
			PlayerObjectReferencer = player.gameObject.GetComponentInChildren<PlayerObjectReferencer>();
			PlayerAnimationReceiver = player.gameObject.GetComponentInChildren<PlayerAnimationReceiver>();
			AvatorTransform = PlayerAvatarObjectReferencer.transform;
			NavAgent = player.gameObject.GetComponent<NavMeshAgent>();
			CharacterController = player.gameObject.GetComponent<CharacterController>();
			Rigidbody = player.gameObject.GetComponent<Rigidbody>();
			CapsuleCollider = player.gameObject.GetComponent<CapsuleCollider>();
			DroppedItemManager = droppedItemManager;
			MoistureManager = new PlayerMoistureManager();
			PlayerSoundManager = new PlayerSoundManager();
			PlayerPeeController = new PlayerPeeController();
			PlayerClothesAction = new PlayerClothesAction();
			Mental = new MentalManager();
			Stamina = new StaminaManager();
			PlayerBaretaState = new PlayerBaretaState();
			PlayerBaretaEscapeState = new PlayerBaretaEscapeState();
			PlayerHandcuffsController = new PlayerHandcuffsController();
			PlayerReceiveShadowChanger = new PlayerReceiveShadowChanger();
			PlayerAdultGoodsController = new PlayerAdultGoodsController();
			PlayerToeHeelController = new PlayerToeHeelController();
			PlayerCostumeController = new PlayerCostumeController();
			LookAtController = new LookAtController();
			PlayerFaceController = new PlayerFaceController();
			PlayerEcstasyController = new PlayerEcstasyController();
			PlayerTitController = new PlayerTitController();
			PlayerFaceShadowOnlyController = new PlayerFaceShadowOnlyController();
			PlayerStrangenessController = new PlayerStrangenessController();
			PlayerPantsController = new PlayerPantsController();
			PlayerFacade = new PlayerFacade();
			PlayerHoldInHand = new PlayerHoldInHand();
			PlayerHairController = new PlayerHairController();
			PlayerMiscProcess = new PlayerMiscProcess();
			PlayerClothes = new PlayerClothes();
			PlayerAnimationManager = new PlayerAnimationManager();
			PlayerAction = new PlayerAction();
			PlayerState = new PlayerStateModel();
			PlayerHeartBeat = new PlayerHeartBeat();
			PlayerBodyCustomizeAdjuster = new PlayerBodyCustomizeAdjuster();
			PlayerAvatarOffsetManager = new PlayerAvatarOffsetManager();
			PlayerFootStepController = new PlayerFootStepController();
			PlayerRpBonusCalculator = new PlayerRpBonusCalculator();
			PlayerKneeAdjuster = new PlayerKneeAdjuster();
			PlayerShrinkWakiController = new PlayerShrinkWakiController();
			PlayerBreathController = new PlayerBreathController();
			PlayerBodyMaterialController = new PlayerBodyMaterialController();
			PlayerCosplayManager = new PlayerCosplayManager();
			PlayerShoesController = new PlayerShoesController();
			PlayerReinforceEffectManager = new PlayerReinforceEffectManager();
			PlayerBoneScaleAdjustManager = new PlayerBoneScaleAdjustManager();
			FutanariController = new FutanariController();
			PlayerShiofukiController = new PlayerShiofukiController();
			BodyPaintController = new BodyPaintController();
			PistonMachineController = new PistonMachineController();
			PlayerMoveCalculator = new PlayerMoveCalculator();
		}

		public void Initialize()
		{
			// Recovered from GameAssembly.dll RVA 0x63E030
			PlayerAnimationManager.Initialize(this);
			MoistureManager.Initialize(this);
			PlayerSoundManager.Initialize(AudioSource, GameObject, this);
			PlayerPeeController.Initialize(this);
			PlayerClothesAction.Initialize(this);
			Mental.Initialize(PlayerState);
			PlayerBaretaState.Initialize(this);
			PlayerBaretaEscapeState.Initialize(this);
			PlayerHandcuffsController.Initialize(this);
			PlayerReceiveShadowChanger.Initialize(this);
			PlayerReinforceEffectManager.Initialize(this);
			PlayerAdultGoodsController.Initialize(this);
			PlayerToeHeelController.Initialize(this);
			LookAtController.InitializeForPlayer(this);
			PlayerFaceController.Initialize(this);
			PlayerEcstasyController.Initialize(this);
			PlayerTitController.Initialize(this);
			PlayerFaceShadowOnlyController.Initialize(this);
			PlayerStrangenessController.Initialize(this);
			PlayerPantsController.Initialize(this);
			PlayerFacade.Initialize(this);
			PlayerHoldInHand.Initialize(this);
			PlayerHairController.Initialize(this);
			PlayerMiscProcess.Initialize(this);
			PlayerClothes.Initialize(this);
			PlayerAnimationReceiver.Initialize(this);
			PlayerAction.Initialize(this);
			PlayerState.Initialize();
			PlayerHeartBeat.Initialize(this);
			PlayerBoneScaleAdjustManager.Initialize(this);
			FutanariController.Initialize(this);
			BodyPaintController.Initialize(this);
			PlayerBodyMaterialController.Initialize(this);
			PlayerBodyCustomizeAdjuster.Initialize(this);
			PlayerAvatarOffsetManager.Initialize(this);
			PlayerFootStepController.Initialize(this);
			PlayerRpBonusCalculator.Initialize(this);
			PlayerKneeAdjuster.Initialize(this);
			PlayerShrinkWakiController.Initialize(this);
			PlayerBreathController.Initialize(this);
			PlayerShoesController.Initialize(this);
			PlayerCosplayManager.Initialize(this);
			PlayerCostumeController.Initialize(this);
			PlayerShiofukiController.Initialize(this);
			PistonMachineController.Initialize(this);
			PlayerMoveCalculator.Initialize(this);
		}

		public void Dispose()
		{
            // Original RVA 0x63F700: optional prefix, then required suffix, in native order.
            Mental?.Dispose();
            Stamina?.Dispose();
            PlayerFacade?.Dispose();
            PlayerClothes?.Dispose();
            PlayerFaceController?.Dispose();
            PlayerBodyCustomizeAdjuster?.Dispose();
            PlayerTitController.OnDispose();
            PlayerLocomotion.Dispose();
            PlayerSoundManager.Dispose();
            // Original Futanari.Dispose adds ChangeBody listener; preserve that proven behavior.
            FutanariController.Dispose();
            PlayerBodyMaterialController.Dispose();
		}
	}
}
