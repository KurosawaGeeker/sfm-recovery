using System;
using Common.Scripts.UI;
using ExposureUnnoticed2.ObjectUI.OptionMenu;
using HadakaCoat.ObjectsUi.Common.Button;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.SelectSexOptionPanel
{
	public class SelectSexOptionPanelView : BasePanelView
	{
		[SerializeField]
		private CustomizeCheckItemView futanariView;

		[SerializeField]
		private CustomizeCheckItemView peeView;

		[SerializeField]
		private CustomizeCheckItemView sexView;

		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		private Action onDecides;

		private Action onCancel;

		private int currentSelectIndex;

		private void Initialize(Action onDecides, Action onCancel)
		{
			// Recovered from GameAssembly.dll RVA 0x4C4A70
			this.onDecides = onDecides;
			this.onCancel = onCancel;
			// Original option callbacks RVA 0x4C57C0 / 0x4C5820 / 0x4C5880.
			futanariView.InitializeForPlayerCustomize(true, value => global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.FutanariDisable = !value);
			peeView.InitializeForPlayerCustomize(true, value => global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.PeeDisable = !value);
			sexView.InitializeForPlayerCustomize(false, value => global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.SexDisable = !value);
			ToPadModeIfNeed();
		}

		private void ToPadModeIfNeed()
		{
			// Recovered from GameAssembly.dll RVA 0x4C4EE0
			if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsGamePad)
			    buttonGroupManager.ForceSelect(buttonGroupManager.Get(currentSelectIndex));
			else
			    buttonGroupManager.ForceSelect((HadakaCoat.ObjectsUi.Common.Button.ButtonGroupManager.IButtonView)null);
		}

		public void OnClickDecide()
		{
			// Recovered from GameAssembly.dll RVA 0x4C4FC0
			global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.AnsweredFirstSexOption = true;
			// Original continuation RVA 0x4C56F0, deliberately preserves null delegate failure.
			global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayFlame(() => onDecides(), 1);
			Close();
		}

		private void Update()
		{
			// Recovered from GameAssembly.dll RVA 0x4C5080
			if (global::ExposureUnnoticed2.Scripts.Base.InputManager.JustChangeIsGamePad) ToPadModeIfNeed();
			if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsGamePad)
			{
			    int direction = 0;
			    if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.UIUp)
			        || global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownLongRepeat(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.UIUp, 20, 5)) direction = -1;
			    if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.UIDown)
			        || global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownLongRepeat(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.UIDown, 20, 5)) ++direction;
			    if (direction != 0)
			    {
			        global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)48, -1f, false);
			        currentSelectIndex = (buttonGroupManager.ButtonCount + currentSelectIndex + direction) % buttonGroupManager.ButtonCount;
			        buttonGroupManager.ForceSelect(buttonGroupManager.Get(currentSelectIndex));
			    }
			    if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.UIDecide)) buttonGroupManager.InvokeOnClick(currentSelectIndex);
			    if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.UIDecideB))
			    {
			        global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49, -1f, false);
			        OnClickDecide();
			    }
			}
			if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.Cancel)) OnClickCancel();
		}

		public void OnClickCancel()
		{
			// Recovered from GameAssembly.dll RVA 0x4C54E0
			global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)50);
			onCancel?.Invoke();
			Close();
		}

		public static void Open(Transform parent, Action onDecides, Action onCancel)
		{
			// Recovered from GameAssembly.dll RVA 0x4C5570
			Common.Scripts.UI.BasePanelView obj = UnityEngine.Object.Instantiate(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.SelectSexOptionPanel, parent);
			obj.GetComponent<SelectSexOptionPanelView>().Initialize(onDecides, onCancel);
		}
	}
}
