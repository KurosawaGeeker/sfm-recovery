using ExposureUnnoticed2.Object3D.Camera;
using ExposureUnnoticed2.Object3D.Player.Scripts.Other;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerLocomotion
	{
		private int skipFrameCameraCount;

		private int skipGravityCount;

		private float gravity;

		private float jumpPower;

		private float verticalPower;

		private SmoothFloat moveSpeed;

		private CharacterController characterController;

		private TPCameraController tpCameraController;

		private FPCameraController fpCameraController;

		private BaseCameraController baseCameraController;

		private PlayerAnimationManager animationManager;

		private MentalManager mental;

		private GameObject root;

		private PlayerStateModel playerStateModel;

		private bool addMessageLock;

		private bool dashButtonLock;

		private PlayerClassAccessor pca;

		private bool oldIsCrouchStrafeBack;

		private PlayerCollisionCorrecter pcc;

		public float CurrentMoveSpeed => moveSpeed.Current;

		public void Initialize(TPCameraController tpCameraController, FPCameraController fpCameraController, BaseCameraController baseCameraController, PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x650B70
			this.pca = pca;
			pcc.Initialize(pca);
			characterController = pca.CharacterController;
			root = pca.GameObject;
			this.tpCameraController = tpCameraController;
			this.fpCameraController = fpCameraController;
			this.baseCameraController = baseCameraController;
			animationManager = pca.PlayerAnimationManager;
			mental = pca.Mental;
			playerStateModel = pca.PlayerState;
			baseCameraController.OnChangeFirstPersonView += OnChangeFirstPersonView;
			pca.NavAgent.updatePosition = false;
			pca.NavAgent.updateRotation = false;
			moveSpeed = new SmoothFloat(0.1f, 1f, 0f, false, false, -1f);
			EventManager.AddListener<CompleteTransitionStageEvent>(OnCompleteTransitionStageEvent, false);
			EventManager.AddListener<ChangeTimeEvent>(OnChangeTimeEvent, false);
		}

		public void Dispose()
		{
			// Recovered from GameAssembly.dll RVA 0x6513D0
			EventManager.RemoveListener<CompleteTransitionStageEvent>(OnCompleteTransitionStageEvent);
			EventManager.RemoveListener<ChangeTimeEvent>(OnChangeTimeEvent);
		}

		public void OnWarp()
		{
			// Recovered from GameAssembly.dll RVA 0x6514F0
			verticalPower = 0f;
			moveSpeed.Current = 0f;
			pca.PlayerState.MoveV = Vector3.zero;
		}

		private Vector3 CalcAutoChaseDirect(Vector3 goal)
		{
			// Recovered from GameAssembly.dll RVA 0x651570
			// Original651570..6518D5 includes five recursively chained unwind regions.
			pca.NavAgent.SetDestination(goal);
			Vector3 target = pca.NavAgent.steeringTarget;
			Vector3 position = pca.Transform.position;
			float differenceX = target.x - position.x;
			float differenceZ = target.z - position.z;
			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(ExternalMethodManager).TypeHandle);
			var capturedAccessor = pca;
			if (capturedAccessor is null) throw new System.NullReferenceException();
			Transform capturedTransform = capturedAccessor.Transform;
			var capturedAgent = capturedAccessor.NavAgent;
			if (capturedTransform is null) throw new System.NullReferenceException();
			Vector3 nextPosition = capturedTransform.position;
			if (capturedAgent is null) throw new System.NullReferenceException();
			capturedAgent.nextPosition = nextPosition;
			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(System.Math).TypeHandle);
			float magnitude = (float)System.Math.Sqrt((double)((0f * 0f + differenceX * differenceX) + differenceZ * differenceZ));
			if (magnitude > 0.00001f)
			    return new Vector3(differenceX / magnitude, 0f / magnitude, differenceZ / magnitude);
			return Vector3.zero;
		}

		private void SetMoveSpeedInverse(bool isInverse)
		{
			// Recovered from GameAssembly.dll RVA 0x6518E0
			if (oldIsCrouchStrafeBack == isInverse) return;
			var capturedAccessor = pca;
			oldIsCrouchStrafeBack = isInverse;
			if (capturedAccessor is null) throw new System.NullReferenceException();
			var capturedCustomize = capturedAccessor.PlayerBodyCustomizeAdjuster;
			var capturedAnimation = capturedAccessor.PlayerAnimationManager;
			if (capturedCustomize is null || capturedAnimation is null) throw new System.NullReferenceException();
			capturedAnimation.SetMoveSpeed((isInverse ? -1f : 1f) * capturedCustomize.MoveSpeed);
		}

		public void OnUpdate(InputData inputData)
		{
			// Recovered from GameAssembly.dll RVA 0x651940
			// Original complete native family 651940..653AFC, not only the first unwind region.
			skipGravityCount = unchecked(skipGravityCount - 1);
			StateUpdate(inputData);
			
			if (!pca.PlayerState.IsAutoChase)
			{
			    if (pca.PlayerState.IsAutoRun)
			    {
			        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(InputManager).TypeHandle);
			        if (InputManager.IsDown(InputManager.InputType.RightClick) || pca.PlayerState.IsStrafe)
			        {
			            Vector3 forward = Vector3.forward;
			            if (inputData is null) throw new System.NullReferenceException();
			            inputData.Move = forward;
			        }
			        else
			        {
			            Vector3 rootPosition = root.transform.position;
			            Vector3 cameraPosition = tpCameraController.transform.position;
			            Vector3 relative = new Vector3(rootPosition.x - cameraPosition.x, 0f, rootPosition.z - cameraPosition.z);
			            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(ExternalMethodManager).TypeHandle);
			            Quaternion inverse = Quaternion.Inverse(Quaternion.LookRotation(relative));
			            Vector3 playerForward = pca.Transform.forward;
			            Vector3 direct = inverse * playerForward;
			            if (inputData is null) throw new System.NullReferenceException();
			            inputData.Move = direct;
			        }
			    }
			    else if (inputData.MoveForwardOneHand)
			    {
			        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(InputManager).TypeHandle);
			        if (InputManager.IsDown(InputManager.InputType.RightClick) || pca.PlayerState.IsStrafe)
			            inputData.Move = Vector3.forward;
			        else
			        {
			            Vector3 rootPosition = root.transform.position;
			            Vector3 cameraPosition = tpCameraController.transform.position;
			            Vector3 relative = new Vector3(rootPosition.x - cameraPosition.x, 0f, rootPosition.z - cameraPosition.z);
			            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(ExternalMethodManager).TypeHandle);
			            Quaternion inverse = Quaternion.Inverse(Quaternion.LookRotation(relative));
			            inputData.Move = inverse * pca.Transform.forward;
			        }
			    }
			}
			else
			{
			    if (playerStateModel.ChaseTargetNpc is null)
			    {
			        if (pca.PlayerState.NearNpc is not null && pca.PlayerState.NearNpcDist <= 5f)
			            playerStateModel.ChaseTargetNpc = pca.PlayerState.NearNpc;
			    }
			    else
			    {
			        Transform chasedTransform = playerStateModel.ChaseTargetNpc.Transform;
			        Transform playerTransform = pca.Transform;
			        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(ExternalMethodManager).TypeHandle);
			        if (ExternalMethodManager.DistSqrt(chasedTransform, playerTransform) > 5f)
			            playerStateModel.ChaseTargetNpc = null;
			    }
			    if (playerStateModel.ChaseTargetNpc is not null)
			    {
			        Quaternion look;
			        if (!playerStateModel.IsStrafe)
			        {
			            Vector3 rootPosition = root.transform.position;
			            Vector3 cameraPosition = tpCameraController.transform.position;
			            Vector3 relative = new Vector3(rootPosition.x - cameraPosition.x, 0f, rootPosition.z - cameraPosition.z);
			            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(ExternalMethodManager).TypeHandle);
			            look = Quaternion.LookRotation(relative);
			        }
			        else
			            look = Quaternion.LookRotation(pca.Transform.forward);
			        Quaternion inverse = Quaternion.Inverse(look);
			        Vector3 goal = pca.PlayerState.ChaseTargetNpc.Transform.position;
			        Vector3 direct = inverse * CalcAutoChaseDirect(goal);
			        if (inputData is null) throw new System.NullReferenceException();
			        inputData.Move = direct;
			    }
			}
			
			// The input null check precedes the state receiver check in the native common suffix.
			var inputStateAccessor = pca;
			if (inputStateAccessor is null || inputData is null) throw new System.NullReferenceException();
			var inputState = inputStateAccessor.PlayerState;
			if (inputState is null) throw new System.NullReferenceException();
			inputState.IsDownMoveForwardOneHand = inputData.MoveForwardOneHand;
			if (!playerStateModel.IsStrafe)
			{
			    if (oldIsCrouchStrafeBack)
			    {
			        var capturedAccessor = pca;
			        oldIsCrouchStrafeBack = false;
			        if (capturedAccessor is null) throw new System.NullReferenceException();
			        var capturedAnimation = capturedAccessor.PlayerAnimationManager;
			        var capturedCustomize = capturedAccessor.PlayerBodyCustomizeAdjuster;
			        if (capturedCustomize is null || capturedAnimation is null) throw new System.NullReferenceException();
			        capturedAnimation.SetMoveSpeed(capturedCustomize.MoveSpeed);
			    }
			}
			else
			{
			    if (!playerStateModel.IsCrouch)
			    {
			        if (oldIsCrouchStrafeBack)
			        {
			            var capturedAccessor = pca;
			            oldIsCrouchStrafeBack = false;
			            if (capturedAccessor is null) throw new System.NullReferenceException();
			            var capturedAnimation = capturedAccessor.PlayerAnimationManager;
			            var capturedCustomize = capturedAccessor.PlayerBodyCustomizeAdjuster;
			            if (capturedCustomize is null || capturedAnimation is null) throw new System.NullReferenceException();
			            capturedAnimation.SetMoveSpeed(capturedCustomize.MoveSpeed);
			        }
			    }
			    else
			    {
			        inputData.Move.x = 0f;
			        SetMoveSpeedInverse(inputData.Move.z < 0f);
			    }
			    if (playerStateModel.CurrentAction.IsMoveOnlyForwardWithStrafe)
			    {
			        inputData.Move.x = 0f;
			        float forward = inputData.Move.z;
			        if (forward < 0f) forward = 0f;
			        inputData.Move.z = forward;
			    }
			}
			
			bool previousStrafe = playerStateModel.IsStrafe;
			if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsActivePassiveSkill(global::ExposureUnnoticed2.Master.Skill.SkillType.FixFps))
			    playerStateModel.IsStrafe = true;
			else if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsActivePassiveSkill(global::ExposureUnnoticed2.Master.Skill.SkillType.FixTps))
			    playerStateModel.IsStrafe = false;
			else if (inputData.JustViewChange)
			    playerStateModel.IsStrafe = !playerStateModel.IsStrafe;
			if (previousStrafe != playerStateModel.IsStrafe)
			    global::ExposureUnnoticed2.Scripts.InGame.VisibilityFogController.Instance.ForceUpdateOnce();
			
			bool dashSpeed = false;
			float baseSpeed;
			if (playerStateModel.IsDash && !pca.PlayerState.IsGamanBaibu)
			{
			    dashSpeed = true;
			    float dashScale = (float)pca.PlayerState.ReinforceEffectData.DashSpeed100 / 100f + 1f;
			    baseSpeed = dashScale * global::ExposureUnnoticed2.Master.Common.MCommon.PlayerJogSpeed;
			    System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.InGame.DebugController).TypeHandle);
			    float debugConsumption = global::ExposureUnnoticed2.Scripts.InGame.DebugController.IsHighSpeedMove ? 0f : 1f;
			    int exposedParts = (int)PlayerFacade.Instance.GetExposedBodyPartsByClothesAndCostume();
			    System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(ExternalMethodManager).TypeHandle);
			    float exposureConsumption = exposedParts == 1 ? 0f : 1f;
			    var capturedStamina = pca.Stamina;
			    var capturedCommon = global::ExposureUnnoticed2.Master.Common.MCommon.Instance;
			    if (capturedCommon is null || capturedStamina is null) throw new System.NullReferenceException();
			    capturedStamina.ConsumePerSecond((debugConsumption * capturedCommon.PlayerStaminaDashPerSecond) * exposureConsumption);
			}
			else if (playerStateModel.IsCrouch)
			{
			    // Original getter inlining captures MCommon before reading GameStateData.
			    float commonSpeed = global::ExposureUnnoticed2.Master.Common.MCommon.PlayerCrouchSpeed;
			    var gameState = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
			    baseSpeed = ((float)gameState.WalkSpeedScaleIndex * 0.05f + 1f) * commonSpeed;
			}
			else if (pca.PlayerState.IsGamanBaibu)
			    baseSpeed = global::ExposureUnnoticed2.Master.Common.MCommon.Instance.PlayerGamanWalkSpeed;
			else
			{
			    float commonSpeed = global::ExposureUnnoticed2.Master.Common.MCommon.PlayerWalkSpeed;
			    var gameState = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
			    baseSpeed = ((float)gameState.WalkSpeedScaleIndex * 0.05f + 1f) * commonSpeed;
			}
			
			Vector3 magnitudeInput = inputData.Move;
			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(System.Math).TypeHandle);
			float firstMagnitude = (float)System.Math.Sqrt((double)((magnitudeInput.y * magnitudeInput.y + magnitudeInput.x * magnitudeInput.x) + magnitudeInput.z * magnitudeInput.z));
			float targetSpeed = firstMagnitude * baseSpeed;
			if (playerStateModel.CurrentAction.CanMove && playerStateModel.CurrentAction.FloatParam != 0f)
			    targetSpeed = playerStateModel.CurrentAction.FloatParam;
			magnitudeInput = inputData.Move;
			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(System.Math).TypeHandle);
			float secondMagnitude = (float)System.Math.Sqrt((double)((magnitudeInput.y * magnitudeInput.y + magnitudeInput.x * magnitudeInput.x) + magnitudeInput.z * magnitudeInput.z));
			float targetAnimationSpeed = (dashSpeed ? 2f : 1f) * secondMagnitude;
			bool stopMotion = !playerStateModel.CurrentAction.CanMove || playerStateModel.IsEcstasyMotion;
			if (!stopMotion)
			{
			    float invalidTime = pca.PlayerState.InvalidLocomotionTime;
			    stopMotion = Time.time < invalidTime;
			}
			if (stopMotion)
			{
			    targetSpeed = 0f;
			    targetAnimationSpeed = 0f;
			}
			if (!pca.PlayerObjectReferencer.OnGroundedChecker.IsGrounded)
			{
			    if (skipGravityCount < 0) verticalPower -= gravity;
			}
			else
			{
			    float groundedPower = inputData.Jump ? jumpPower : 0f;
			    verticalPower = groundedPower;
			    if (!pca.PlayerObjectReferencer.DownhillChecker.IsGrounded && skipGravityCount < 0)
			        verticalPower = groundedPower - (gravity + gravity);
			}
			
			Vector3 heading;
			if (!playerStateModel.IsStrafe)
			{
			    Vector3 rootPosition = root.transform.position;
			    Vector3 cameraPosition = tpCameraController.transform.position;
			    heading = new Vector3(rootPosition.x - cameraPosition.x, 0f, rootPosition.z - cameraPosition.z);
			    System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(ExternalMethodManager).TypeHandle);
			    if (targetSpeed > 0f)
			    {
			        Vector3 worldInput = Quaternion.LookRotation(heading, Vector3.up) * inputData.Move;
			        float rate = pca.PlayerState.IsGamanBaibu ? 5f : 15f;
			        Vector3 oldForward = root.transform.forward;
			        float deltaTime = Time.deltaTime;
			        Vector3 rotated = Vector3.RotateTowards(oldForward, worldInput, deltaTime * rate, 0f);
			        if (playerStateModel.CurrentAction.CanMove)
			        {
			            Transform capturedRootTransform = root.transform;
			            Quaternion look = Quaternion.LookRotation(rotated);
			            if (capturedRootTransform is null) throw new System.NullReferenceException();
			            capturedRootTransform.rotation = look;
			        }
			    }
			    if (animationManager is null) throw new System.NullReferenceException();
			    animationManager.SetStrafe(false, false, inputData.Move.x, inputData.Move.z);
			}
			else
			{
			    float yaw = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetCameraMoveX();
			    bool cameraBlocked = CursorController.Instance.IsVisible;
			    if (!cameraBlocked)
			    {
			        cameraBlocked = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.BaseCameraController.IsLockCamera;
			        if (!cameraBlocked)
			        {
			            var shop = global::ExposureUnnoticed2.ObjectUI.PcMenuPanel.OnlineShopPanel.OnlineShopPanelView.Instance;
			            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(UnityEngine.Object).TypeHandle);
			            cameraBlocked = shop != null;
			        }
			    }
			    if (cameraBlocked)
			    {
			        yaw = 0f;
			        skipFrameCameraCount = 4;
			    }
			    if (skipFrameCameraCount > 0 && inputData.CameraMove.sqrMagnitude > 0f)
			    {
			        skipFrameCameraCount = unchecked(skipFrameCameraCount - 1);
			        yaw = 0f;
			    }
			    if (PlayerController.Instance.Pca.PlayerState.IsEcstasyMotion) yaw = 0f;
			    var chaseAccessor = pca;
			    if (chaseAccessor is null) throw new System.NullReferenceException();
			    if (chaseAccessor.PlayerState.IsAutoChase && chaseAccessor.PlayerState.NearNpc is not null && chaseAccessor.PlayerState.NearNpcDist <= 5f)
			        yaw = 0f;
			
			    Quaternion oldRotation = root.transform.rotation;
			    Quaternion yawRotation = Quaternion.Euler(0f, yaw, 0f);
			    // Raw652E36..652EDD: oldRotation * yawRotation; multiplication operand/order retained.
			    Quaternion rotation = new Quaternion(
			        ((yawRotation.x * oldRotation.w + yawRotation.w * oldRotation.x) + yawRotation.z * oldRotation.y) - yawRotation.y * oldRotation.z,
			        ((yawRotation.y * oldRotation.w + yawRotation.w * oldRotation.y) + yawRotation.x * oldRotation.z) - yawRotation.z * oldRotation.x,
			        ((yawRotation.z * oldRotation.w + yawRotation.w * oldRotation.z) + yawRotation.y * oldRotation.x) - yawRotation.x * oldRotation.y,
			        ((yawRotation.w * oldRotation.w - yawRotation.x * oldRotation.x) - yawRotation.y * oldRotation.y) - yawRotation.z * oldRotation.z);
			    Vector3 forward = Vector3.forward;
			    float xx = rotation.x + rotation.x, yy = rotation.y + rotation.y, zz = rotation.z + rotation.z;
			    heading = new Vector3(
			        ((1f - (zz * rotation.z + yy * rotation.y)) * forward.x + (yy * rotation.x - zz * rotation.w) * forward.y) + (yy * rotation.w + zz * rotation.x) * forward.z,
			        ((1f - (zz * rotation.z + xx * rotation.x)) * forward.y + (zz * rotation.w + yy * rotation.x) * forward.x) + (zz * rotation.y - xx * rotation.w) * forward.z,
			        ((zz * rotation.x - yy * rotation.w) * forward.x + (xx * rotation.w + zz * rotation.y) * forward.y) + (1f - (yy * rotation.y + xx * rotation.x)) * forward.z);
			    if (playerStateModel.CurrentAction.CanMove)
			    {
			        Transform capturedRootTransform = root.transform;
			        Quaternion look = Quaternion.LookRotation(heading, Vector3.up);
			        if (capturedRootTransform is null) throw new System.NullReferenceException();
			        capturedRootTransform.rotation = look;
			    }
			    chaseAccessor = pca;
			    if (chaseAccessor is null) throw new System.NullReferenceException();
			    if (chaseAccessor.PlayerState.IsAutoChase && chaseAccessor.PlayerState.NearNpc is not null && chaseAccessor.PlayerState.NearNpcDist <= 5f)
			    {
			        Vector3 nearPosition = chaseAccessor.PlayerState.NearNpc.Transform.position;
			        Vector3 playerPosition = pca.Transform.position;
			        Vector3 relative = new Vector3(nearPosition.x - playerPosition.x, 0f, nearPosition.z - playerPosition.z);
			        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(ExternalMethodManager).TypeHandle);
			        Transform capturedRootTransform = root.transform;
			        Quaternion capturedRotation = root.transform.rotation;
			        Quaternion targetRotation = Quaternion.LookRotation(relative);
			        Quaternion result = Quaternion.Lerp(capturedRotation, targetRotation, 0.5f);
			        if (capturedRootTransform is null) throw new System.NullReferenceException();
			        capturedRootTransform.rotation = result;
			    }
			    heading = root.transform.forward;
			    if (animationManager is null) throw new System.NullReferenceException();
			    animationManager.SetStrafe(true, inputData.Dash, inputData.Move.x, inputData.Move.z);
			}
			
			Quaternion basis = Quaternion.LookRotation(heading, Vector3.up);
			Vector3 inputMove = inputData.Move;
			float twiceX = basis.x + basis.x, twiceY = basis.y + basis.y, twiceZ = basis.z + basis.z;
			// Raw65347D..653592 inlines Quaternion-vector multiplication, no extra helper definition.
			Vector3 moveDirection = new Vector3(
			    ((1f - (basis.z * twiceZ + basis.y * twiceY)) * inputMove.x + (basis.x * twiceY - basis.w * twiceZ) * inputMove.y) + (basis.w * twiceY + basis.x * twiceZ) * inputMove.z,
			    ((1f - (basis.z * twiceZ + basis.x * twiceX)) * inputMove.y + (basis.w * twiceZ + basis.x * twiceY) * inputMove.x) + (basis.y * twiceZ - basis.w * twiceX) * inputMove.z,
			    ((basis.x * twiceZ - basis.w * twiceY) * inputMove.x + (basis.w * twiceX + basis.y * twiceZ) * inputMove.y) + (1f - (basis.y * twiceY + basis.x * twiceX)) * inputMove.z);
			Vector3 actualForward = root.transform.forward;
			float alignment = (actualForward.y * moveDirection.y + actualForward.x * moveDirection.x) + actualForward.z * moveDirection.z;
			if (!(alignment > 0f)) alignment = 0f;
			if (playerStateModel.IsStrafe) alignment = 1f;
			moveSpeed.UpdateTo(targetSpeed);
			animationManager.TargetMoveSpeed = targetAnimationSpeed;
			float debugScale = 1f;
			if (playerStateModel.IsDash)
			{
			    System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.InGame.DebugController).TypeHandle);
			    if (global::ExposureUnnoticed2.Scripts.InGame.DebugController.IsHighSpeedMove) debugScale = 5f;
			}
			magnitudeInput = inputData.Move;
			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(System.Math).TypeHandle);
			float finalMagnitude = (float)System.Math.Sqrt((double)((magnitudeInput.y * magnitudeInput.y + magnitudeInput.x * magnitudeInput.x) + magnitudeInput.z * magnitudeInput.z));
			Vector3 scaledDirection = new Vector3((moveDirection.x * alignment) * finalMagnitude, (moveDirection.y * alignment) * finalMagnitude, (moveDirection.z * alignment) * finalMagnitude);
			float currentSpeed = moveSpeed.Current;
			float horizontalX = (currentSpeed * scaledDirection.x) * debugScale;
			float horizontalY = (currentSpeed * scaledDirection.y) * debugScale;
			float horizontalZ = (currentSpeed * scaledDirection.z) * debugScale;
			var capturedAction = playerStateModel.CurrentAction;
			if (capturedAction is null) throw new System.NullReferenceException();
			bool canMove = capturedAction.CanMove;
			var finalAccessor = pca;
			if (finalAccessor is null) throw new System.NullReferenceException();
			var finalState = finalAccessor.PlayerState;
			float capturedVerticalPower = verticalPower;
			Vector3 up = Vector3.up;
			Vector3 velocity;
			if (canMove)
			    velocity = new Vector3(
			        capturedVerticalPower * up.x + horizontalX,
			        capturedVerticalPower * up.y + horizontalY,
			        capturedVerticalPower * up.z + horizontalZ);
			else
			    velocity = new Vector3(capturedVerticalPower * up.x, capturedVerticalPower * up.y, capturedVerticalPower * up.z);
			if (finalState is null) throw new System.NullReferenceException();
			finalState.MoveV = velocity;
		}

		public void OnFixedUpdate()
		{
			// Recovered from GameAssembly.dll RVA 0x653B00
			// Reconstructed from the complete original RVA0x653B00. SDK physics calls remain real production calls.
			float oldYaw = pca.Transform.rotation.eulerAngles.y;
			Vector3 displacement = Vector3.zero;
			if (pca.PlayerState.SmoothTransformTarget != null)
			{
			    displacement += Vector3.Lerp(pca.Transform.position, pca.PlayerState.SmoothTransformTarget.position, 0.1f) - pca.Transform.position;
			    pca.Transform.rotation = Quaternion.Lerp(pca.Transform.rotation, pca.PlayerState.SmoothTransformTarget.rotation, 0.1f);
			}
			if (pca.PlayerState.IsFixedPositionRotation)
			{
			    displacement += Vector3.Lerp(pca.Transform.position, pca.PlayerState.FixedPosition, 0.1f) - pca.Transform.position;
			    pca.Transform.rotation = Quaternion.Lerp(pca.Transform.rotation, pca.PlayerState.FixedRotation, 0.2f);
			}
			displacement += (pca.PlayerState.SumFixedPushVThisFrame + pca.PlayerState.MoveV) * Time.fixedDeltaTime;
			if (displacement.sqrMagnitude > 0f) characterController.Move(displacement);
			pca.PlayerState.SumFixedPushVThisFrame = Vector3.zero;
			if (pca.PlayerState.SmoothTransformTarget != null)
			{
			    float yawDifference = Mathf.Abs(oldYaw - pca.PlayerState.SmoothTransformTarget.rotation.eulerAngles.y);
			    if ((pca.Transform.position - pca.PlayerState.SmoothTransformTarget.position).sqrMagnitude < 0.001f &&
			        (yawDifference < 0.1f || yawDifference > 359.9f))
			        pca.PlayerState.SmoothTransformTarget = null;
			    pca.PlayerState.SmoothRemainTime -= Time.fixedDeltaTime;
			    if (pca.PlayerState.SmoothRemainTime < 0f) pca.PlayerState.SmoothTransformTarget = null;
			}
			if (pca.PlayerState.IsFixedWarpPositionRotation)
			{
			    pca.Transform.position = pca.PlayerState.FixedWarpPosition;
			    pca.Transform.rotation = pca.PlayerState.FixedWarpRotation;
			}
			if (pca.PlayerState.SmoothRotateTarget.HasValue)
			{
			    pca.Transform.rotation = Quaternion.Lerp(pca.Transform.rotation, pca.PlayerState.SmoothRotateTarget.Value, 0.1f);
			    float yawDifference = Mathf.Abs(oldYaw - pca.PlayerState.SmoothRotateTarget.Value.eulerAngles.y);
			    if (yawDifference < 0.1f || yawDifference > 359.9f) pca.PlayerState.SmoothRotateTarget = null;
			}
			// Original x64 only checks the CLR reference at0x20, not Unity fake-null for this NPC field.
			if (pca.PlayerState.NearNpc is not null && pca.PlayerState.NearNpcDist <= 0.5f) pcc.CollisionCorrect();
		}

		private void StateUpdate(InputData inputData)
		{
			// Reconstructed from original RVA654C80..655171. This dispatch does not handle first-person view.
			playerStateModel.IsDash = false;
			bool canDash = !pca.PlayerState.IsEcstasyMotion && (int)pca.PlayerState.CurrentAction.Type == -1;
			bool dashRequested = inputData.Dash && (inputData.Move.sqrMagnitude > 0f || inputData.MoveForwardOneHand);
			if (canDash && dashRequested && !dashButtonLock)
			{
			    // Native captures Stamina before this required singleton call/null check; no MCommon value is read.
			    var capturedStamina = pca.Stamina;
			    var common = global::ExposureUnnoticed2.Master.Common.MCommon.Instance;
			    if (common is null) throw new System.NullReferenceException();
			    if (capturedStamina.Current > 0f)
			    {
			        playerStateModel.IsDash = true;
			        playerStateModel.IsCrouch = false;
			    }
			    else
			    {
			        dashButtonLock = true;
			        if (!addMessageLock) addMessageLock = true;
			    }
			}
			else if (inputData.JustCrouch)
			{
			    // Original native inlines this existing helper with its default false skip flags.
			    if (pca.PlayerState.IsCrouch || PlayerFacade.Instance.CanUseHand(true))
			        playerStateModel.IsCrouch = !playerStateModel.IsCrouch;
			}
			if (!inputData.Dash)
			{
			    addMessageLock = false;
			    dashButtonLock = false;
			}
			if (!pca.PlayerState.IsAutoChase)
			{
			    if (inputData.JustAutoChase)
			    {
			        if (!playerStateModel.CurrentAction.CanMove || playerStateModel.CurrentAction.IsMoveOnlyForwardWithStrafe)
			            global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(
			                global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("log_cant_use_hand_by_other"));
			        else
			        {
			            pca.NavAgent.enabled = true;
			            pca.PlayerState.IsAutoChase = true;
			            pca.PlayerState.IsAutoRun = false;
			        }
			    }
			}
			else if (inputData.JustMove || inputData.Dash || inputData.JustAutoChase || inputData.JustInteract)
			{
			    pca.NavAgent.enabled = false;
			    pca.PlayerState.IsAutoChase = false;
			    playerStateModel.ChaseTargetNpc = null;
			}
			if (!pca.PlayerState.IsAutoRun)
			{
			    if (inputData.JustAutoRun)
			    {
			        pca.PlayerState.IsAutoRun = true;
			        pca.PlayerState.IsAutoChase = false;
			    }
			}
			else if (inputData.JustMove || inputData.Dash || inputData.JustAutoRun || inputData.JustInteract)
			    pca.PlayerState.IsAutoRun = false;
		}

		private void OnChangeFirstPersonView()
		{
		}

		public void OnEndAction()
		{
			// Recovered original GameAssembly RVA 0x655180; four chained unwind regions.
			var capturedState = playerStateModel;
			if (capturedState is null) throw new System.NullReferenceException();
			if (!capturedState.IsStrafe) return;
			var capturedCamera = fpCameraController;
			if (capturedCamera is null) throw new System.NullReferenceException();
			Transform capturedChild = capturedCamera.Child;
			if (capturedChild is null) throw new System.NullReferenceException();
			Vector3 capturedForward = capturedChild.forward;
			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(ExternalMethodManager).TypeHandle);
			Vector3 capturedUp = Vector3.up;
			capturedForward.y = 0f;
			Quaternion capturedRotation = Quaternion.LookRotation(capturedForward, capturedUp);
			GameObject capturedRoot = root;
			if (capturedRoot is null) throw new System.NullReferenceException();
			Transform capturedTransform = capturedRoot.transform;
			if (capturedTransform is null) throw new System.NullReferenceException();
			capturedTransform.rotation = capturedRotation;
		}

		private void OnCompleteTransitionStageEvent(CompleteTransitionStageEvent completeTransitionStageEvent)
		{
			// Recovered from GameAssembly.dll RVA 0x6553B0
			pca.NavAgent.areaMask = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType == (global::ExposureUnnoticed2.Master.Stage.StageType)10 ? 14 : 7;
			skipGravityCount = 10;
		}

		private void OnChangeTimeEvent(ChangeTimeEvent evt)
		{
			// Recovered from GameAssembly.dll RVA 0x655480
			skipGravityCount = 10;
		}
	// Recovered original member RVA 0x655490: .ctor
		public PlayerLocomotion()
        {
            skipGravityCount = 10;
            gravity = 1f;
            pcc = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.PlayerCollisionCorrecter();
        }
	}
}
