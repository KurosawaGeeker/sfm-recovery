using System;
using Common.Scripts.UI;
using ExposureUnnoticed2.Master.FastTravel;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.FastTravelPanel
{
	public class FastTravelPanelView : BasePanelView
	{
		[SerializeField]
		private Transform itemParent;

		[SerializeField]
		private GameObject itemPrefab;

		[SerializeField]
		private TextMeshProUGUI title;

		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		[SerializeField]
		private Image image;

		[SerializeField]
		private Sprite[] dayTimeSprite;

		[SerializeField]
		private Sprite[] nightSprite;

		private RFastTravel currentSelect;

		private int currentSelectIndex;

		private void Initialize(Action onClose)
		{
		}

		private void ToPadModeIfNeed()
		{
		}

		private void LateUpdate()
		{
		}

		public void OnClickClose()
		{
		}

		private void OnClickItem(RFastTravel record)
		{
		}

		public void OnClickFastTravel()
		{
		}

		public static void Open(Action onClose)
		{
		}
	}
}
