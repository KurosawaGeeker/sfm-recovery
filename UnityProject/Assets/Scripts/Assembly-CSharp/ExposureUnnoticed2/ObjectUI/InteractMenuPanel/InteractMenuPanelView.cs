using Common.Scripts.UI;
using ExposureUnnoticed2.Scripts.Base;
using HadakaCoat.ObjectsUi.Common.Button;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InteractMenuPanel
{
	public class InteractMenuPanelView : BasePanelView
	{
		[SerializeField]
		private InteractMenuItemView[] buttons;

		[SerializeField]
		private RectTransform background;

		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		[SerializeField]
		private GameObject interactLabel;

		private InteractMenuItemModel[] itemModels;

		private bool isFirstFrame = true;

		private int currentSelectIndex;

		private bool isClosed;

		public static InteractMenuPanelView Instance { get; private set; }

		private void Initialize()
		{
			// Recovered from GameAssembly.dll RVA 0x4F70E0
			var capturedBackground = background;
			int count = InteractManager.CurrentInteractModel.Controller.MenuAction.Count;
			ExternalMethodManager.SetHeight(capturedBackground, (float)unchecked(count * 50 + 14));
			if (InteractManager.CurrentInteractModel.Controller.MenuAction.Count > 6)
			    background.sizeDelta = new Vector2(475f, 314f);
			for (int i = 0; i < buttons.Length; i++)
			{
			    if (i < InteractManager.CurrentInteractModel.Controller.MenuAction.Count)
			    {
			        var button = buttons[i];
			        string nameKey = InteractManager.CurrentInteractModel.Controller.MenuAction[i].NameKey;
			        button.Initialize(nameKey);
			    }
			    var buttonObject = buttons[i].gameObject;
			    bool active = i < InteractManager.CurrentInteractModel.Controller.MenuAction.Count;
			    buttonObject.SetActive(active);
			}
			Instance = this;
			// Original cached callback RVA0x4F8510.
			onClose = () => Instance = null;
			buttonGroupManager.ReSearchButton(false, false);
			EventManager.AddListener<StartTransitionStageEvent>(OnTransitionStageEvent, false);
			Scheduler.DelayFlame(ToPadModeIfNeed, 1);
		}

		private void OnDestroy()
		{
			// Recovered from GameAssembly.dll RVA 0x4F77D0
			EventManager.RemoveListener<StartTransitionStageEvent>(OnTransitionStageEvent);
		}

		private void Initialize(InteractMenuItemModel[] models)
		{
			// Recovered from GameAssembly.dll RVA 0x4F7890
			Instance = this;
			itemModels = models;
			var capturedBackground = background;
			ExternalMethodManager.SetHeight(capturedBackground, (float)unchecked(models.Length * 50 + 14));
			for (int i = 0; i < buttons.Length; i++)
			{
			    if (i < models.Length)
			    {
			        var button = buttons[i];
			        string nameKey = models[i].NameKey;
			        button.Initialize(nameKey);
			    }
			    var buttonObject = buttons[i].gameObject;
			    buttonObject.SetActive(i < models.Length);
			}
		}

		private void ToPadModeIfNeed()
		{
			// Recovered from GameAssembly.dll RVA 0x4F7A30
			if (InputManager.IsGamePad) buttonGroupManager.ForceSelect(currentSelectIndex);
		}

		private void LateUpdate()
		{
			// Recovered from GameAssembly.dll RVA 0x4F7B00
			if (IsActivePanel() && InputManager.IsGamePad)
			{
			    if (InputManager.IsJustToGamePad) ToPadModeIfNeed();
			    bool up = InputManager.IsDownFrame(InputManager.InputType.UIUp);
			    bool down = InputManager.IsDownFrame(InputManager.InputType.UIDown);
			    int move = (down ? 1 : 0) - (up ? 1 : 0);
			    if (buttonGroupManager.ButtonCount > 6)
			    {
			        if (InputManager.IsDownFrame(InputManager.InputType.UILeft)) move = unchecked(move - 6);
			        if (InputManager.IsDownFrame(InputManager.InputType.UIRight)) move = unchecked(move + 6);
			    }
			    if (move != 0)
			    {
			        global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)48, -1f, false);
			        int numerator = unchecked(buttonGroupManager.ButtonCount + currentSelectIndex + move);
			        var selectedGroup = buttonGroupManager;
			        currentSelectIndex = numerator % selectedGroup.ButtonCount;
			        selectedGroup.ForceSelect(currentSelectIndex);
			    }
			    if (InputManager.IsDownFrame(InputManager.InputType.UIDecide))
			        buttonGroupManager.InvokeOnClick(currentSelectIndex);
			    if (InputManager.IsDownFrame(InputManager.InputType.Cancel)) Close();
			}
			else
			{
			    if (InputManager.IsDownFrame(InputManager.InputType.Cancel)) Close();
			    if (InputManager.IsDownFrame(InputManager.InputType.Interact) && !isFirstFrame) OnClick(0);
			}
			var capturedLabel = interactLabel;
			capturedLabel.SetActive(!InputManager.IsGamePad);
			isFirstFrame = false;
		}

		public void OnClick(int index)
		{
			// Recovered from GameAssembly.dll RVA 0x4F7F90
			if (isClosed) return;
			if (!InteractManager.CurrentInteractModel.Controller.MenuAction.ContainsKey(index))
			    throw new System.Exception("MenuActionに" + index + " keyがありません");
			// Original captured callback RVA0x4F8520; it reads the live itemModels at execution.
			Scheduler.DelayFlame(() =>
			{
			    if (itemModels is null)
			        InteractManager.CurrentInteractModel.Controller.MenuAction[index].OnClick();
			    else
			        itemModels[index].OnClick();
			}, 2);
			Close();
		}

		public void OnClickEmpty()
		{
			// Recovered from GameAssembly.dll RVA 0x4BFA40
			Close();
		}

		public static void Open()
		{
			// Recovered from GameAssembly.dll RVA 0x4F81D0
			// Original class-init precedes asset/UI instance validation.
			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager).TypeHandle);
			var assets = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
			if (assets is null) throw new System.NullReferenceException();
			var ui = global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance;
			if (ui is null) throw new System.NullReferenceException();
			var panel = ui.Open(assets.InteractMenuPanel, false) as InteractMenuPanelView;
			panel.Initialize();
		}

		public static void Open(InteractMenuItemModel[] models)
		{
			// Recovered from GameAssembly.dll RVA 0x4F82D0
			// Original class-init precedes asset/UI instance validation.
			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager).TypeHandle);
			var assets = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
			if (assets is null) throw new System.NullReferenceException();
			var ui = global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance;
			if (ui is null) throw new System.NullReferenceException();
			var panel = ui.Open(assets.InteractMenuPanel, false) as InteractMenuPanelView;
			panel.Initialize(models);
		}

		private void OnTransitionStageEvent(StartTransitionStageEvent startTransitionStageEvent)
		{
			// Recovered from GameAssembly.dll RVA 0x4BFA40
			Close();
		}
	}
}
