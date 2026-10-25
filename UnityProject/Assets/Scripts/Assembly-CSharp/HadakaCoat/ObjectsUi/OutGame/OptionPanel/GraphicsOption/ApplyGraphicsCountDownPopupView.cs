using System;
using Common.CommonUI.CommonGauge;
using Common.Scripts.UI;
using TMPro;
using UnityEngine;

namespace HadakaCoat.ObjectsUi.OutGame.OptionPanel.GraphicsOption
{
	public class ApplyGraphicsCountDownPopupView : BasePanelView
	{
		[SerializeField]
		private TextMeshProUGUI countText;

		[SerializeField]
		private GaugeView gauge;

		private Action onYes;

		private Action onNo;

		private float countStart;

		private bool isClosed;

		private void Initialize(Action onYes, Action onNo)
		{
		}

		private void Update()
		{
		}

		public void OnClickNo()
		{
		}

		public void OnClickYes()
		{
		}

		public static void Open(Transform parent, Action onYes, Action onNo)
		{
		}
	}
}
