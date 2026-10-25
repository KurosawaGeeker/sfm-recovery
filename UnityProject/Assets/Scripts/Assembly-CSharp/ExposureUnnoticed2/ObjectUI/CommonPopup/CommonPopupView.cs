using System;
using Common.Scripts.UI;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.CommonPopup
{
	public class CommonPopupView : BasePanelView
	{
		[SerializeField]
		private GameObject oneButtonParent;

		[SerializeField]
		private GameObject twoButtonParent;

		[SerializeField]
		private GameObject threeButtonParent;

		[SerializeField]
		private TextMeshProUGUI titleText;

		[SerializeField]
		private TextMeshProUGUI descriptionText;

		[SerializeField]
		private CommonPopupButtonView oneButton;

		[SerializeField]
		private CommonPopupButtonView twoLeftButton;

		[SerializeField]
		private CommonPopupButtonView twoRightButton;

		[SerializeField]
		private CommonPopupButtonView threeLeftButton;

		[SerializeField]
		private CommonPopupButtonView threeMiddleButton;

		[SerializeField]
		private CommonPopupButtonView threeRightButton;

		private void Update()
		{
			// Recovered from GameAssembly.dll RVA 0x577EE0
			if (!IsActivePanel()) return;
			if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.Cancel))
			{
			    if (oneButtonParent.activeInHierarchy) OnClickOne();
			    else if (twoButtonParent.activeInHierarchy) OnClickTwoLeft();
			    else if (threeButtonParent.activeInHierarchy)
			    {
			        global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData(twoLeftButton.SeType, -1f, false);
			        threeLeftButton.OnClick?.Invoke();
			        Close();
			    }
			}
			else if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.UIDecide))
			{
			    if (oneButtonParent.activeInHierarchy) OnClickOne();
			    else if (twoButtonParent.activeInHierarchy) OnClickTwoRight();
			    else if (threeButtonParent.activeInHierarchy)
			    {
			        global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData(twoLeftButton.SeType, -1f, false);
			        threeRightButton.OnClick?.Invoke();
			        Close();
			    }
			}
			else if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.UIDecideB))
			{
			    if (oneButtonParent.activeInHierarchy || twoButtonParent.activeInHierarchy) return;
			    if (!threeButtonParent.activeInHierarchy) return;
			    global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData(twoLeftButton.SeType, -1f, false);
			    threeMiddleButton.OnClick?.Invoke();
			    Close();
			}
		}

		private void Initialize(string title, string description, CommonButtonData one)
		{
			// Recovered from GameAssembly.dll RVA 0x5783C0
			CommonInitialize(title, description);
			oneButton.UpdateView(one);
			twoButtonParent.SetActive(false);
			threeButtonParent.SetActive(false);
		}

		private void Initialize(string title, string description, Action one)
		{
			// Recovered from GameAssembly.dll RVA 0x5784B0
			CommonInitialize(title, description);
			oneButton.OnClick = one;
			twoButtonParent.SetActive(false);
			threeButtonParent.SetActive(false);
		}

		private void Initialize(string title, string description, Action left, Action right)
		{
			// Recovered from GameAssembly.dll RVA 0x5785D0
			CommonInitialize(title, description);
			twoLeftButton.OnClick = left;
			twoRightButton.OnClick = right;
			oneButtonParent.SetActive(false);
			threeButtonParent.SetActive(false);
		}

		private void Initialize(string title, string description, CommonButtonData left, CommonButtonData right)
		{
			// Recovered from GameAssembly.dll RVA 0x578760
			CommonInitialize(title, description);
			twoLeftButton.UpdateView(left);
			twoRightButton.UpdateView(right);
			oneButtonParent.SetActive(false);
			threeButtonParent.SetActive(false);
		}

		private void Initialize(string title, string description, CommonButtonData left, CommonButtonData middle, CommonButtonData right)
		{
			// Recovered from GameAssembly.dll RVA 0x578890
			CommonInitialize(title, description);
			threeLeftButton.UpdateView(left);
			threeMiddleButton.UpdateView(middle);
			threeRightButton.UpdateView(right);
			oneButtonParent.SetActive(false);
			twoButtonParent.SetActive(false);
		}

		private void CommonInitialize(string title, string description)
		{
			// Recovered from GameAssembly.dll RVA 0x5789F0
			titleText.text = title;
			descriptionText.text = description;
			oneButton.SeType = (global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49;
			twoLeftButton.SeType = (global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)50;
			twoRightButton.SeType = (global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49;
			threeLeftButton.SeType = (global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)50;
			threeMiddleButton.SeType = (global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49;
			threeRightButton.SeType = (global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49;
		}

		public void OnClickOne()
		{
			// Recovered from GameAssembly.dll RVA 0x578AD0
			global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData(oneButton.SeType, -1f, false);
			oneButton.OnClick?.Invoke();
			Close();
		}

		public void OnClickTwoLeft()
		{
			// Recovered from GameAssembly.dll RVA 0x578BA0
			global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData(twoLeftButton.SeType, -1f, false);
			twoLeftButton.OnClick?.Invoke();
			Close();
		}

		public void OnClickTwoRight()
		{
			// Recovered from GameAssembly.dll RVA 0x578C70
			global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData(twoRightButton.SeType, -1f, false);
			twoRightButton.OnClick?.Invoke();
			Close();
		}

		public void OnClickThreeLeft()
		{
			// Recovered from GameAssembly.dll RVA 0x578D40
			global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData(twoLeftButton.SeType, -1f, false);
			threeLeftButton.OnClick?.Invoke();
			Close();
		}

		public void OnClickThreeMiddle()
		{
			// Recovered from GameAssembly.dll RVA 0x578E10
			global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData(twoLeftButton.SeType, -1f, false);
			threeMiddleButton.OnClick?.Invoke();
			Close();
		}

		public void OnClickThreeRight()
		{
			// Recovered from GameAssembly.dll RVA 0x578EE0
			global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData(twoLeftButton.SeType, -1f, false);
			threeRightButton.OnClick?.Invoke();
			Close();
		}

		public static void Open(string title, string description, CommonButtonData one)
		{
			// Recovered from GameAssembly.dll RVA 0x578FB0
			(global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.Open(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.CommonPopup, false) as CommonPopupView).Initialize(title, description, one);
		}

		public static void Open(string title, string description, Action one)
		{
			// Recovered from GameAssembly.dll RVA 0x5791C0
			(global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.Open(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.CommonPopup, false) as CommonPopupView).Initialize(title, description, one);
		}

		public static void Open(string title, string description, Action left, Action right)
		{
			// Recovered from GameAssembly.dll RVA 0x579400
			(global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.Open(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.CommonPopup, false) as CommonPopupView).Initialize(title, description, left, right);
		}

		public static void Open(string title, string description, CommonButtonData left, CommonButtonData right, bool isFront = false)
		{
			// Recovered from GameAssembly.dll RVA 0x5796B0
			(global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.Open(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.CommonPopup, isFront) as CommonPopupView).Initialize(title, description, left, right);
		}

		public static void Open(Transform parent, string title, string description, CommonButtonData left, CommonButtonData right)
		{
			// Recovered from GameAssembly.dll RVA 0x579850
			BasePanelView panel = UnityEngine.Object.Instantiate(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.CommonPopup, parent);
			panel.GetComponent<CommonPopupView>().Initialize(title, description, left, right);
		}

		public static void Open(Transform parent, string title, string description, CommonButtonData left, CommonButtonData middle, CommonButtonData right)
		{
			// Recovered from GameAssembly.dll RVA 0x5799B0
			(global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.Open(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.CommonPopup, true) as CommonPopupView).Initialize(title, description, left, middle, right);
		}
	}
}
