using System;
using Common.CommonUI.CommonGauge;
using Common.Scripts.UI;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.WaitPanel
{
	public class WaitPanelView : BasePanelView
	{
		[SerializeField]
		private GaugeView gauge;

		private float startTime;

		private float waitTime;

		private bool isClose;

		private bool isUnscaled;

		private Action onComplete;

		private Func<bool> cancelCondition;

		private Action onCancel;

		private void Initialize(float waitTime, Action onComplete, bool isUnscaled, Func<bool> cancelCondition = null, Action onCancel = null)
		{
		}

		private float GetTime()
		{
			return 0f;
		}

		private void LateUpdate()
		{
		}

		public void OnClickCancel()
		{
		}

		public static void Open(float waitTime, Action onComplete, bool isUnscaled = false, Func<bool> cancelCondition = null, Action onCancel = null)
		{
		}
	}
}
