using System;
using Common.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.NameEditPanel
{
	public class NameEditPanelView : BasePanelView
	{
		[SerializeField]
		private new Text name;

		[SerializeField]
		private InputField inputField;

		private Action<string> onClickOk;

		private void Initialize(string initText, Action<string> onClickOk)
		{
            this.onClickOk = onClickOk;
            var text = name;
            inputField.text = initText;
            text.text = initText;
        }

		private void LateUpdate()
		{
            // Preserve native active-pad path without a second active read after a submit callback.
            if (IsActivePanel() && global::ExposureUnnoticed2.Scripts.Base.InputManager.IsGamePad)
            {
                if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1000))
                {
                    global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49);
                    onClickOk?.Invoke(name.text);
                    Close();
                }
                if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)10))
                {
                    global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)50);
                    Close();
                }
            }
            else if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)10) && IsActivePanel())
            {
                global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)50);
                Close();
            }
        }

		public void OnClickCancel()
		{
            Close();
        }

		public void OnClickOk()
		{
            onClickOk?.Invoke(name.text);
            Close();
        }

		public static void Open(string initText, Action<string> onClickOk)
		{
            var panel = global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.Open(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.NameEditPanel,false) as NameEditPanelView;
            panel.Initialize(initText,onClickOk);
        }
	}
}
