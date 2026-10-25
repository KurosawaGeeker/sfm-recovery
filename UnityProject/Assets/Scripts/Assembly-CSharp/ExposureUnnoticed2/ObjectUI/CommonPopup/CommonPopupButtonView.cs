using System;
using ExposureUnnoticed2.Scripts.Sound;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.CommonPopup
{
	public class CommonPopupButtonView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI text;

		public InGameSoundManager.SeType SeType { get; set; }

		public Action OnClick { get; set; }

		public void UpdateView(CommonButtonData data)
		{
			// Recovered from GameAssembly.dll RVA 0x577DE0
			if (data.SeType != InGameSoundManager.SeType.NoType) SeType = data.SeType;
			if (!string.IsNullOrEmpty(data.Text)) text.text = data.Text;
			if (data.OnClick != null) OnClick = data.OnClick;
		}
	}
}
