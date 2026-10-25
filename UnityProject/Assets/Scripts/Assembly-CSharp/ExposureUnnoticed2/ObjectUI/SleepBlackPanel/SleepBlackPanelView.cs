using Common.Scripts.UI;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.SleepBlackPanel
{
	public class SleepBlackPanelView : BasePanelView
	{
		[SerializeField]
		private TextMeshProUGUI textObject;

		private bool is24h;

		private bool isKillTime;

		public static bool IsOpen { get; private set; }

		private void Initialize(bool is24h, bool isKillTime)
		{
		}

		public void OnEnterPureBlack()
		{
		}

		public void OnAnimEnd()
		{
		}

		public void PlayJingle()
		{
		}

		public static void Open(bool is24h, bool isKillTime)
		{
		}
	}
}
