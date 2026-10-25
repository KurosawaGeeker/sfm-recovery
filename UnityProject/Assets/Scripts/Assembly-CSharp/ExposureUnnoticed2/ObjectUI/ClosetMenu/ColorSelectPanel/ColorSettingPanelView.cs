using System;
using Common.Scripts.UI;
using ExposureUnnoticed2.ObjectUI.OptionMenu;
using Ricimi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.ClosetMenu.ColorSelectPanel
{
	public class ColorSettingPanelView : BasePanelView
	{
		[SerializeField]
		private TextMeshProUGUI titleText;

		[SerializeField]
		private Image color;

		[SerializeField]
		private GameObject rgbParent;

		[SerializeField]
		private GameObject hsvParent;

		[SerializeField]
		private CustomizeBarItemView rBar;

		[SerializeField]
		private CustomizeBarItemView gBar;

		[SerializeField]
		private CustomizeBarItemView bBar;

		[SerializeField]
		private CustomizeBarItemView hBar;

		[SerializeField]
		private CustomizeBarItemView sBar;

		[SerializeField]
		private CustomizeBarItemView vBar;

		[SerializeField]
		private CustomizeCheckItemView isHsv;

		[SerializeField]
		private Ricimi.Gradient sGradient;

		private Action<int, int, int> onChange;

		private int currentR;

		private int currentG;

		private int currentB;

		private float currentH;

		private float currentS;

		private float currentV;

		private void Initialize(string title, int r, int g, int b, Action<int, int, int> onChange, Action onClose = null)
		{
		}

		private void Refresh()
		{
		}

		private void InitializeRGBBar()
		{
		}

		private void InitializeHSVBar()
		{
		}

		private void OnChange()
		{
		}

		private void Update()
		{
		}

		public void OnClickClose()
		{
		}

		public static void Open(string title, int r, int g, int b, Action<int, int, int> onChange, Action onClose = null)
		{
// Complete original body draft; native/CLR acceptance pending.
var panel = global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.Open(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.ColorSettingPanel, false) as ColorSettingPanelView; panel.Initialize(title, r, g, b, onChange, onClose);
}
	}
}
