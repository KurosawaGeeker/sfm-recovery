using System;
using ExposureUnnoticed2.ObjectUI.ClosetMenu.CustomizeMenu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.OptionMenu
{
	public class CustomizeColorItemView : MonoBehaviour, ICustomizeItemType
	{
		[SerializeField]
		private CustomizeItemType customizeItemType;

		[SerializeField]
		private TextMeshProUGUI text;

		[SerializeField]
		private Image color;

		private Action onClose;

		private Action onOpen;

		private Action<int, int, int> onChange;

		private Toggle toggle;

		private int r;

		private int g;

		private int b;

		public CustomizeItemType CustomizeItemType => customizeItemType;

		public void Initialize(int initR, int initG, int initB, Action<int, int, int> onChange, string text = null, Action onOpen = null, Action onClose = null)
		{
// Complete original body draft; native/CLR acceptance pending.
this.onChange = onChange; this.onClose = onClose; this.onOpen = onOpen; r = initR; g = initG; b = initB; color.color = new Color((float)initR / 255f, (float)initG / 255f, (float)initB / 255f, 1f); this.onChange(initR, initG, initB); if (text != null) this.text.text = text;
}

		private void OnChange(int newR, int newG, int newB)
		{
// Complete original body draft; native/CLR acceptance pending.
r = newR; g = newG; b = newB; color.color = new Color((float)newR / 255f, (float)newG / 255f, (float)newB / 255f, 1f); onChange(newR, newG, newB);
}

		public void OnClickChange()
		{
// Complete original body draft; native/CLR acceptance pending.
onOpen?.Invoke(); global::ExposureUnnoticed2.ObjectUI.ClosetMenu.ColorSelectPanel.ColorSettingPanelView.Open(text.text, r, g, b, OnChange, onClose);
}
	}
}
