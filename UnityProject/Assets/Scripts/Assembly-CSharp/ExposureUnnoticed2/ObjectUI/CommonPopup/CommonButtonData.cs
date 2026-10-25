using System;
using ExposureUnnoticed2.Scripts.Sound;

namespace ExposureUnnoticed2.ObjectUI.CommonPopup
{
	public struct CommonButtonData
	{
		public InGameSoundManager.SeType SeType;

		public string Text;

		public Action OnClick;

		public CommonButtonData(InGameSoundManager.SeType seType, Action onClick)
		{
			// Recovered from GameAssembly.dll RVA 0x577C70
			SeType = seType;
			OnClick = onClick;
			Text = null;
		}

		public CommonButtonData(Action onClick)
		{
			// Recovered from GameAssembly.dll RVA 0x577D20
			OnClick = onClick;
			SeType = InGameSoundManager.SeType.NoType;
			Text = null;
		}
	}
}
