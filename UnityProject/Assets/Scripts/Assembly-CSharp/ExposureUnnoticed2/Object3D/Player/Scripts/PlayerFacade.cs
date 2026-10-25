using ExposureUnnoticed2.Master.Action;
using ExposureUnnoticed2.Master.AdultGoods;
using ExposureUnnoticed2.Master.Clothes;
using ExposureUnnoticed2.Master.Cosplay;
using ExposureUnnoticed2.Master.Hair;
using ExposureUnnoticed2.Master.Mission;
using ExposureUnnoticed2.Object3D.Light;
using ExposureUnnoticed2.Object3D.NPC.Script;
using ExposureUnnoticed2.ObjectUI.InventoryPanel;
using ExposureUnnoticed2.Scripts.Sound;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerFacade
	{
		public static PlayerFacade Instance;

		private PlayerClassAccessor pca;

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x6480D0
			Instance = this;
			this.pca = pca;
		}

		public void Dispose()
		{
			// Recovered from GameAssembly.dll RVA 0x6481D0
			Instance = null;
		}

		public bool IsIgnoreBaretaState()
		{
			return false;
		}

		public void SetHadBasket(bool hadBasket)
		{
		}

		public void SetHoldClothes(ClothesType type)
		{
			// Recovered from GameAssembly.dll RVA 0x6483C0
			if ((int)type != -1 && (int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentHadObject != -1)
			    throw new System.Exception("なにかを持っているのに服を持とうとしています");
			global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentHoldClothes = type;
			var state = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
			if ((int)type == -1)
			{
			    if ((int)state.PlayerCurrentHadObject == 1) state.PlayerCurrentHadObject = (global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerStateModel.HadObjectType)(-1);
			}
			else if ((int)state.PlayerCurrentHadObject == -1) state.PlayerCurrentHadObject = (global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerStateModel.HadObjectType)1;
		}

		public void TransPick(PlayerStateModel.HadObjectType dropType, ClothesType type = ClothesType.None)
		{
		}

		public void TransPickClothes(ClothesType type)
		{
			// Recovered from GameAssembly.dll RVA 0x648530
			pca.PlayerState.LastPickClothesType = type;
			pca.PlayerState.TransPickClothes = true;
		}

		public void TransAction(ActionType type)
		{
			// Recovered from GameAssembly.dll RVA 0x648570
			pca.PlayerState.TransAction = type;
		}

		public bool TransActionIfNewNext(ActionType type)
		{
            if (pca.PlayerState.CurrentAction.Type == type) return false;
            pca.PlayerState.TransAction = type;
            return true;
        }

		public ClothesType GetAndRemoveHoldInHandClothes()
		{
			return ClothesType.Coat;
		}

		public void SmoothMoveTo(Transform transform)
		{
			pca.PlayerState.SmoothTransformTarget = transform;
			pca.PlayerState.SmoothRemainTime = 1f;
		}

		public void SmoothRotateY(float y)
		{
global::UnityEngine.Vector3 euler = pca.Transform.rotation.eulerAngles;
System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
float angle = (euler.y + y) % 360f;
if (angle > 180f) angle -= 360f;
else if (angle < -180f) angle += 360f;
var state = pca.PlayerState;
global::UnityEngine.Quaternion target = global::UnityEngine.Quaternion.Euler(euler.x, angle, euler.z);
if (state is null) throw new System.NullReferenceException();
state.SmoothRotateTarget = target;
}

		public void ChangeAccessoryIfAble(IEquipableMaster record)
		{
            // Recovered complete original native body.
            if (!Instance.CanUseHand(true)) return;
            pca.PlayerState.EquippingAdultGoods = null;
            pca.PlayerState.EquippingCostume = null;
            if (record == null) return;
            if (record is global::ExposureUnnoticed2.Master.Accessory.MAdultGoodsRecord accessory)
            {
                pca.PlayerState.EquippingAdultGoods = accessory;
                if (record.TransDetachActionType() == (ActionType)(-1)) pca.PlayerAnimationReceiver.JustEquipAccessory();
                else
                {
                    ActionType action = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipAdultGoods(accessory.Type) ? record.TransDetachActionType() : record.TransAttachActionType();
                    pca.PlayerState.TransAction = action;
                }
            }
            if (record is global::ExposureUnnoticed2.Master.Costume.RCostume costume)
            {
                pca.PlayerState.EquippingCostume = costume;
                ActionType action = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipCostume(costume) ? record.TransDetachActionType() : record.TransAttachActionType();
                pca.PlayerState.TransAction = action;
            }
        }

		public void ForceChangeAdultGoods(MAdultGoodsType type, bool isActive)
		{
		}

		public void ChangeHair(MHairRecord record)
		{
            pca.PlayerHairController.ChangeHair(record.Type);
		}

		public bool IsHoldInHandClothes()
		{
			return false;
		}

		public bool IsPlayingAction()
		{
			return false;
		}

		public void AddAddMoisture(float add)
		{
            // Complete original RVA 0x648F40.
            var data = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
            data.PlayerAddMoisture = add + data.PlayerAddMoisture;
        }

		public void ChangeClothes(MClothesRecord record)
		{
            global::ExposureUnnoticed2.Scripts.Base.Events.ChangeEquipmentEvent.ClothesRecord = record;
            global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ChangeEquipmentEvent);
		}

		public bool IsHideSomePartsByCosplay()
		{
// Complete original method; original state latch, sampler order and independent player graphs.
return (uint)pca.PlayerState.HiddenBodyPartsByCostume != 0u;
}

		public ExposedBodyParts GetExposedBodyPartsByClothesAndCostume()
		{
			// Recovered from GameAssembly.dll RVA 0x6490D0
			uint exposed = (uint)pca.PlayerClothes.GetExposedBodyParts();
			uint hidden = (uint)pca.PlayerState.HiddenBodyPartsByCostume;
			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
			bool genitalHidden = (hidden & 4u) != 0u && (!global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveFutanari || pca.PlayerState.IsHideFutaByCosplay);
			bool frontHidden = genitalHidden && (hidden & 1u) != 0u;
			bool sideHidden = (hidden & 8u) != 0u;
			bool hipHidden = (hidden & 2u) != 0u || (hidden & 16u) != 0u;
			unchecked
			{
			    // Original branch priority is Front, then Upper, then All; unknown/composite flags are retained.
			    if ((exposed & 2u) != 0u)
			    {
			        if (frontHidden) exposed = (exposed - 2u) | 1u;
			    }
			    else if ((exposed & 8u) != 0u)
			    {
			        if (frontHidden) exposed = (exposed - 8u) | 1u;
			        else if (sideHidden) exposed = (exposed - 8u) | 2u;
			    }
			    else if ((exposed & 32u) != 0u)
			    {
			        if (frontHidden && sideHidden && hipHidden) exposed = 1u;
			        else if (frontHidden || sideHidden || hipHidden)
			        {
			            exposed = (exposed - 32u) | 14u;
			            if (frontHidden) exposed -= 2u;
			            if (sideHidden) exposed -= 8u;
			            // Original JMP0x6492D9 targets shared OR1 tail0x64936A after Hip subtraction.
			            if (hipHidden) exposed = (exposed - 4u) | 1u;
			        }
			    }
			    if ((exposed & 4u) != 0u && hipHidden) exposed = (exposed - 4u) | 1u;
			}
			return (ExposedBodyParts)exposed;
		}

		public void SetAnimatorSpeed(float val)
		{
// Complete original RVA 0x6493F0; PE/typed identity is audited in this owned stage.
pca.Animator.speed = val;
}

		public void SetAnimationMoveSpeed(float val)
		{
// Complete original RVA 0x649470; PE/typed identity is audited in this owned stage.
pca.PlayerAnimationManager.TargetMoveSpeed = val;
}

		public void SetFaceLayerWeight(float val)
		{
// Complete original RVA 0x6494A0; PE/typed identity is audited in this owned stage.
pca.PlayerAnimationManager.SetFaceLayerWeight(val);
}

		public void ActiveUnderwear()
		{
		}

		public bool CanPutHandcuffsToMap()
		{
			return false;
		}

		public Transform GetTransform()
		{
			return null;
		}

		public bool CanUseHand(bool isSendMessage, bool isSkipHandcuff = false, bool isSkipBasket = false, bool isSkipPlayingAction = false, bool isSkipCrouch = false)
		{
			// Reconstructed from original RVA649610. isSkipBasket is intentionally unused in the original native body.
			string reason = "log_cant_use_hand_by_other";
			if (!isSkipHandcuff && global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsAttachSomeHandcuffs())
			    reason = "log_cant_use_hand_by_handcuff";
			else if (!isSkipCrouch && pca.PlayerState.IsCrouch)
			    reason = "log_cant_use_hand_by_crouch";
			else if (((int)pca.PlayerState.CurrentAction.Type == -1 || isSkipPlayingAction) &&
			         (int)pca.PlayerState.CurrentState == 0 && !pca.PlayerState.IsEcstasyMotion &&
			         !global::ExposureUnnoticed2.Scripts.InGame.SexManager.Instance.IsSexMode)
			    return true;
			if (isSendMessage)
			    global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(
			        global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(reason));
			return false;
		}

		public StrangeData CalcStrangerDegreeFrom(Transform npcTransform, Transform head, NpcComponentAccessor nca)
		{
			return pca.PlayerStrangenessController.CalcStrangerDegreeFrom(npcTransform, head, nca);
		}

		public void UpdateTouchLight(StreetLightController light, bool isTouch)
		{
		}

		public void SetCosplayActive(CosplayType type, int id, bool isActive)
		{
			pca.PlayerCosplayManager.SetCosplayActive(type, id, isActive, false);
		}

		public void PlaySe(InGameSoundManager.SeType type)
		{
		}

		public void Push(Vector3 pushV, bool isFixed = false)
		{
		}
	}
}
