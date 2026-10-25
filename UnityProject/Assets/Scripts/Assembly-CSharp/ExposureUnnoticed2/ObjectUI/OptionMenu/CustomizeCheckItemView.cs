using System;
using ExposureUnnoticed2.ObjectUI.ClosetMenu.CustomizeMenu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.OptionMenu
{
	public class CustomizeCheckItemView : MonoBehaviour, ICustomizeItemType
	{
		[SerializeField]
		private CustomizeItemType customizeItemType;

		[SerializeField]
		private TextMeshProUGUI text;

		private Action<bool> onChange;

		private Toggle toggle;

		private bool isPlayerCustomize;

		public CustomizeItemType CustomizeItemType => customizeItemType;

		public bool Current => toggle.isOn;

		public void Initialize(bool initValue, Action<bool> onChange, string text = null)
		{
			// Recovered from GameAssembly.dll RVA 0x51BD40
			this.onChange = onChange;
			toggle = GetComponentInChildren<Toggle>();
			toggle.isOn = initValue;
			if (text != null) this.text.text = text;
		}

		public void InitializeForPlayerCustomize(bool initValue, Action<bool> onChange, string text = null)
		{
			// Recovered from GameAssembly.dll RVA 0x51BE90
			isPlayerCustomize = true;
			Initialize(initValue, onChange, text);
		}

		public void OnChangeToggle(int index)
		{
			// Recovered from GameAssembly.dll RVA 0x51BEB0
			onChange?.Invoke(toggle.isOn);
			if (isPlayerCustomize)
			    global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ChangeBodyEvent);
		}
	}
}
