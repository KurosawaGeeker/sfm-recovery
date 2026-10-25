using ExposureUnnoticed2.Master.Action;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerAction
	{
		private PlayerAnimationManager playerAnimationManager;

		private PlayerStateModel playerState;

		private PlayerClassAccessor pca;

		private bool IsAlreadyChangeActionThisFrame;

		public Vector3 SitOffsetV { get; private set; }

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x625280
			this.pca = pca;
			playerAnimationManager = pca.PlayerAnimationManager;
			playerState = pca.PlayerState;
		}

		public void OnFrameEnd()
		{
			// SOURCE DRAFT: original6253B0; clear flag before a possible PCA/state null failure.
			IsAlreadyChangeActionThisFrame = false;
			var capturedState = pca.PlayerState;
			float oldCooldown = capturedState.PoseActionCoolTime;
			float delta = UnityEngine.Time.deltaTime;
			capturedState.PoseActionCoolTime = oldCooldown - delta;
		}

		public void OnUpdate(InputData inputData)
		{
    // SOURCE DRAFT ONLY: original PlayerAction.OnUpdate RVA 0x625440.
    // No native/CIL equivalence claim. Existing target ChangeAction is called directly.
    // Original initial metadata block: 625463..6255B7 (20 slots):
    // 2D5DAE0 Dictionary<ItemType,int>.get_Item; 2D58288 ExternalMethodManager;
    // 2D58398 GameState; 2D5AB80 InteractManager; 2D53AA8 UnityEngine.Object;
    // 2D58230 PlayerFacade; 2D6F698 log_not_have_piston_machine;
    // 2D62C48 log_not_have_handcuff_key; 2D62C40 log_not_have_vibe_remocon;
    // 2D6F688 log_not_active_time_stop; 2D6F678 ui_noEyeMask;
    // 2D6F668 log_cant_by_not_futanari; 2D6F658 log_not_equip_vibe;
    // 2D6F648 ui_NoWater; 2D6F638 log_not_have_dildo;
    // 2D6F628 log_already_wear_clothes; 2D6F618 log_cant_by_expose_front;
    // 2D6F608 log_not_rank_underwear; 2D5E118 log_cant_use_hand_by_other;
    // 2D6F5F8 log_not_equip_keyhandcuff.
    // Inlined metadata slots: 2D522A8 Vector3, 2D3E668 log_cant_use_hand_by_crouch,
    // 2D3E660 log_cant_use_hand_by_handcuff. All 23 identities are in typed-slots.json.

    int next = -1;
    float cooldown = 0f;
    string messageKey = null;

    // PHASE wall-fixed: saved lines113..188; ICall sites6256A4/6256DD/625784/625817.
    int firstType = (int)pca.PlayerState.CurrentAction.Type;
    if (firstType < 10030 || firstType > 10098)
    {
        if ((int)pca.PlayerState.CurrentAction.Type == 10106)
        {
            var wall = pca.PlayerState.LastPickupDildoWall;
            if (wall is null) throw new System.NullReferenceException();
            var wallGameObject = wall.gameObject; // ICall6256A4 Component.get_gameObject
            if (wallGameObject is null) throw new System.NullReferenceException();
            if (wallGameObject.activeInHierarchy) // ICall6256DD
            {
                pca.PlayerState.IsFixedPositionRotation = true;
                var wallPositionAccessor = pca;
                if (wallPositionAccessor is null) throw new System.NullReferenceException();
                var wallPositionTarget = wallPositionAccessor.PlayerState;
                var wallPositionSource = wallPositionAccessor.PlayerState.LastPickupDildoWall;
                if (wallPositionSource is null) throw new System.NullReferenceException();
                var wallPositionTransform = wallPositionSource.transform;
                if (wallPositionTransform is null) throw new System.NullReferenceException();
                var wallPosition = wallPositionTransform.position; // ICall625784
                wallPositionTarget.FixedPosition = wallPosition;

                var wallRotationAccessor = pca;
                if (wallRotationAccessor is null) throw new System.NullReferenceException();
                var wallRotationTarget = wallRotationAccessor.PlayerState;
                var wallRotationSource = wallRotationAccessor.PlayerState.LastPickupDildoWall;
                if (wallRotationSource is null) throw new System.NullReferenceException();
                var wallRotationTransform = wallRotationSource.transform;
                if (wallRotationTransform is null) throw new System.NullReferenceException();
                var wallRotation = wallRotationTransform.rotation; // ICall625817
                wallRotationTarget.FixedRotation = wallRotation;
            }
        }
    }
    else
    {
        // PHASE ranged-fixed: saved lines189..408, entry62583D, join625ED7.
        var area = pca.PlayerState.LastInteractModel.Controller;
        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(UnityEngine.Object).TypeHandle);
        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(UnityEngine.Object).TypeHandle);
        if (area is null)
        {
            next = -1;
            ChangeAction((ActionType)next);
        }
        else
        {
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(UnityEngine.Object).TypeHandle);
            // Original inlined Unity-object native pointer validity test, public boundary.
            if (area == null)
            {
                next = -1;
                ChangeAction((ActionType)next);
            }
            else
            {
                pca.PlayerState.IsFixedPositionRotation = true;
                var areaVector = pca.PlayerState.LastInteractModel.Controller.VectorParam;
                var actionVector = pca.PlayerState.CurrentAction.VectorParam;
                float actionZ = pca.PlayerState.CurrentAction.VectorParam.z;
                float height = pca.PlayerBodyCustomizeAdjuster.HeightScale;
                float extraZ = pca.PlayerState.CurrentAction.FloatParam;
                System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
                float offsetX = areaVector.x + actionVector.x;
                float offsetY = areaVector.y + actionVector.y;

                var areaPositionSource = pca.PlayerState.LastInteractModel.Controller;
                if (areaPositionSource is null) throw new System.NullReferenceException();
                var areaPositionTransform = areaPositionSource.transform; // ICall625A76
                if (areaPositionTransform is null) throw new System.NullReferenceException();
                var areaPosition = areaPositionTransform.position; // ICall625ABF
                var areaForwardSource = pca.PlayerState.LastInteractModel.Controller;
                if (areaForwardSource is null) throw new System.NullReferenceException();
                var areaForwardTransform = areaForwardSource.transform; // ICall625B23
                if (areaForwardTransform is null) throw new System.NullReferenceException();
                var areaForward = areaForwardTransform.forward; // direct1F4C360
                var playerUpTransform = pca.Transform;
                if (playerUpTransform is null) throw new System.NullReferenceException();
                var playerUp = playerUpTransform.up; // direct1F4C0E0
                var offsetRotation = UnityEngine.Quaternion.LookRotation(areaForward, playerUp); // ICall625BC6
                float offsetZ = (height * actionZ + extraZ) + areaVector.z;
                var rotatedOffset = offsetRotation * new UnityEngine.Vector3(offsetX, offsetY, offsetZ);
                var fixedPositionTarget = pca.PlayerState;
                fixedPositionTarget.FixedPosition = new UnityEngine.Vector3(
                    areaPosition.x + rotatedOffset.x, areaPosition.y + rotatedOffset.y,
                    areaPosition.z + rotatedOffset.z);

                var firstRotationSource = pca.PlayerState.LastInteractModel.Controller;
                if (firstRotationSource is null) throw new System.NullReferenceException();
                var firstRotationTransform = firstRotationSource.transform; // ICall625CB6
                if (firstRotationTransform is null) throw new System.NullReferenceException();
                var firstRotation = firstRotationTransform.rotation; // ICall625CFA
                var firstEuler = firstRotation.eulerAngles; // direct3FA4B0
                var secondRotationSource = pca.PlayerState.LastInteractModel.Controller;
                if (secondRotationSource is null) throw new System.NullReferenceException();
                var secondRotationTransform = secondRotationSource.transform; // ICall625D82
                if (secondRotationTransform is null) throw new System.NullReferenceException();
                var secondRotation = secondRotationTransform.rotation; // ICall625DC6
                var secondEuler = secondRotation.eulerAngles; // direct3FA4B0
                var rotationAction = pca.PlayerState.CurrentAction;
                var fixedRotationTarget = pca.PlayerState; // capture before ICall625E7D
                float yaw = secondEuler.y + rotationAction.RotateYOffset;
                // Original inline multiplies x/y/z by 0.017453292f then Internal_FromEulerRad.
                // Public Euler is the existing API boundary; native parity is pending.
                var resultRotation = UnityEngine.Quaternion.Euler(firstEuler.x, yaw, firstEuler.z);
                fixedRotationTarget.FixedRotation = resultRotation;
                if ((int)pca.PlayerState.TransAction != -1 && !pca.PlayerState.IsEcstasyMotion)
                {
                    next = (int)pca.PlayerState.TransAction;
                    ChangeAction((ActionType)next);
                }
            }
        }
    }

    // PHASE common/seat dispatch: original625ED7 onward, cold CommonEquip6277D8.
    int typeAfterFixed = (int)pca.PlayerState.CurrentAction.Type;
    if (typeAfterFixed == 10008)
    {
        float capturedTime = UnityEngine.Time.time; // ICall627800
        float capturedEndTime = pca.PlayerState.CommonEquipEndTime;
        // Raw COMISS/JBE returns also when either operand is NaN.
        if (!(capturedTime > capturedEndTime)) return;
        var onEnd = pca.PlayerState.OnEndCommonEquip;
        if (onEnd != null) onEnd(); // delegate indirect627849
        goto CancelAction;
    }

    int seatType = (int)pca.PlayerState.CurrentAction.Type;
    if (seatType == 10026 || seatType == 10027 || unchecked((uint)(seatType - 10101)) < 4u || seatType == 10105)
    {
        // PHASE seat-warp: original6270xx..6277D3; saved lines438..688.
        var seatVector = pca.PlayerState.LastInteractModel.Controller.VectorParam;
        var seatForwardTransform = pca.Transform;
        if (seatForwardTransform is null) throw new System.NullReferenceException();
        var seatForward = seatForwardTransform.forward;
        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(UnityEngine.Vector3).TypeHandle);
        var seatUp = UnityEngine.Vector3.up;
        var seatOffsetRotation = UnityEngine.Quaternion.LookRotation(seatForward, seatUp); // ICall6270A3
        var heightAccessor = pca;
        if (heightAccessor is null) throw new System.NullReferenceException();
        var heightAdjuster = heightAccessor.PlayerBodyCustomizeAdjuster;
        if (heightAdjuster is null) throw new System.NullReferenceException();
        float seatOffsetY = (heightAdjuster.HeightScale - 1f) * -0.47f;
        float seatOffsetZ = (heightAccessor.PlayerBodyCustomizeAdjuster.HeightScale - 1f) * 0.19f;
        var seatHeightOffset = seatOffsetRotation * new UnityEngine.Vector3(0f, seatOffsetY, seatOffsetZ);
        SitOffsetV = new UnityEngine.Vector3(seatVector.x + seatHeightOffset.x,
            seatVector.y + seatHeightOffset.y, seatVector.z + seatHeightOffset.z);

        var seatPositionSource = pca.PlayerState.LastInteractModel.Controller;
        if (seatPositionSource is null) throw new System.NullReferenceException();
        var seatPositionTransform = seatPositionSource.transform; // ICall6271C2
        if (seatPositionTransform is null) throw new System.NullReferenceException();
        var seatPosition = seatPositionTransform.position; // ICall62720B
        var capturedSitOffset = SitOffsetV;
        var targetSeatPosition = new UnityEngine.Vector3(seatPosition.x + capturedSitOffset.x,
            seatPosition.y + capturedSitOffset.y, seatPosition.z + SitOffsetV.z);
        var seatRotationSource = pca.PlayerState.LastInteractModel.Controller;
        if (seatRotationSource is null) throw new System.NullReferenceException();
        var seatRotationTransform = seatRotationSource.transform; // ICall6272AE
        if (seatRotationTransform is null) throw new System.NullReferenceException();
        var targetSeatRotation = seatRotationTransform.rotation; // ICall6272F2
        if ((int)pca.PlayerState.CurrentAction.Type == 10027)
        {
            var standPositionSource = pca.PlayerState.LastInteractModel.Controller;
            if (standPositionSource is null) throw new System.NullReferenceException();
            var standPositionTransform = standPositionSource.transform;
            if (standPositionTransform is null) throw new System.NullReferenceException();
            targetSeatPosition = standPositionTransform.position; // ICall6273A0
            if (pca.PlayerState.LastInteractModel.Controller.BoolParam)
            {
                System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(UnityEngine.Vector3).TypeHandle);
                var flipAxis = UnityEngine.Vector3.up;
                var flip = UnityEngine.Quaternion.AngleAxis(180f, flipAxis); // ICall62747E
                // Raw scalar product order: targetSeatRotation * flip (not reversed).
                float bx = targetSeatRotation.x, by = targetSeatRotation.y;
                float bz = targetSeatRotation.z, bw = targetSeatRotation.w;
                float ax = flip.x, ay = flip.y, az = flip.z, aw = flip.w;
                targetSeatRotation = new UnityEngine.Quaternion(
                    ((ax * bw + aw * bx) + az * by) - ay * bz,
                    ((ay * bw + aw * by) + ax * bz) - az * bx,
                    ((az * bw + aw * bz) + ay * bx) - ax * by,
                    ((aw * bw - ax * bx) - ay * by) - az * bz);
            }
        }
        pca.PlayerState.IsFixedWarpPositionRotation = true;
        var warpPositionAccessor = pca;
        if (warpPositionAccessor is null) throw new System.NullReferenceException();
        var warpPositionTarget = warpPositionAccessor.PlayerState;
        var warpPositionTransform = pca.Transform;
        if (warpPositionTransform is null) throw new System.NullReferenceException();
        var warpCurrentPosition = warpPositionTransform.position; // ICall627607
        warpPositionTarget.FixedWarpPosition = new UnityEngine.Vector3(
            (targetSeatPosition.x - warpCurrentPosition.x) * 0.1f + warpCurrentPosition.x,
            (targetSeatPosition.y - warpCurrentPosition.y) * 0.1f + warpCurrentPosition.y,
            (targetSeatPosition.z - warpCurrentPosition.z) * 0.1f + warpCurrentPosition.z);
        var warpRotationAccessor = pca;
        if (warpRotationAccessor is null) throw new System.NullReferenceException();
        var warpRotationTarget = warpRotationAccessor.PlayerState;
        var warpRotationTransform = pca.Transform;
        if (warpRotationTransform is null) throw new System.NullReferenceException();
        var warpCurrentRotation = warpRotationTransform.rotation; // ICall6276C6
        var warpResultRotation = UnityEngine.Quaternion.Lerp(warpCurrentRotation, targetSeatRotation, 0.2f); // ICall62771E; raw627708 bits3E4CCCCD
        warpRotationTarget.FixedWarpRotation = warpResultRotation;
        if (pca.PlayerState.IsSit)
        {
            if (inputData is null) throw new System.NullReferenceException();
            if (inputData.JustMove && !pca.PlayerState.IsEcstasyMotion)
            {
                int sitType = (int)pca.PlayerState.CurrentAction.Type;
                if (sitType == 10026 || sitType == 10101)
                {
                    next = 10027;
                    goto ApplyAction;
                }
            }
        }
        if ((int)pca.PlayerState.TransAction == -1) return;
        if (pca.PlayerState.IsEcstasyMotion) return;
        next = (int)pca.PlayerState.TransAction;
        goto ApplyAction;
    }

    // PHASE standard input: original625F77..627030, saved lines690..1213.
    if (inputData is null) throw new System.NullReferenceException();
    if (inputData.JustOnaniNormal)
    {
        if (!playerState.IsEcstasyMotion)
        {
            var capturedOnaniFacade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
            if (capturedOnaniFacade is null) throw new System.NullReferenceException();
            // Original inline CanUseHand(true) reads capturedOnaniFacade.private pca.
            // Fold to its existing public member; receiver/null prefix is preserved.
            // This added managed dispatch boundary requires native/CIL observation.
            if (capturedOnaniFacade.CanUseHand(true))
            {
                if ((int)playerState.CurrentAction.Type != 0)
                {
                    next = 0;
                    goto ApplyAction;
                }
                goto CancelAction;
            }
        }
    }
    if ((int)inputData.JustPoseSkill != 0) goto SelectPose;

RegularActionChecks:
    // Original626367: one pose demands exposed front and active visible genital state.
    if ((int)playerState.CurrentAction.Type == 50019)
    {
        var exposureFacade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
        if (exposureFacade is null) throw new System.NullReferenceException();
        uint exposed = (uint)exposureFacade.GetExposedBodyPartsByClothesAndCostume();
        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
        if ((exposed & 0x2Au) == 0u) goto CancelAction;
        if (pca.PlayerState.IsHideFutaByCosplay) goto CancelAction;
    }

RegularContinuation:
    if ((int)playerState.CurrentAction.Type == 50013)
    {
        var exposureFacade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
        if (exposureFacade is null) throw new System.NullReferenceException();
        uint exposed = (uint)exposureFacade.GetExposedBodyPartsByClothesAndCostume();
        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
        if ((exposed & 0x2Au) == 0u) goto CancelAction;
    }
    float firstMoveY = inputData.Move.y;
    float firstMoveX = inputData.Move.x;
    float firstMoveZ = inputData.Move.z;
    float firstMoveMagnitude = (firstMoveX * firstMoveX + firstMoveY * firstMoveY) + firstMoveZ * firstMoveZ;
    if (firstMoveMagnitude > 0f)
    {
        var movementState = playerState;
        if (movementState is null) throw new System.NullReferenceException();
        if (!movementState.IsEcstasyMotion && movementState.CurrentAction.IsMoveToCancel)
        {
            cooldown = pca.PlayerState.PoseActionCoolTime;
            goto CancelWhenCooldownAllows;
        }
    }
    if (inputData.Dash)
    {
        float dashMoveY = inputData.Move.y;
        float dashMoveX = inputData.Move.x;
        float dashMoveZ = inputData.Move.z;
        float dashMagnitude = (dashMoveX * dashMoveX + dashMoveY * dashMoveY) + dashMoveZ * dashMoveZ;
        if (dashMagnitude > 0f)
        {
            var dashState = playerState;
            if (dashState is null) throw new System.NullReferenceException();
            if (!dashState.IsEcstasyMotion && dashState.CurrentAction.CanMove && !dashState.CurrentAction.CantCancelDash && (int)dashState.CurrentAction.Type != -1)
                goto CancelAction;
        }
    }
    if (inputData.JustDrop && (int)playerState.CurrentAction.Type == -1)
    {
        var heldGameState = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
        if (heldGameState is null) throw new System.NullReferenceException();
        if ((int)heldGameState.PlayerCurrentHadObject != -1)
        {
            if (pca is null) throw new System.NullReferenceException();
            heldGameState = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
            if (heldGameState is null) throw new System.NullReferenceException();
            var dropTarget = pca.PlayerState;
            if (dropTarget is null) throw new System.NullReferenceException();
            dropTarget.LastDropType = heldGameState.PlayerCurrentHadObject;
            next = 10002;
            goto ApplyAction;
        }
    }
    if (inputData.JustDropHandcuffKey && !playerState.IsPlayingAction)
    {
        bool attached = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsAttachSomeHandcuffs();
        messageKey = "log_not_equip_keyhandcuff";
        if (!attached) goto MessageAndReturn;
        var handcuffGameState = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
        if (handcuffGameState is null) throw new System.NullReferenceException();
        if ((int)handcuffGameState.AttachedHandcuffType != 4) goto MessageAndReturn;
        int keyCount = handcuffGameState.ItemDict[(global::ExposureUnnoticed2.Master.Item.ItemType)10000];
        messageKey = "log_not_have_handcuff_key";
        if (keyCount == 0) goto MessageAndReturn;
        pca.PlayerState.LastDropItemType = (global::ExposureUnnoticed2.Scripts.InGame.DroppedItemManager.DropItemType)5;
        next = 10002;
        goto ApplyAction;
    }
    if (inputData.JustDropVibeRemocon && !playerState.IsPlayingAction)
    {
        var remoteGameState = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
        if (remoteGameState is null) throw new System.NullReferenceException();
        int remoteCount = remoteGameState.ItemDict[(global::ExposureUnnoticed2.Master.Item.ItemType)10001];
        messageKey = "log_not_have_vibe_remocon";
        if (remoteCount == 0) goto MessageAndReturn;
        pca.PlayerState.LastDropItemType = (global::ExposureUnnoticed2.Scripts.InGame.DroppedItemManager.DropItemType)6;
        next = 10002;
        goto ApplyAction;
    }
    if (inputData.JustInteract && playerState.CurrentAction.AnotherMotionCount > 0)
    {
        var capturedAnimation = pca.PlayerAnimationManager;
        if (capturedAnimation is null) throw new System.NullReferenceException();
        capturedAnimation.RotateAnotherMotionIndex(false);
        return;
    }
    if (inputData.JustPants && !playerState.IsPlayingAction)
    {
        var pants = pca.PlayerAvatarObjectReferencer.PantsParent;
        if (pants is null) throw new System.NullReferenceException();
        if (pants.activeInHierarchy)
        {
            var pantsFacade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
            if (pantsFacade is null) throw new System.NullReferenceException();
            if (!pantsFacade.CanUseHand(true)) return;
            bool released = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsRelease((global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)0);
            messageKey = "log_not_rank_underwear";
            if (!released) goto MessageAndReturn;
            next = 10014;
            goto ApplyAction;
        }
    }
    if (inputData.JustBra && !playerState.IsPlayingAction)
    {
        var bra = pca.PlayerAvatarObjectReferencer.BraParent;
        if (bra is null) throw new System.NullReferenceException();
        if (bra.activeInHierarchy)
        {
            var braFacade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
            if (braFacade is null) throw new System.NullReferenceException();
            if (!braFacade.CanUseHand(true)) return;
            bool released = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsRelease((global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)0);
            messageKey = "log_not_rank_underwear";
            if (!released) goto MessageAndReturn;
            next = 10016;
            goto ApplyAction;
        }
    }
    if (!playerState.IsPlayingAction && pca.PlayerState.TransPick)
    {
        next = 10001;
        goto ApplyAction;
    }
    if (inputData.JustSwitchVibrator && (int)pca.PlayerState.CurrentAction.Type != 10024)
    {
        var vibeFacade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
        if (vibeFacade is null) throw new System.NullReferenceException();
        if (vibeFacade.CanUseHand(true))
        {
            var vibeGameState = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
            if (vibeGameState is null) throw new System.NullReferenceException();
            int remoteCount = vibeGameState.ItemDict[(global::ExposureUnnoticed2.Master.Item.ItemType)10001];
            string vibeMessage = "log_not_have_vibe_remocon";
            if (remoteCount != 0)
            {
                var vibeState = pca.PlayerState;
                if (vibeState is null) throw new System.NullReferenceException();
                vibeMessage = "log_not_equip_vibe";
                if (vibeState.EcstasyRiseByVibratorPerSecond > 0f)
                {
                    next = 10024;
                    goto ApplyAction;
                }
            }
            global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(vibeMessage));
        }
    }
    if (inputData.JustSwitchPistonMachine && (int)pca.PlayerState.CurrentAction.Type != 8)
    {
        var pistonFacade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
        if (pistonFacade is null) throw new System.NullReferenceException();
        if (pistonFacade.CanUseHand(true))
        {
            if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringAdultGoods((global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoodsType)8)
                || global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringAdultGoods((global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoodsType)9)
                || global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringAdultGoods((global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoodsType)10))
            {
                next = 8;
                goto ApplyAction;
            }
            global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("log_not_have_piston_machine"));
        }
    }
    if (inputData.JustTimeStop && (int)pca.PlayerState.CurrentAction.Type != 7)
    {
        var stopFacade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
        if (stopFacade is null) throw new System.NullReferenceException();
        if (stopFacade.CanUseHand(true))
        {
            bool active = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsActivePassiveSkill((global::ExposureUnnoticed2.Master.Skill.SkillType)17);
            messageKey = "log_not_active_time_stop";
            if (!active) goto MessageAndReturn;
            next = 7;
            goto ApplyAction;
        }
    }
    if (inputData.JustRingDildo)
    {
        var itemFacade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
        if (itemFacade is null) throw new System.NullReferenceException();
        if (itemFacade.CanUseHand(true))
        {
            bool haveItem = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringItem((global::ExposureUnnoticed2.Master.Item.ItemType)1);
            messageKey = "log_not_have_dildo";
            if (!haveItem) goto MessageAndReturn;
            var record = global::ExposureUnnoticed2.Master.Item.MItem.Get((global::ExposureUnnoticed2.Master.Item.ItemType)1);
            global::ExposureUnnoticed2.Master.Item.MItem.UseItemProcess(record);
            return;
        }
    }
    if (inputData.JustRingHandcuff)
    {
        var itemFacade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
        if (itemFacade is null) throw new System.NullReferenceException();
        if (itemFacade.CanUseHand(true))
        {
            var record = global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoods.Get((global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoodsType)3);
            global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoods.UseItemProcess(record);
            return;
        }
    }
    if (inputData.JustRingEyeMask)
    {
        var itemFacade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
        if (itemFacade is null) throw new System.NullReferenceException();
        if (itemFacade.CanUseHand(true))
        {
            var record = global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoods.Get((global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoodsType)2);
            global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoods.UseItemProcess(record);
        }
    }
    if (inputData.JustDrinkWater)
    {
        var waterFacade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
        if (waterFacade is null) throw new System.NullReferenceException();
        if (waterFacade.CanUseHand(true))
        {
            bool haveWater = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringItem((global::ExposureUnnoticed2.Master.Item.ItemType)0);
            messageKey = "ui_NoWater";
            if (!haveWater) goto MessageAndReturn;
            var record = global::ExposureUnnoticed2.Master.Item.MItem.Get((global::ExposureUnnoticed2.Master.Item.ItemType)0);
            global::ExposureUnnoticed2.Master.Item.MItem.UseItemProcess(record);
        }
    }
    if (inputData.JustEyeMask)
    {
        var eyeFacade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
        if (eyeFacade is null) throw new System.NullReferenceException();
        if (eyeFacade.CanUseHand(true))
        {
            bool haveEyeMask = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringAdultGoods((global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoodsType)2);
            messageKey = "ui_noEyeMask";
            if (!haveEyeMask) goto MessageAndReturn;
            var record = global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoods.Get((global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoodsType)2);
            global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoods.UseItemProcess(record);
        }
    }
    System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.InteractManager).TypeHandle);
    var head = global::ExposureUnnoticed2.Scripts.Base.InteractManager.GetHead();
    if (head is null) throw new System.NullReferenceException();
    if ((int)head.Type == 4 && pca.PlayerState.TransPickClothes)
    {
        var clothesFacade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
        if (clothesFacade is null) throw new System.NullReferenceException();
        if (clothesFacade.CanUseHand(true))
        {
            var clothesGameState = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
            if (clothesGameState is null) throw new System.NullReferenceException();
            if ((int)clothesGameState.PlayerCurrentClothesAState == 5)
            {
                var targetAction = pca.PlayerAction;
                if (targetAction is null) throw new System.NullReferenceException();
                targetAction.ChangeAction((ActionType)10000);
            }
            else
                global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("log_already_wear_clothes"));
        }
    }
    if (playerState.IsPlayingAction) return;
    if ((int)pca.PlayerState.TransAction == -1) return;
    var pendingFacade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
    if (pendingFacade is null) throw new System.NullReferenceException();
    bool skipHandcuff = pca.PlayerState.ForceSkipHandcuffCheckTransAction;
    if (!pendingFacade.CanUseHand(true, skipHandcuff)) return;
    pca.PlayerState.ForceSkipHandcuffCheckTransAction = false;
    next = (int)pca.PlayerState.TransAction;
    goto ApplyAction;

SelectPose:
    {
        int capturedCurrentType = (int)pca.PlayerState.CurrentAction.Type;
        var poseFacade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
        if (poseFacade is null) throw new System.NullReferenceException();
        bool skipCurrentAction = capturedCurrentType > 49999 && capturedCurrentType < 60001;
        if (!poseFacade.CanUseHand(true, false, false, skipCurrentAction, false)) goto RegularActionChecks;
        var poseState = pca.PlayerState;
        if (poseState is null) throw new System.NullReferenceException();
        messageKey = "log_cant_use_hand_by_other";
        if (poseState.IsGamanBaibu) goto MessageAndReturn;
        var skill = global::ExposureUnnoticed2.Master.Skill.MSkill.GetByIndex(unchecked((int)inputData.JustPoseSkill - 10000));
        if (skill is null) throw new System.NullReferenceException();
        next = unchecked((int)skill.Type + 40000);
        if (next == 50019)
        {
            var exposureFacade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
            if (exposureFacade is null) throw new System.NullReferenceException();
            uint exposed = (uint)exposureFacade.GetExposedBodyPartsByClothesAndCostume();
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
            messageKey = "log_cant_by_expose_front";
            if ((exposed & 0x2Au) == 0u) goto MessageAndReturn;
            if (pca.PlayerState.IsHideFutaByCosplay) goto MessageAndReturn;
            var poseGameState = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
            if (poseGameState is null) throw new System.NullReferenceException();
            messageKey = "log_cant_by_not_futanari";
            if (!poseGameState.IsActiveFutanari) goto MessageAndReturn;
        }
        else if (next == 50013)
        {
            var exposureFacade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
            if (exposureFacade is null) throw new System.NullReferenceException();
            uint exposed = (uint)exposureFacade.GetExposedBodyPartsByClothesAndCostume();
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
            messageKey = "log_cant_by_expose_front";
            if ((exposed & 0x2Au) == 0u) goto MessageAndReturn;
        }
        var previousAction = playerState.CurrentAction;
        if (previousAction is null) throw new System.NullReferenceException();
        var cooldownAccessor = pca;
        if (cooldownAccessor is null) throw new System.NullReferenceException();
        var cooldownState = cooldownAccessor.PlayerState;
        if ((int)previousAction.Type != next)
        {
            if (cooldownState is null) throw new System.NullReferenceException();
            if (!(0f >= cooldownState.PoseActionCoolTime)) return; // raw26306 COMISS/JB; NaN returns
            cooldownAccessor.PlayerState.PoseActionCoolTime = 0.5f;
            goto ApplyAction;
        }
        if (cooldownState is null) throw new System.NullReferenceException();
        cooldown = cooldownState.PoseActionCoolTime;
        goto CancelWhenCooldownAllows;
    }

CancelWhenCooldownAllows:
    // Raw26337/264C3 COMISS(0,cooldown), JAE cancel; unordered (NaN) returns.
    if (!(0f >= cooldown)) return;
CancelAction:
    next = -1;
ApplyAction:
    // Original62607B: same PlayerAction target, no fixture/helper substitution.
    ChangeAction((ActionType)next);
    return;
MessageAndReturn:
    // Original626350 shared fallback: Localizer then MessagePanel, then return.
    global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(messageKey));
    return;
}

		public void ChangeAction(ActionType next)
		{
    // Complete original 627A90..627F7C family, including its nine-way table.
    int oldType = (int)playerState.CurrentAction.Type;
    int requested = (int)next;
    if ((oldType > 10029 && oldType < 10099) ||
        ((unchecked((uint)(oldType - 10026)) < 2u ||
          unchecked((uint)(oldType - 10101)) < 4u || oldType == 10105) && requested == 10027))
    {
        var capturedRing = global::ExposureUnnoticed2.ObjectUI.InGame.RingMenu.RingMenuParentView.Instance;
        if (object.ReferenceEquals(capturedRing, null)) throw new global::System.NullReferenceException();
        capturedRing.SetOverride((global::ExposureUnnoticed2.ObjectUI.InGame.RingMenu.RingMenuParentView.RingMenuOverrideType)0);
    }
    // The original clear above occurs even when this per-frame guard is set.
    if (IsAlreadyChangeActionThisFrame) return;
    if (requested < 10003)
    {
        if (requested == -1) pca.PlayerLocomotion.OnEndAction();
        else if (requested == 10002)
        {
            pca.PlayerAnimationManager.SetHoldInHand(false);
            pca.PlayerAnimationManager.SetHadBasket(false);
        }
    }
    else if (requested == 10009) pca.PlayerAnimationManager.SetHoldInHand(false);
    else if (requested != 10010 && requested != 10011)
    {
        if (requested == 10012) pca.PlayerAvatarObjectReferencer.BottleWater.SetActive(true);
        else if (requested == 10013)
        {
            pca.PlayerAvatarObjectReferencer.PeeParticle.gameObject.SetActive(true);
            pca.PlayerAvatarObjectReferencer.PeeParticle.Play(true);
            pca.PlayerSoundManager.PlaySe((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)16);
        }
        else
        {
            switch (requested)
            {
                case 10019: case 10020: case 10022:
                    pca.PlayerSoundManager.SetPickingSeSoundIfNeed(true);
                    break;
                case 10021: case 10023: case 10024: case 10025:
                    break;
                case 10026:
                case 10101:
                    var capturedRing = global::ExposureUnnoticed2.ObjectUI.InGame.RingMenu.RingMenuParentView.Instance;
                    if (object.ReferenceEquals(capturedRing, null)) throw new global::System.NullReferenceException();
                    capturedRing.SetOverride((global::ExposureUnnoticed2.ObjectUI.InGame.RingMenu.RingMenuParentView.RingMenuOverrideType)7);
                    break;
                case 10027:
                    pca.PlayerState.IsSit = false;
                    break;
            }
        }
    }
    var resetState = pca.PlayerState;
    global::System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.InteractManager).TypeHandle);
    if (resetState == null) throw new global::System.NullReferenceException();
    resetState.LastInteractModel = global::ExposureUnnoticed2.Scripts.Base.InteractManager.CurrentInteractModel;
    // Capture this separate receiver before the original master lookup call.
    var capturedPlayerState = playerState;
    var nextRecord = global::ExposureUnnoticed2.Master.Action.MAction.Get(next);
    if (capturedPlayerState == null) throw new global::System.NullReferenceException();
    capturedPlayerState.CurrentAction = nextRecord;
    var actionAfterLookup = playerState.CurrentAction;
    if (object.ReferenceEquals(actionAfterLookup, null)) throw new global::System.NullReferenceException();
    var capturedAnimation = playerAnimationManager;
    if (capturedAnimation == null) throw new global::System.NullReferenceException();
    capturedAnimation.TransAction(actionAfterLookup.Type);
    if (requested != -1) IsAlreadyChangeActionThisFrame = true;
    // Existing original helper, not a new reflection/access bridge. Its reset
    // null-prefix and side effects must be compared with this original inline.
    pca.PlayerAnimationManager.RotateAnotherMotionIndex(true);
}
	}
}
