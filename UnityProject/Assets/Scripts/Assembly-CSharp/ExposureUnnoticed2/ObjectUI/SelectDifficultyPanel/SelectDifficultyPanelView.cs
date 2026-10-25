using System;
using Common.Scripts.UI;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.SelectDifficultyPanel
{
	public class SelectDifficultyPanelView : BasePanelView
	{
		[SerializeField]
		private TextMeshProUGUI descriptionText;

		[SerializeField]
		private ButtonView decideButton;

		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		private DifficultType currentSelect;

		private bool isFromSystemMenu;

		private int currentSelectIndex;

		private Action onCancel;

		private void Initialize(bool isFromSystemMenu, Action onClose, Action onCancel)
		{
			// Recovered from GameAssembly.dll RVA 0x4C58E0
			this.onClose = onClose;
			this.onCancel = onCancel;
			this.isFromSystemMenu = isFromSystemMenu;
			decideButton.SetMask(true);
			currentSelect = isFromSystemMenu ? global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DifficultType : DifficultType.Original;
			currentSelectIndex = currentSelect switch
			{
			    DifficultType.Casual => 2,
			    DifficultType.Easy => 1,
			    DifficultType.Original => 0,
			    _ => throw new System.Runtime.CompilerServices.SwitchExpressionException(currentSelect)
			};
			buttonGroupManager.ForceSelect(buttonGroupManager.Get(currentSelectIndex));
			RefreshView();
			ToPadModeIfNeed();
		}

		private void ToPadModeIfNeed()
		{
			// Recovered from GameAssembly.dll RVA 0x4C5AD0
			if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsGamePad)
			    buttonGroupManager.ForceSelect(buttonGroupManager.Get(currentSelectIndex));
		}

		private void RefreshView()
		{
			// Recovered from GameAssembly.dll RVA 0x4C5BA0
			string key = currentSelect switch
			{
			    DifficultType.Casual => "ui_difficulty_description_casual",
			    DifficultType.Easy => "ui_difficulty_description_easy2",
			    DifficultType.Original => "ui_difficulty_description_original",
			    _ => throw new System.Runtime.CompilerServices.SwitchExpressionException(currentSelect)
			};
			descriptionText.text = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(key);
			decideButton.SetMask(false);
		}

		public void OnClickItem(int index)
		{
			// Recovered from GameAssembly.dll RVA 0x4C5CA0
			currentSelect = (DifficultType)index;
			RefreshView();
		}

		public void OnClickDecide()
		{
			// Recovered from GameAssembly.dll RVA 0x4C5CB0
			if (!isFromSystemMenu)
			    global::ExposureUnnoticed2.ObjectUI.SaveDataSelectPanel.ManualSavePanelView.Open(transform.parent, currentSelect, onCancel);
			else
			    global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DifficultType = currentSelect;
			Close();
		}

		private void LateUpdate()
		{
		}

		public void OnClickCancel()
		{
			// Recovered from GameAssembly.dll RVA 0x4C6230
			global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)50);
			onCancel?.Invoke();
			Close();
		}

		public static void Open(Transform parent, Action onClose = null, bool isFromSystemMenu = false, Action onCancel = null)
		{
			// Recovered from GameAssembly.dll RVA 0x4C62C0
			BasePanelView obj = UnityEngine.Object.Instantiate(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.SelectDifficultyPanel, parent);
			obj.GetComponent<SelectDifficultyPanelView>().Initialize(isFromSystemMenu, onClose, onCancel);
		}
	}
}
