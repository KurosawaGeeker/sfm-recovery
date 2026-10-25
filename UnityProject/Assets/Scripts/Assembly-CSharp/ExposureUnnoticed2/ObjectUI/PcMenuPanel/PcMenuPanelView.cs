using Common.Scripts.UI;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.PcMenuPanel
{
	public class PcMenuPanelView : BasePanelView
	{
		[SerializeField]
		private GameObject skillSwitchMask;

		[SerializeField]
		private Button skillSwitchButton;

		[SerializeField]
		private GameObject reinforceInfoObject;

		[SerializeField]
		private TextMeshProUGUI reinforceText;

		[SerializeField]
		private TextMeshProUGUI skillText;

		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		private bool isOpenChild;

		private int currentSelectIndex;

		private void Initialize()
		{
            // Complete original4CA540. Preserve release/rank rereads and the final original slow cancellation.
            skillText.color = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerPassiveSkillActiveDict.Count == 0 ? new Color(0.5f,0.5f,0.5f,1f) : Color.white;
            reinforceText.color = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsRelease((global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)8) ? Color.white : new Color(0.5f,0.5f,0.5f,1f);
            bool available = false;
            if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsRelease((global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)10) && global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ConfirmedReinforceRank < 10) available = true;
            else if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsRelease((global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)9) && global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ConfirmedReinforceRank < 9) available = true;
            else if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsRelease((global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)8) && global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ConfirmedReinforceRank < 8) available = true;
            reinforceInfoObject.SetActive(available);
            global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49);
            global::ExposureUnnoticed2.Scripts.InGame.SlowAssistManager.Instance.CancelSlow();
            ToPadModeIfNeed(true);
        }

		private void ToPadModeIfNeed(bool isForce = false)
		{
            // The original PC branch deliberately returns when already active and not forced.
            if (IsActivePanel() && !isForce) return;
            if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsGamePad) buttonGroupManager.ForceSelect(currentSelectIndex);
            else buttonGroupManager.ForceSelect((ButtonGroupManager.IButtonView)null);
        }

		public override void OnReturnActive()
		{
            ToPadModeIfNeed(true);
        }

		private void LateUpdate()
		{
            OnUpdate();
            if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsJustToGamePad) ToPadModeIfNeed(false);
            if (IsActivePanel() && global::ExposureUnnoticed2.Scripts.Base.InputManager.IsGamePad && !isOpenChild)
            {
                int move = 0;
                if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1001) || global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownLongRepeat((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1001,20,5)) move = -1;
                if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1002) || global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownLongRepeat((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1002,20,5)) ++move;
                if (move != 0)
                {
                    global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)48);
                    currentSelectIndex = unchecked(buttonGroupManager.ButtonCount + currentSelectIndex + move) % buttonGroupManager.ButtonCount;
                    buttonGroupManager.ForceSelect(currentSelectIndex);
                }
                if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1000)) buttonGroupManager.InvokeOnClick(currentSelectIndex);
            }
            if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)10) && IsActivePanel() && !isOpenChild)
            {
                global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)50);
                Close();
            }
        }

		public void OnClickClose()
		{
            global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)50);
            Close();
        }

		public void OnClickManualSave()
		{
            // Complete original RVA 0x4CAE80: save current slot, then one-button completion popup.
            global::ExposureUnnoticed2.Scripts.InGame.SaveManager.Save(
                global::ExposureUnnoticed2.Scripts.Base.SaveRapper.SaveDataType.ManualSlot, -1, false);
            global::ExposureUnnoticed2.ObjectUI.CommonPopup.CommonPopupView.Open(
                global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_common_confirm"),
                global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_save_complete"),
                new global::ExposureUnnoticed2.ObjectUI.CommonPopup.CommonButtonData
                {
                    SeType = (global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)0,
                    Text = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_common_ok"),
                    OnClick = () => { }
                });
		}

		public void OnClickSwitchSkill()
		{
		}

		public void OnClickFpsRestriction()
		{
		}

		public void OnClickVibeReinforce()
		{
		}

		public void OnClickDroneReinforce()
		{
		}

		public void OnClickSelectSexualityType()
		{
		}

		public void OnClickOnlineShop()
		{
            global::ExposureUnnoticed2.ObjectUI.PcMenuPanel.OnlineShopPanel.OnlineShopPanelView.Open();
		}

		public void OnClickReinforce()
		{
		}

		public void OnClickPurchaseHair()
		{
		}

		public void OnClickDifficulty()
		{
		}

		public void OnClickCollectAll()
		{
		}

		public static void Open()
		{
            var panel = global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.Open(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.PcMenuPanel,false) as PcMenuPanelView;
            panel.Initialize();
        }
	}
}
