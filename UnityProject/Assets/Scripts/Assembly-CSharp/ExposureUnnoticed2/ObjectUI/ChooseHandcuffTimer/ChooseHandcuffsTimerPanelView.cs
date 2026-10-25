using System;
using Common.Scripts.UI;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.ChooseHandcuffTimer
{
	public class ChooseHandcuffsTimerPanelView : BasePanelView
	{
		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		[SerializeField]
		private TextMeshProUGUI descriptionText;

		private int currentSelectIndex;

		private int currentSelectMinutes;

		private Action onSet;

		private Action onCancel;

		private bool isMap;

		private void Initialize(Action onSet, bool isMap, Action onCancel)
		{
		}

		private void ToPadModeIfNeed()
		{
		}

		private void Update()
		{
		}

		public void OnClickMinutes(int minutes)
		{
		}

		public void OnClickSet()
		{
		}

		public void OnClickCancel()
		{
		}

		public static void Open(Action onSet, bool isMap, Action onCancel)
		{
		}
	}
}
