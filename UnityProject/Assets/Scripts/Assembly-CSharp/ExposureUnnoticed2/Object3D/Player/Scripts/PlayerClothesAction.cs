using ExposureUnnoticed2.Master.Clothes;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerClothesAction
	{
		private PlayerAnimationManager playerAnimationManager;

		private PlayerSoundManager playerSoundManager;

		private PlayerClassAccessor pca;

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x6426A0
			playerAnimationManager = pca.PlayerAnimationManager;
			playerSoundManager = pca.PlayerSoundManager;
			this.pca = pca;
			pca.PlayerState.CurrentClothesBStateCache = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesBState;
		}

		public void OnUpdate(InputData inputData)
		{
            if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsAttachSomeHandcuffs()) return;
            if (global::ExposureUnnoticed2.ObjectUI.InGame.RingMenu.RingMenuParentView.Instance.IsOpenRing) return;
            var previous = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesAState;
            if (!pca.PlayerState.IsArmAction && !pca.PlayerState.IsEcstasyMotion &&
                pca.PlayerState.EcstasyRemainAftermathTime <= 0f && pca.PlayerState.VibeBikunMotionRemainTime <= 0f)
            {
                if (pca.Mental is null) throw new System.NullReferenceException();
                if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime && !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsRelease((global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)6) && global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesAState != PlayerStateModel.ClothesState.DropClothes)
                {
                    CloseClothes(PlayerStateModel.CoatActionType.TypeA);
                    CloseClothes(PlayerStateModel.CoatActionType.TypeB);
                }
                if (inputData.JustOpenClothesA) OpenClothes(PlayerStateModel.CoatActionType.TypeA);
                else if (inputData.JustCloseClothesA)
                {
                    CloseClothes(PlayerStateModel.CoatActionType.TypeA);
                    CloseClothes(PlayerStateModel.CoatActionType.TypeB);
                }
                else if (inputData.JustOpenClothesB && (int)pca.PlayerState.CurrentAction.Type != 10004)
                    OpenClothes(PlayerStateModel.CoatActionType.TypeB);
            }
            if (previous != global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesAState && global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesAState != PlayerStateModel.ClothesState.DropClothes)
            {
                playerSoundManager.PlaySe((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)3);
                if (playerAnimationManager.TargetArmLayerWeight <= 0f ||
                    (((int)pca.PlayerState.CurrentAction.Type == -1 || !pca.PlayerState.CurrentAction.IsIgnoreClothesArmPulse) && pca.PlayerState.IsCrouch))
                    playerAnimationManager.SetArmPulse();
            }
            playerAnimationManager.TargetClothesStateABlend = (float)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesAState;
            playerAnimationManager.TargetCoatStateBBlend = (float)pca.PlayerState.CurrentClothesBStateCache;
        }

		private void OpenClothes(PlayerStateModel.CoatActionType type)
		{
            if (!IsEnoughSkillLevel(pca.PlayerState.CurrentClothes.ClothesType, type)) return;
            if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime && !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsRelease((global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)6))
            {
                global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("log_cant_expose_by_daytime"), (global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType)0);
                return;
            }
            if (pca.Mental is null) throw new System.NullReferenceException();
            if (pca.PlayerClothes.GetNextState(type) == PlayerStateModel.ClothesState.None) return;
            if (pca.PlayerClothes.GetNextState(type) == PlayerStateModel.ClothesState.DropClothes &&
                ((int)pca.PlayerState.CurrentAction.Type != -1 || pca.PlayerState.IsArmAction)) return;
            if (type == PlayerStateModel.CoatActionType.TypeA)
            {
                if (pca.PlayerClothes.GetNextState(PlayerStateModel.CoatActionType.TypeA) == PlayerStateModel.ClothesState.DropClothes && pca.PlayerState.IsCrouch)
                {
                    global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("log_cant_expose_by_crouch"), (global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType)0);
                    return;
                }
                var targetState = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
                var next = pca.PlayerClothes.GetNextState(PlayerStateModel.CoatActionType.TypeA);
                targetState.PlayerCurrentClothesAState = next;
                if (pca.PlayerState.CurrentClothes.GetAction(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesAState).IsArmActionFromPrev)
                    pca.PlayerState.IsArmAction = true;
            }
            else
            {
                if (type != PlayerStateModel.CoatActionType.TypeB) return;
                var targetState = pca.PlayerState;
                var next = pca.PlayerClothes.GetNextState(PlayerStateModel.CoatActionType.TypeB);
                targetState.CurrentClothesBStateCache = next;
                if (pca.PlayerState.CurrentClothes.GetAction(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesBState).IsArmActionFromPrev)
                    pca.PlayerState.IsArmAction = true;
                return;
            }
            if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesAState == PlayerStateModel.ClothesState.DropClothes)
            {
                var state = pca.PlayerState;
                var action = global::ExposureUnnoticed2.Master.Action.MAction.Get((global::ExposureUnnoticed2.Master.Action.ActionType)10004);
                state.CurrentAction = action;
            }
        }

		private void CloseClothes(PlayerStateModel.CoatActionType type)
		{
            if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesAState == PlayerStateModel.ClothesState.DropClothes) return;
            if (pca.PlayerClothes.GetPrevState(type) == PlayerStateModel.ClothesState.None) return;
            if (type == PlayerStateModel.CoatActionType.TypeA)
            {
                var state = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
                var previous = pca.PlayerClothes.GetPrevState(PlayerStateModel.CoatActionType.TypeA);
                state.PlayerCurrentClothesAState = previous;
                if (pca.PlayerState.CurrentClothes.GetAction(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesAState).IsArmActionFromNext)
                    pca.PlayerState.IsArmAction = true;
            }
            else if (type == PlayerStateModel.CoatActionType.TypeB)
            {
                var state = pca.PlayerState;
                var previous = pca.PlayerClothes.GetPrevState(PlayerStateModel.CoatActionType.TypeB);
                state.CurrentClothesBStateCache = previous;
                if (pca.PlayerState.CurrentClothes.GetAction(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesBState).IsArmActionFromNext)
                    pca.PlayerState.IsArmAction = true;
            }
        }

		private bool IsEnoughSkillLevel(ClothesType clothesType, PlayerStateModel.CoatActionType type)
		{
            if (global::ExposureUnnoticed2.Scripts.InGame.FashionShopManager.IsInTryArea()) return true;
            return pca.PlayerClothes.IsEnoughSkillLevel(type);
        }

		public void OnCompletePickClothes()
		{
            global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesAState = PlayerStateModel.ClothesState.OpenA2;
            playerAnimationManager.ForceSetClothesStateBlend(PlayerStateModel.ClothesState.OpenA2);
        }
	}
}
