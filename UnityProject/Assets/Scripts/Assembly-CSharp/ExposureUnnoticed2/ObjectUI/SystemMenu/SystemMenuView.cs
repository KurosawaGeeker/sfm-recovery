using Common.Scripts.UI;
using HadakaCoat.ObjectsUi.Common.Button;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.SystemMenu
{
	public class SystemMenuView : BasePanelView
	{
		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		private bool isFirstFrame;

		private int currentSelectIndex;

		public static SystemMenuView Instance { get; private set; }

		private void Initialize()
		{
// Complete original RVA 0x4BFF40; isolated raw/typed proof required.
GetComponent<CanvasGroup>().alpha = 0f;
isFirstFrame = true;
Instance = this;
global::ExposureUnnoticed2.Scripts.Base.TimeScaleManager.Instance.SwitchMode(global::ExposureUnnoticed2.Scripts.Base.TimeScaleManager.TimeScaleMode.OpenPause, true);
global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.GameStateApplier.OnLateUpdate(true);
global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49, -1f, false);
ToPadModeIfNeed(true);

}

		private void ToPadModeIfNeed(bool isForce = false)
		{
// Complete original RVA 0x4C00B0; isolated raw/typed proof required.
if (!IsActivePanel() && !isForce) return;
if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsGamePad)
    buttonGroupManager.ForceSelect(currentSelectIndex);
else
    buttonGroupManager.ForceSelect((ButtonGroupManager.IButtonView)null);

}

		public override void OnReturnActive()
		{
// Complete original RVA 0x4C0170; isolated raw/typed proof required.
ToPadModeIfNeed(false);

}

		private void LateUpdate()
		{
// Complete original RVA 0x4C0180; isolated raw/typed proof required.
OnUpdate();
if (!isFirstFrame && ForceActiveButtonMode != ForceActiveButtonType.NonActive)
{
    if (IsActivePanelPadMode())
    {
        if (global::ExposureUnnoticed2.Scripts.Base.InputManager.JustChangeIsGamePad) ToPadModeIfNeed(false);
        int direction = 0;
        if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.UIUp)
            || global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownLongRepeat(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.UIUp, 20, 5)) direction = -1;
        if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.UIDown)
            || global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownLongRepeat(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.UIDown, 20, 5)) ++direction;
        if (direction != 0)
        {
            global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)48, -1f, false);
            currentSelectIndex = unchecked(buttonGroupManager.ButtonCount + currentSelectIndex + direction) % buttonGroupManager.ButtonCount;
            buttonGroupManager.ForceSelect(currentSelectIndex);
        }
        if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.UIDecide))
            buttonGroupManager.InvokeOnClick(currentSelectIndex);
        if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.Cancel)) OnClickResume();
    }
    else if (IsActivePanel())
    {
        if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.Cancel)) OnClickResume();
    }
}
isFirstFrame = false;

}

		public void OnClickResume()
		{
// Complete original RVA 0x4C0440; isolated raw/typed proof required.
global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)50, -1f, false);
global::ExposureUnnoticed2.Scripts.Base.TimeScaleManager.Instance.SwitchMode(global::ExposureUnnoticed2.Scripts.Base.TimeScaleManager.TimeScaleMode.OpenPause, false);
Instance = null;
Close();

}

		public void OnClickOption()
		{
		}

		public void OnClickGraphicsOption()
		{
		}

		public void OnClickDifficulty()
		{
		}

		public void OnClickEmergencyEscape()
		{
		}

		public void OnClickHelp()
		{
		}

		public void OnClickLoad()
		{
		}

		public void OnClickSaveAndGoToTitle()
		{
		}

		public static void Open()
		{
            global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayFlame(() =>
            {
                var view = global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.Open(
                    global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.SystemMenu, true) as SystemMenuView;
                if (view is null) throw new global::System.NullReferenceException();
                view.GetComponent<CanvasGroup>().alpha = 0f;
                view.isFirstFrame = true;
                Instance = view;
                global::ExposureUnnoticed2.Scripts.Base.TimeScaleManager.Instance.SwitchMode(global::ExposureUnnoticed2.Scripts.Base.TimeScaleManager.TimeScaleMode.OpenPause, true);
                global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.GameStateApplier.OnLateUpdate(true);
                global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49);
                view.ToPadModeIfNeed(true);
            }, 1);
		}
	}
}
