using System;
using Common.CommonUI.CommonGauge;
using Common.Scripts.UI;
using ExposureUnnoticed2.Master.ReinforceEffect;
using ExposureUnnoticed2.Object3D.Player.Scripts.Other;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.BuffPanel
{
	public class BuffPanelView : BasePanelView
	{
		private struct TotalData
		{
			public int RpBonus100;

			public int RpBonusLastMult100;

			public int RpBonusReinforce100;

			public int Strange100;

			public int Dokidoki100;
		}

		[SerializeField]
		private TextMeshProUGUI baseInfoCurrentRp;

		[SerializeField]
		private TextMeshProUGUI baseInfoBaseBareyasusa;

		[SerializeField]
		private TextMeshProUGUI difficulty;

		[SerializeField]
		private GameObject itemPrefab;

		[SerializeField]
		private Transform itemParent;

		[SerializeField]
		private GameObject barItemPrefab;

		[SerializeField]
		private Transform totalParent;

		[SerializeField]
		private TextMeshProUGUI currentFearRank;

		[SerializeField]
		private TextMeshProUGUI currentFearRankValue;

		[SerializeField]
		private TextMeshProUGUI nextFearRankValue;

		[SerializeField]
		private GaugeView fearGauge;

		[SerializeField]
		private StatusPanelStatisticsView statusPanelStatisticsView;

		private new Action onClose;

		private TotalData totalData;

		public void Initialize(Action onClose)
		{
		}

		private void CreateItem(BuffModel buffModel)
		{
		}

		private bool CreateBarIfInCondition(bool currentIsCreateBar, string name)
		{
			return false;
		}

		private void CreateReinforceItem(int[] effectIds, bool isCreatedBar, string barName)
		{
		}

		private void AddTotalData(RReinforceEffect record)
		{
		}

		private void LateUpdate()
		{
		}

		public void OnClickClose()
		{
		}

		public static void Open(Action onClose)
		{
		}
	}
}
