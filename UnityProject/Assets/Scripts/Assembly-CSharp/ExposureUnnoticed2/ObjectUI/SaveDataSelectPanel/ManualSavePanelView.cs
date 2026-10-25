using System;
using System.Collections.Generic;
using Common.CommonUI.CommonScrollView;
using Common.Scripts.UI;
using ExposureUnnoticed2.ObjectUI.SelectDifficultyPanel;
using ExposureUnnoticed2.Scripts.Base;
using HadakaCoat.ObjectsUi.Common.Button;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.SaveDataSelectPanel
{
	public class ManualSavePanelView : BasePanelView
	{
		[SerializeField]
		private GameObject itemPrefab;

		[SerializeField]
		private Transform itemParent;

		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		[SerializeField]
		private CommonScrollRectView commonScrollRectView;

		private DifficultType difficultType;

		private Action onCancel;

		private int currentSelectIndex;

		private bool isOpenChild;

		private List<SaveSlotItemView> childViewList = new List<SaveSlotItemView>();

		private void Initialize(DifficultType difficultType, Action onCancel)
		{
			// Recovered from GameAssembly.dll RVA 0x4E2FE0
			this.difficultType = difficultType;
			this.onCancel = onCancel;
			RefreshView();
		}

		private void ToPadModeIfNeed()
		{
			// Recovered from GameAssembly.dll RVA 0x4E3040
			if (!isOpenChild && InputManager.IsGamePad)
			    buttonGroupManager.ForceSelect(buttonGroupManager.Get(currentSelectIndex));
		}

		private void OnClickSlot(SaveRapper.SaveDataType type, int slot)
		{
			// Recovered from GameAssembly.dll RVA 0x4E30E0
			void OnSelect()
			{
			    // Original closure routine RVA 0x4E4240.
			    var oldOptions = global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData;
			    global::ExposureUnnoticed2.Scripts.InGame.SaveManager.CurrentSaveSlot = slot;
			    global::ExposureUnnoticed2.Scripts.InGame.SaveManager.Load(SaveRapper.SaveDataType.NewGame, global::ExposureUnnoticed2.Scripts.InGame.SaveManager.CurrentSaveSlot);
			    global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData = oldOptions;
			    global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DifficultType = difficultType;
			    global::ExposureUnnoticed2.ObjectUI.FadeOutPanel.FadePanelView.Open(new global::ExposureUnnoticed2.ObjectUI.FadeOutPanel.WaitSceneLoadTwiceStarter(),
			        () => UnityEngine.SceneManagement.SceneManager.LoadScene("SpringboardLoadEmpty"),
			        null,
			        () => global::ExposureUnnoticed2.ObjectUI.TutorialPanel.TutorialPanelView.Open(global::ExposureUnnoticed2.ObjectUI.TutorialPanel.TutorialPanelView.TutorialOpenKind.FirstPlay),
			        true);
			}
			global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49, -1f, false);
			if (!global::ExposureUnnoticed2.Scripts.InGame.SaveManager.IsExistSaveData(type, slot))
			{
			    OnSelect();
			    return;
			}
			isOpenChild = true;
			Transform parent = transform;
			string title = Localizer.Trans("ui_common_confirm");
			string description = Localizer.Trans("ui_save_update_confirm");
			var no = new global::ExposureUnnoticed2.ObjectUI.CommonPopup.CommonButtonData
			{
			    SeType = (global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)0,
			    Text = Localizer.Trans("ui_common_no"),
			    // Original cancellation callbacks RVA 0x4E4610 -> 0x4E46F0.
			    OnClick = () => Scheduler.DelayFlame(() => isOpenChild = false, 1)
			};
			var yes = new global::ExposureUnnoticed2.ObjectUI.CommonPopup.CommonButtonData
			{
			    SeType = (global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)0,
			    Text = Localizer.Trans("ui_common_yes"),
			    OnClick = () =>
			    {
			        // Original affirmative callback RVA 0x4E4710: synchronous.
			        SaveRapper.PreProcess(false, slot, false);
			        ES3.DeleteFile(SaveRapper.GetSaveDataPath());
			        OnSelect();
			    }
			};
			global::ExposureUnnoticed2.ObjectUI.CommonPopup.CommonPopupView.Open(parent, title, description, no, yes);
		}

		private void RefreshView()
		{
			// Recovered from GameAssembly.dll RVA 0x4E3560
			ExternalMethodManager.DestroyChildren(itemParent);
			int count = SystemManager.IsTrial ? 1 : 20;
			for (int slot = 0; slot < count; ++slot)
			{
			    GameObject item = UnityEngine.Object.Instantiate(itemPrefab, itemParent);
			    SaveSlotItemView view = item.GetComponent<SaveSlotItemView>();
			    view.Initialize(SaveRapper.SaveDataType.ManualSlot, OnClickSlot, slot, true);
			    childViewList.Add(view);
			}
			buttonGroupManager.ReSearchButton(false, false);
			Scheduler.DelayFlame(ToPadModeIfNeed, 1);
		}

		private void LateUpdate()
		{
			// Recovered from GameAssembly.dll RVA 0x4E3940
			if (InputManager.JustChangeIsGamePad) ToPadModeIfNeed();
			if (InputManager.IsGamePad && !isOpenChild)
			{
			    int direction = 0;
			    if (InputManager.IsDownFrame(InputManager.InputType.UIUp)
			        || InputManager.IsDownLongRepeat(InputManager.InputType.UIUp, 20, 5)) direction = -1;
			    if (InputManager.IsDownFrame(InputManager.InputType.UIDown)
			        || InputManager.IsDownLongRepeat(InputManager.InputType.UIDown, 20, 5)) ++direction;
			    if (direction != 0)
			    {
			        childViewList[currentSelectIndex].ForceSetEmit(0);
			        global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)48, -1f, false);
			        currentSelectIndex = (buttonGroupManager.ButtonCount + currentSelectIndex + direction) % buttonGroupManager.ButtonCount;
			        childViewList[currentSelectIndex].ForceSetEmit(1);
			        buttonGroupManager.ForceSelect(buttonGroupManager.Get(currentSelectIndex));
			        commonScrollRectView.ScrollToCentering(childViewList[currentSelectIndex].gameObject);
			    }
			    if (InputManager.IsDownFrame(InputManager.InputType.UIDecide)) buttonGroupManager.InvokeOnClick(currentSelectIndex);
			}
			if (InputManager.IsDownFrame(InputManager.InputType.Cancel) && !isOpenChild)
			{
			    global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)50, -1f, false);
			    OnClickClose();
			}
		}

		public void OnClickClose()
		{
			// Recovered from GameAssembly.dll RVA 0x4E3E30
			onCancel();
			Close();
		}

		public static void Open(Transform parent, DifficultType difficultType, Action onCancel)
		{
			// Recovered from GameAssembly.dll RVA 0x4E3E70
			BasePanelView panel = UnityEngine.Object.Instantiate(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.ManualSavePanel, parent);
			panel.GetComponent<ManualSavePanelView>().Initialize(difficultType, onCancel);
		}
	}
}
