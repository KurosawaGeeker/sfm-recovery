using Common.Scripts.UI;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.ChooseHandcuffsPanel
{
	public class SleepSelectPanelView : BasePanelView
	{
		private enum UntilType
		{
			Morning = 0,
			Night = 1
		}

		[SerializeField]
		private TextMeshProUGUI descriptionText;

		[SerializeField]
		private TextMeshProUGUI currentTimeText;

		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		private UntilType currentUntil;

		private void Initialize()
		{
		}

		private void RefreshView()
		{
		}

		private void LateUpdate()
		{
		}

		public void OnClickCancel()
		{
		}

		public void OnClickMorning()
		{
		}

		public void OnClickNight()
		{
		}

		public void OnClickSleep()
		{
		}

		public static void Open()
		{
		}
	}
}
