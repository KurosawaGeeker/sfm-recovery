using ExposureUnnoticed2.Master.Action;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerAnimationManager
	{
		public enum Layer
		{
			Locomotion = 0,
			Action = 1,
			OverrideArmHand = 2,
			ClothesActionArm = 3,
			OverrideClothArmLayer = 4,
			Upper = 5,
			OverrideHandArm = 6,
			OverrideRightArmHand = 7
		}

		private const int LocomotionLayer = 0;

		private const int ActionLayer = 1;

		private const int ClothesActionArmLayer = 3;

		private const int OverrideClothArmLayer = 4;

		private const int Upper = 5;

		private const int OverrideArmForHandcuff = 8;

		private const int OverrideLeftArm = 9;

		private const int FaceLayer = 10;

		private static readonly int MoveSpeed = Animator.StringToHash("Speed");

		private static readonly int CoatState = Animator.StringToHash("CoatState");

		private static readonly int HeadHeight = Animator.StringToHash("HeadHeight");

		private static readonly int IsStrafe = Animator.StringToHash("IsStrafe");

		private static readonly int StrafeX = Animator.StringToHash("StrafeX");

		private static readonly int StrafeY = Animator.StringToHash("StrafeY");

		private static readonly int IsCrouch = Animator.StringToHash("IsCrouch");

		private static readonly int ActionId = Animator.StringToHash("ActionId");

		private static readonly int Action = Animator.StringToHash("TransAction");

		private float baseMoveSpeed;

		private float pulseTargetArmLayerWeight;

		private bool isGamanBaibu;

		private Animator animator;

		private PlayerStateModel playerStateModel;

		private PlayerClassAccessor pca;

		public SmoothFloat coatStateA;

		private SmoothFloat coatStateB;

		private SmoothFloat headLayerWeight;

		private SmoothFloat faceLayerWeight;

		private SmoothFloat armLayerWeight;

		private SmoothFloat overrideArmForHandcuffLayerWeight;

		private SmoothFloat overrideRightArmHand;

		private SmoothFloat overrideArmLayerWeight;

		private SmoothFloat strafeX;

		private SmoothFloat strafeY;

		private SmoothFloat dokidoki;

		private SmoothFloat moveSpeedBlend;

		private SmoothFloat handcaffsWeightBlend;

		private SmoothFloat anotherMotionIndex;

		private SmoothFloat locomotionMotionSpeed;

		private static readonly int Ecstasy = Animator.StringToHash("Ecstasy");

		private static readonly int IsBareta = Animator.StringToHash("IsBareta");

		private static readonly int Dokidoki = Animator.StringToHash("Dokidoki");

		private static readonly int HadBasket = Animator.StringToHash("HadBasket");

		private static readonly int DownHip = Animator.StringToHash("DownHip");

		private static readonly int OpenHip = Animator.StringToHash("OpenHip");

		private static readonly int CoatStateB = Animator.StringToHash("CoatStateB");

		private static readonly int HoldInHand = Animator.StringToHash("HoldInHand");

		private static readonly int BaretaEscape = Animator.StringToHash("BaretaEscape");

		private static readonly int IsHandcuffs = Animator.StringToHash("IsHandcuffs");

		private static readonly int IsGamanBaibu = Animator.StringToHash("IsGamanBaibu");

		private static readonly int HandcuffsWeight = Animator.StringToHash("HandcuffsWeight");

		private static readonly int DropClothes = Animator.StringToHash("DirectDropClothes");

		private static readonly int NoHip = Animator.StringToHash("DirectNoHip");

		private static readonly int BaretaFace = Animator.StringToHash("BaretaFace");

		private static readonly int LocomotionMotionSpeed = Animator.StringToHash("LocomotionMotionSpeed");

		private static readonly int PlayerEcstasy = Animator.StringToHash("PlayerEcstasy");

		private static readonly int OldActionId = Animator.StringToHash("OldActionId");

		private int oldActionId;

		private static readonly int MotionSpeedScaleWhenSlow = Animator.StringToHash("MotionSpeedScaleWhenSlow");

		private static readonly int BreastSize = Animator.StringToHash("BreastSize");

		private static readonly int AnotherMotionIndex = Animator.StringToHash("AnotherMotionIndex");

		private static readonly int IsEcstasy = Animator.StringToHash("IsEcstasy");

		private static readonly int FutanariSize = Animator.StringToHash("FutanariSize");

		private static readonly int MotionSpeedScale = Animator.StringToHash("MotionSpeedScale");

		private static readonly int PreviewNakedArmPose = Animator.StringToHash("PreviewNakedArmPose");

		private static readonly int WalkSpeedScale = Animator.StringToHash("WalkSpeedScale");

		public float TargetMoveSpeed { get; set; }

		public float TargetClothesStateABlend { get; set; }

		public float TargetCoatStateBBlend { get; set; }

		public float TargetArmLayerWeight { get; set; }

		public bool IsForceHeadLayerWeight { get; set; }

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x62C9D0
			playerStateModel = pca.PlayerState;
			animator = pca.Animator;
			faceLayerWeight.Target = 1f;
			locomotionMotionSpeed.Current = 1f;
			baseMoveSpeed = 1f;
			this.pca = pca;
		}

		public void OnUpdate(InputData inputData)
		{
            // Original62CB20..62D7C5; tail dependency is the existing UpdateHandcuffsWeight.
            var capturedAnimator = animator;
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(PlayerAnimationManager).TypeHandle);
            var crouchState = playerStateModel;
            if (crouchState is null || capturedAnimator is null) throw new System.NullReferenceException();
            capturedAnimator.SetBool(IsCrouch, crouchState.IsCrouch);

            float headTarget = 0f;
            if (IsForceHeadLayerWeight) headTarget = 1f;
            else if (playerStateModel.IsStrafe)
            {
                float moveY = inputData.Move.y;
                float moveX = inputData.Move.x;
                float moveZ = inputData.Move.z;
                float squared = (moveX * moveX + moveY * moveY) + moveZ * moveZ;
                if (0f >= squared) headTarget = 1f;
            }
            headLayerWeight.UpdateTo(headTarget);
            var capturedFace = faceLayerWeight;
            capturedFace.UpdateTo(capturedFace.Target);
            capturedFace = faceLayerWeight;
            if (capturedFace is null) throw new System.NullReferenceException();
            capturedAnimator = animator;
            float faceCurrent = capturedFace.Current;
            if (capturedAnimator is null) throw new System.NullReferenceException();
            capturedAnimator.SetLayerWeight(10, faceCurrent);
            moveSpeedBlend.UpdateTo(TargetMoveSpeed);

            float pulse = pulseTargetArmLayerWeight - 0.1f;
            if (pulse < 0f) pulse = 0f;
            else if (3f < pulse) pulse = 3f;
            pulseTargetArmLayerWeight = pulse;
            float armTarget;
            if (pulse > 0f)
            {
                if (pulse > 2f) armTarget = 2.5f - pulse;
                else armTarget = pulse < 1f ? pulse : 1f;
            }
            else
            {
                armTarget = 0f;
                if (playerStateModel.CurrentAction.Type == ActionType.None
                    || pca.PlayerState.CurrentAction.IsKeepClothesArmLayer
                    || playerStateModel.CurrentAction.Type == ActionType.DroppingClothes)
                {
                    if (!playerStateModel.IsCrouch
                        && (!pca.PlayerState.CurrentAction.IsIgnoreClothesArmLayerByEcstasy
                            || !playerStateModel.IsEcstasyMotion))
                        armTarget = TargetArmLayerWeight;
                }
            }
            armLayerWeight.UpdateTo(armTarget);
            var capturedArm = armLayerWeight;
            if (capturedArm is null) throw new System.NullReferenceException();
            capturedAnimator = animator;
            float armCurrent = capturedArm.Current;
            if (capturedAnimator is null) throw new System.NullReferenceException();
            capturedAnimator.SetLayerWeight(3, armCurrent);

            var capturedCoat = coatStateA;
            if (TimeScaleManager.Instance is null) throw new System.NullReferenceException();
            float timeScale = UnityEngine.Time.timeScale;
            if (capturedCoat is null) throw new System.NullReferenceException();
            capturedCoat.SetSecond(timeScale * 0.2f);
            coatStateA.UpdateTo(TargetClothesStateABlend);
            capturedCoat = coatStateB;
            if (TimeScaleManager.Instance is null) throw new System.NullReferenceException();
            timeScale = UnityEngine.Time.timeScale;
            if (capturedCoat is null) throw new System.NullReferenceException();
            capturedCoat.SetSecond(timeScale * 0.2f);
            coatStateB.UpdateTo(TargetCoatStateBBlend);

            capturedAnimator = animator;
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(PlayerAnimationManager).TypeHandle);
            capturedCoat = coatStateA;
            if (capturedCoat is null || capturedAnimator is null) throw new System.NullReferenceException();
            float current = capturedCoat.Current;
            int parameter = CoatState;
            capturedAnimator.SetFloat(parameter, current);
            capturedCoat = coatStateB;
            if (capturedCoat is null) throw new System.NullReferenceException();
            capturedAnimator = animator;
            if (capturedAnimator is null) throw new System.NullReferenceException();
            current = capturedCoat.Current;
            parameter = CoatStateB;
            capturedAnimator.SetFloat(parameter, current);
            var capturedSpeed = moveSpeedBlend;
            if (capturedSpeed is null) throw new System.NullReferenceException();
            capturedAnimator = animator;
            if (capturedAnimator is null) throw new System.NullReferenceException();
            current = capturedSpeed.Current;
            parameter = MoveSpeed;
            capturedAnimator.SetFloat(parameter, current);

            var bodyCustomization = pca.PlayerBodyCustomizeAdjuster;
            capturedAnimator = animator;
            parameter = BreastSize;
            if (bodyCustomization is null) throw new System.NullReferenceException();
            int size;
            if (bodyCustomization.SmallBreastSize < 100)
                size = unchecked(bodyCustomization.SmallBreastSize - 100);
            else size = bodyCustomization.BigBreastSize;
            if (capturedAnimator is null) throw new System.NullReferenceException();
            current = size;
            capturedAnimator.SetFloat(parameter, current);

            var capturedStrafe = strafeX;
            capturedStrafe.UpdateTo(capturedStrafe.Target);
            capturedStrafe = strafeY;
            capturedStrafe.UpdateTo(capturedStrafe.Target);
            capturedAnimator = animator;
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(PlayerAnimationManager).TypeHandle);
            capturedStrafe = strafeX;
            if (capturedStrafe is null || capturedAnimator is null) throw new System.NullReferenceException();
            current = capturedStrafe.Current;
            parameter = StrafeX;
            capturedAnimator.SetFloat(parameter, current);
            capturedStrafe = strafeY;
            if (capturedStrafe is null) throw new System.NullReferenceException();
            capturedAnimator = animator;
            if (capturedAnimator is null) throw new System.NullReferenceException();
            current = capturedStrafe.Current;
            parameter = StrafeY;
            capturedAnimator.SetFloat(parameter, current);

            var stateForDokidoki = playerStateModel;
            if (stateForDokidoki is null) throw new System.NullReferenceException();
            var capturedDokidoki = dokidoki;
            if (capturedDokidoki is null) throw new System.NullReferenceException();
            capturedDokidoki.UpdateTo(stateForDokidoki.Dokidoki);
            capturedDokidoki = dokidoki;
            if (capturedDokidoki is null) throw new System.NullReferenceException();
            capturedAnimator = animator;
            if (capturedAnimator is null) throw new System.NullReferenceException();
            current = capturedDokidoki.Current;
            parameter = Dokidoki;
            capturedAnimator.SetFloat(parameter, current);
            var gameData = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
            if (gameData is null) throw new System.NullReferenceException();
            capturedAnimator = animator;
            if (capturedAnimator is null) throw new System.NullReferenceException();
            current = gameData.PlayerEcstasy;
            parameter = PlayerEcstasy;
            capturedAnimator.SetFloat(parameter, current);

            capturedAnimator = animator;
            parameter = MotionSpeedScaleWhenSlow;
            if (TimeScaleManager.Instance is null) throw new System.NullReferenceException();
            timeScale = UnityEngine.Time.timeScale;
            if (capturedAnimator is null) throw new System.NullReferenceException();
            current = 1f / timeScale;
            capturedAnimator.SetFloat(parameter, current);
            var capturedAnother = anotherMotionIndex;
            capturedAnother.UpdateTo(capturedAnother.Target);
            capturedAnother = anotherMotionIndex;
            if (capturedAnother is null) throw new System.NullReferenceException();
            capturedAnimator = animator;
            if (capturedAnimator is null) throw new System.NullReferenceException();
            current = capturedAnother.Current;
            parameter = AnotherMotionIndex;
            capturedAnimator.SetFloat(parameter, current);

            float lateMoveY = inputData.Move.y;
            float lateMoveX = inputData.Move.x;
            float lateMoveZ = inputData.Move.z;
            float lateSquared = (lateMoveX * lateMoveX + lateMoveY * lateMoveY) + lateMoveZ * lateMoveZ;
            float movementScale = 1f;
            if (lateSquared > 0f && !pca.PlayerState.IsDash)
            {
                float index = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.WalkSpeedScaleIndex;
                float scaleProduct = index * 0.05f;
                movementScale = 1f / (1f - scaleProduct);
            }
            var capturedLocomotion = locomotionMotionSpeed;
            if (capturedLocomotion is null) throw new System.NullReferenceException();
            capturedLocomotion.UpdateTo(movementScale * baseMoveSpeed);
            capturedAnimator = animator;
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(PlayerAnimationManager).TypeHandle);
            capturedLocomotion = locomotionMotionSpeed;
            if (capturedLocomotion is null || capturedAnimator is null) throw new System.NullReferenceException();
            current = capturedLocomotion.Current;
            parameter = LocomotionMotionSpeed;
            capturedAnimator.SetFloat(parameter, current);

            var currentAction = playerStateModel.CurrentAction;
            if (currentAction is null) throw new System.NullReferenceException();
            var capturedOverride = overrideArmForHandcuffLayerWeight;
            if (capturedOverride is null) throw new System.NullReferenceException();
            float overrideTarget = currentAction.DeActiveHandcuffOverrideArm ? 0f : 1f;
            capturedOverride.UpdateTo(overrideTarget);
            capturedOverride = overrideArmForHandcuffLayerWeight;
            if (capturedOverride is null) throw new System.NullReferenceException();
            capturedAnimator = animator;
            current = capturedOverride.Current;
            if (capturedAnimator is null) throw new System.NullReferenceException();
            capturedAnimator.SetLayerWeight(8, current);
            UpdateHandcuffsWeight();
        }

		public void SetMoveSpeed(float speed)
		{
			// Recovered from GameAssembly.dll RVA 0x426E60
			baseMoveSpeed = speed;
		}

		private void UpdateHandcuffsWeight()
		{
            // Original62D7D0..62D95E, including all four chained unwind regions.
            float target = pca.PlayerState.CurrentAction.HandcuffsWeight;
            if (isGamanBaibu)
            {
                if (pca.PlayerState.VibeBikunMotionRemainTime <= 0f) target = 0.9f;
                if (pca.PlayerState.IsEcstasyMotion) target = 0.7f;
                if ((int)pca.PlayerState.CurrentState == 1) target = 0.5f;
            }
            int action = (int)pca.PlayerState.CurrentAction.Type;
            if (action >= 0x272e && action < 0x2773)
                target = pca.PlayerState.CurrentAction.HandcuffsWeight;
            handcaffsWeightBlend.UpdateTo(target);
            var capturedAnimator = animator;
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(PlayerAnimationManager).TypeHandle);
            var capturedBlend = handcaffsWeightBlend;
            if (capturedBlend is null || capturedAnimator is null) throw new System.NullReferenceException();
            float current = capturedBlend.Current;
            int parameter = HandcuffsWeight;
            capturedAnimator.SetFloat(parameter, current);
        }

		public void DirectDropClothes()
		{
			// Recovered from GameAssembly.dll RVA 0x62D960
			animator.SetTrigger(DropClothes);
			coatStateA.Current = 5f;
			TargetClothesStateABlend = 5f;
			animator.SetFloat(CoatState, coatStateA.Current);
		}

		public void DirectNoHip()
		{
// Complete original 0x62DAC0..0x62DB74 linked-unwind family.
            animator.SetTrigger(NoHip);
        }

		public void ForceSetClothesStateBlend(PlayerStateModel.ClothesState state)
		{
			// Recovered from GameAssembly.dll RVA 0x62DB80
			coatStateA.Current = (float)state;
			TargetClothesStateABlend = (float)state;
			animator.SetFloat(CoatState, coatStateA.Current);
		}

		public void SetHeadHeight(float height)
		{
		}

		public void SetMotionSpeedScale(float scale)
		{
			// Recovered from GameAssembly.dll RVA 0x62DD50
			animator.SetFloat(MotionSpeedScale, scale);
		}

		public void SetFutanariSize(float size)
		{
		}

		public void SetStrafe(bool isStrafe, bool isDash, float strafeX, float strafeY)
		{
			// Draft from complete original 62DEF0..62DFD8.
			Animator capturedAnimator = animator;
			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(PlayerAnimationManager).TypeHandle);
			if (capturedAnimator is null) throw new System.NullReferenceException();
			capturedAnimator.SetBool(IsStrafe, isStrafe);
			SmoothFloat capturedX = this.strafeX;
			if (capturedX is null) throw new System.NullReferenceException();
			float factorX = (float)((isDash ? 1 : 0) + 1);
			capturedX.Target = factorX * strafeX;
			SmoothFloat capturedY = this.strafeY;
			if (capturedY is null) throw new System.NullReferenceException();
			float factorY = (float)((isDash ? 1 : 0) + 1);
			capturedY.Target = factorY * strafeY;
		}

		public void TransAction(ActionType type)
		{
            // Candidate from complete original RVA 0x62DFE0..0x62E18E.
            Animator capturedAnimator = animator;
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(PlayerAnimationManager).TypeHandle);
            if (capturedAnimator is null) throw new System.NullReferenceException();
            capturedAnimator.SetInteger(OldActionId, oldActionId);
            capturedAnimator = animator;
            if (capturedAnimator is null) throw new System.NullReferenceException();
            capturedAnimator.SetInteger(ActionId, (int)type);
            capturedAnimator = animator;
            if (capturedAnimator is null) throw new System.NullReferenceException();
            capturedAnimator.SetTrigger(Action);
            oldActionId = (int)type;
        }

		public void TransBaretaEscape()
		{
			// GameAssembly.dll RVA 0x62E190: capture before static initialization.
			Animator capturedAnimator = animator;
			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(PlayerAnimationManager).TypeHandle);
			if (capturedAnimator is null) throw new System.NullReferenceException();
			capturedAnimator.SetTrigger(BaretaEscape);
		}

		public void TransEcstasy()
		{
		}

		public void SetEcstasy(bool isEcstasy)
		{
// Complete original native body; original gates, state order and failure prefixes retained.
var capturedAnimator = animator;
System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(PlayerAnimationManager).TypeHandle);
if (capturedAnimator is null) throw new System.NullReferenceException();
capturedAnimator.SetBool(IsEcstasy, isEcstasy);
}

		public void SetPreviewNakedArmPose(bool isNaked)
		{
// Complete original native body draft; native/CLR acceptance pending.
var receiver = animator; System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(PlayerAnimationManager).TypeHandle); if (receiver is null) throw new System.NullReferenceException(); receiver.SetBool(PreviewNakedArmPose, isNaked);
}

		public void SetArmPulse()
		{
            pulseTargetArmLayerWeight = 2.5f;
        }

		public void SetBareta(bool isBareta)
		{
// Complete original native body; original gates, state order and failure prefixes retained.
var capturedAnimator = animator;
System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(PlayerAnimationManager).TypeHandle);
if (capturedAnimator is null) throw new System.NullReferenceException();
capturedAnimator.SetBool(IsBareta, isBareta);
}

		public void SetBaretaFace(bool isBareta)
		{
// Complete original native body; original gates, state order and failure prefixes retained.
var capturedAnimator = animator;
System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(PlayerAnimationManager).TypeHandle);
if (capturedAnimator is null) throw new System.NullReferenceException();
capturedAnimator.SetBool(BaretaFace, isBareta);
}

		public void SetHadBasket(bool hadBasket)
		{
            // Candidate from complete original RVA 0x62E560..0x62E5E2.
            Animator capturedAnimator = animator;
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(PlayerAnimationManager).TypeHandle);
            if (capturedAnimator is null) throw new System.NullReferenceException();
            capturedAnimator.SetBool(HadBasket, hadBasket);
        }

		public void SetHoldInHand(bool holdInHand)
		{
            // Original RVA 0x62E5F0: capture receiver before static initialization.
            Animator capturedAnimator = animator;
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(PlayerAnimationManager).TypeHandle);
            if (capturedAnimator is null) throw new System.NullReferenceException();
            capturedAnimator.SetBool(HoldInHand, holdInHand);
        }

		public void SetHandcuffs(bool isHandcuffs)
		{
			// Recovered from GameAssembly.dll RVA 0x62E680
			animator.SetBool(IsHandcuffs, isHandcuffs);
		}

		public void SetGamanBaibu(bool isGamanBaibu)
		{
// Complete original native body; original gates, state order and failure prefixes retained.
if (this.isGamanBaibu == isGamanBaibu) return;
this.isGamanBaibu = isGamanBaibu;
var capturedAnimator = animator;
System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(PlayerAnimationManager).TypeHandle);
if (capturedAnimator is null) throw new System.NullReferenceException();
capturedAnimator.SetBool(IsGamanBaibu, isGamanBaibu);
}

		public void SetHip(bool isOpenAction)
		{
		}

		public void SetFaceLayerWeight(float val)
		{
// Complete original native body; original gates, state order and failure prefixes retained.
faceLayerWeight.Target = val;
}

		public void RotateAnotherMotionIndex(bool isReset = false)
		{
    // Complete original 62E8E0..62E966; count is read before the reset flag.
    var capturedAction = pca.PlayerState.CurrentAction;
    if (object.ReferenceEquals(capturedAction, null)) throw new global::System.NullReferenceException();
    int count = capturedAction.AnotherMotionCount;
    var capturedIndex = anotherMotionIndex;
    if (count == 0 || isReset)
    {
        if (capturedIndex == null) throw new global::System.NullReferenceException();
        capturedIndex.Target = 0f;
        return;
    }
    if (capturedIndex == null) throw new global::System.NullReferenceException();
    float nextValue = capturedIndex.Target + 1f;
    float modulus = (float)unchecked(count + 1);
    // Original unregistered CRT helper3A02A0 is remainder, not Mathf.Repeat.
    // Original fmod numerator-NaN branch preserves its sign/payload; CLR rem
    // canonicalizes this case, so retain the already-quiet ADDSS result first.
    float result = float.IsNaN(nextValue) ? nextValue : nextValue % modulus;
    anotherMotionIndex.Target = result;
}

		public Constant.Direct NearStrafeDirect()
		{
			// GameAssembly.dll RVA 0x62E970. COMISS/JAE tests the four upper bounds
			// in order; unordered/NaN comparisons all fall through to Back.
			float angle = Mathf.Atan2(strafeX.Current, strafeY.Current) * 57.29578f;
			float absoluteAngle = Mathf.Abs(angle);
			bool right = 0f <= angle;
			if (absoluteAngle <= 22.5f) return Constant.Direct.Forward;
			if (absoluteAngle <= 67.5f) return right ? Constant.Direct.ForwardRight : Constant.Direct.ForwardLeft;
			if (absoluteAngle <= 112.5f) return right ? Constant.Direct.Right : Constant.Direct.Left;
			if (absoluteAngle <= 157.5f) return right ? Constant.Direct.BackRight : Constant.Direct.BackLeft;
			return Constant.Direct.Back;
		}
	// Recovered original member RVA 0x62F360: .cctor
		// Recovered original .cctor RVA 0x62F360: ordered field initializers preserve BeforeFieldInit.
	// Recovered original member RVA 0x62EA40: .ctor
		public PlayerAnimationManager()
        {
            TargetCoatStateBBlend = 10f;
            TargetArmLayerWeight = 1f;
            coatStateA = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(0.2f, 1f, 0f, false, false, -1f);
            coatStateB = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(0.2f, 1f, 10f, false, false, -1f);
            headLayerWeight = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(0.2f, 1f, 0f, false, false, -1f);
            faceLayerWeight = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(0.5f, 1f, 0f, false, false, -1f);
            armLayerWeight = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(0.2f, 1f, 0f, false, false, -1f);
            overrideArmForHandcuffLayerWeight = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(0.2f, 1f, 0f, false, false, -1f);
            overrideRightArmHand = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(0.2f, 1f, 0f, false, false, -1f);
            overrideArmLayerWeight = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(0.2f, 1f, 0f, false, false, -1f);
            strafeX = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(10f, 1f, 0f, true, false, -1f);
            strafeY = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(10f, 1f, 0f, true, false, -1f);
            dokidoki = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(5f, 1f, 0f, false, false, -1f);
            moveSpeedBlend = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(0.2f, 1f, 0f, false, false, -1f);
            handcaffsWeightBlend = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(1f, 1f, 0f, false, false, -1f);
            anotherMotionIndex = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(0.35f, 1f, 0f, false, false, -1f);
            locomotionMotionSpeed = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(0.2f, 1f, 0f, false, false, -1f);
        }
	}
}
