using ExposureUnnoticed2.Scripts.InGame;
using ExposureUnnoticed2.Scripts.Sound;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerAnimationReceiver : MonoBehaviour
	{
		private PlayerSoundManager playerSoundManager;

		private PlayerStateModel playerStateModel;

		private DroppedItemManager droppedItemManager;

		private PlayerAnimationManager playerAnimationManager;

		private PlayerClassAccessor pca;

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x6290C0
			this.pca = pca;
			playerSoundManager = pca.PlayerSoundManager;
			playerStateModel = pca.PlayerState;
			droppedItemManager = pca.DroppedItemManager;
			playerAnimationManager = pca.PlayerAnimationManager;
		}

		private void OnPlaySe(InGameSoundManager.SeType type)
		{
            // Original RVA 0x6292A0, complete native extent 68 bytes.
            if ((int)type == 8)
                pca.PlayerShiofukiController.EmitShiofuki();
            else
                playerSoundManager.PlaySe(type);
		}

		public void OnChangeTimeStop()
		{
		}

		public void OnSitDildoAction()
		{
		}

		public void CommonOnEndSit()
		{
		}

		public void FootR(AnimationEvent type)
		{
		    // Both FootR and FootL share original RVA 0x629C70.
		    pca.PlayerFootStepController.OnFootStep(type);
		}

		public void FootL(AnimationEvent type)
		{
		    pca.PlayerFootStepController.OnFootStep(type);
		}

		public void OnClothesIntoWasher()
		{
		}

		public void OnClothesTakeFromWasher()
		{
		}

		public void OnAnimDropClothes()
		{
// Complete original 0x629EF0; reread PCA after the entire drop operation.
            pca.PlayerClothes.DropClothes(null);
            pca.PlayerAction.ChangeAction((global::ExposureUnnoticed2.Master.Action.ActionType)(-1));
        }

		public void OnAnimEndPickingCoat()
		{
// Complete original 0x629F50; reread PCA after the entire pick operation.
            pca.PlayerClothes.PickClothes();
            pca.PlayerAction.ChangeAction((global::ExposureUnnoticed2.Master.Action.ActionType)(-1));
        }

		public void OnAnimEndPick()
		{
		}

		public void OnAnimEndPickUpItem()
		{
		}

		public void OnAnimEndEcstasy()
		{
            // Complete original 0x62A220..0x62A2C3, including all three unwind ranges.
            pca.PlayerAnimationManager.SetEcstasy(false);
            global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayUnscaledTime(() =>
            {
                // Original receiver callback <OnAnimEndEcstasy>b__18_0, RVA 0x62C7C0.
                var manager = global::ExposureUnnoticed2.Scripts.InGame.SexManager.Instance;
                if (manager.ActiveTraceSexPlayerNpc is not null)
                {
                    manager.ActiveTraceSexPlayerNpc.Nca.NpcAnimationManager.IsEcstasy(false);
                    manager.ActiveTraceSexPlayerNpc.Nca.NpcAnimationManager.SetFaceIku(false);
                }
                playerStateModel.IsEcstasyMotion = false;
                // Original inline private-field write, folded to its exact existing one-write helper.
                pca.PlayerFaceController.OnEndEcstasy();
                global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerEcstasy = 0f;
                if (!pca.PlayerState.CurrentAction.IsEcstasyToIdle) return;
                pca.PlayerAction.ChangeAction((global::ExposureUnnoticed2.Master.Action.ActionType)(-1));
                pca.PlayerMiscProcess.ReturnInteractTokenIfNeed();
            }, 0.1f);
		}

		public void OnAnimDrop()
		{
		}

		public void JustEffectItem()
		{
		}

		public void OnAnimJustHandOver()
		{
		}

		public void CommonOnEndAnim()
		{
            // Original RVA 0x62AFE0. Preserve receiver rereads after external effects.
            int actionType = (int)pca.PlayerState.CurrentAction.Type;
            if (actionType == 10010)
                pca.PlayerAnimationManager.SetHoldInHand(true);
            else if (actionType == 10011)
            {
                if (!global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenWater)
                {
                    global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.ChangeItemNum((global::ExposureUnnoticed2.Master.Item.ItemType)0, 1);
                    string message = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("item_get_water");
                    global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(message, (global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType)1);
                    global::ExposureUnnoticed2.ObjectUI.TutorialPanel.TutorialPanelView.Open((global::ExposureUnnoticed2.ObjectUI.TutorialPanel.TutorialPanelView.TutorialOpenKind)4);
                }
            }
            else if (actionType == 10012)
                pca.PlayerAvatarObjectReferencer.BottleWater.SetActive(false);
            global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayFlame(() =>
            {
                // Original generated callback RVA 0x62C950; reread pca after ChangeAction.
                pca.PlayerAction.ChangeAction((global::ExposureUnnoticed2.Master.Action.ActionType)(-1));
                pca.PlayerMiscProcess.ReturnInteractTokenIfNeed();
            }, 2);
        }

		public void OnAnimEndChangeClothes()
		{
		}

		public void JustEquipAccessory()
		{
            // Recovered complete original native body.
            if (pca.PlayerState.EquippingAdultGoods)
            {
                bool isEquipped = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipAdultGoods(pca.PlayerState.EquippingAdultGoods.Type);
                var accessor = pca;
                var record = accessor.PlayerState.EquippingAdultGoods;
                accessor.PlayerAdultGoodsController.ChangeAdultGoods(record.Type, !isEquipped);
            }
            else if (pca.PlayerState.EquippingCostume)
            {
                var controller = pca.PlayerCostumeController;
                bool isEquipped = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipCostume(pca.PlayerState.EquippingCostume);
                controller.OnChangeCostume(pca.PlayerState.EquippingCostume, !isEquipped);
            }
            global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ChangeSomeEquipEvent);
        }

		public void EndArmAction()
		{
            // Complete original RVA 0x62B4F0. Cached receiver state is intentional.
            playerStateModel.IsArmAction = false;
        }

		public void CoatStateMove(PlayerStateModel.CoatStateMove type)
		{
            // Complete original RVA 0x62B510; commit the action cache after blend target.
            if (type == PlayerStateModel.CoatStateMove.None) return;
            if (type == PlayerStateModel.CoatStateMove.CloseBtoOpenB1)
                pca.PlayerClothes.TargetClothStateBBlend = PlayerStateModel.ClothesState.OpenB1;
            else if (type == PlayerStateModel.CoatStateMove.OpenB1toCloseB)
                pca.PlayerClothes.TargetClothStateBBlend = PlayerStateModel.ClothesState.CloseB;
            else
                throw new System.ArgumentOutOfRangeException("type", type, null);
            var state = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
            state.PlayerCurrentClothesBState = pca.PlayerState.CurrentClothesBStateCache;
        }

		public void ChangeHoldInHand(int IsRemove)
		{
		}

		public void ChangeVisiblePants(int isVisible)
		{
		}

		public void ChangeVisibleBra(int isVisible)
		{
		}

		public void DropPants()
		{
		}

		public void DropBra()
		{
		}

		public void OnEndSad()
		{
		    // Original GameAssembly RVA 0x62BC50. Production animation event from Sad clips.
		    var existingElevator = global::ExposureUnnoticed2.Object3D.ScenePlops.Elevator.ElevatorController.Instance;
		    // This comparison intentionally uses UnityEngine.Object semantics, as original.
		    if (existingElevator != null)
		    {
		        var elevator = global::ExposureUnnoticed2.Object3D.ScenePlops.Elevator.ElevatorController.Instance;
		        var playerPosition = pca.Transform.position;
		        if (elevator is null) throw new global::System.NullReferenceException();
		        var bounds = elevator.PushBounds;
		        if (bounds.Contains(playerPosition))
		        {
		            pca.PlayerAnimationManager.TransAction(global::ExposureUnnoticed2.Master.Action.ActionType.CrouchCry);
		            return;
		        }
		    }
		    if (pca.PlayerState.CurrentAction.Type == global::ExposureUnnoticed2.Master.Action.ActionType.SadHandcuffAtMap)
		        return;
		    pca.Player.ChangeState(PlayerStateModel.PlayerState.BaretaEscape);
		    pca.PlayerAnimationManager.TransBaretaEscape();
		    pca.PlayerAnimationManager.TargetMoveSpeed = 2f;
		    pca.CharacterController.enabled = false;
		    pca.NavAgent.enabled = true;
		    pca.NavAgent.updatePosition = true;
		    pca.NavAgent.updateRotation = true;
		    // C# evaluates the agent receiver before evaluating destination, and the call
		    // null check after those arguments; retain original observable call order.
		    pca.NavAgent.SetDestination(global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.CurrentStageController.GetEscapeTarget().position);
		}

		private void ChangeVibrator()
		{
		}

		private void OnChangePistonMachine()
		{
		}

		private void CommonStartAnim()
		{
		}

		private void PickDildo()
		{
		}

		public void OnEndSitDown()
		{
		}
	}
}
