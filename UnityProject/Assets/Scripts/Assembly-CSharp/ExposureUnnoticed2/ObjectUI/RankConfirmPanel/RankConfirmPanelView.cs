using System;
using Common.CommonUI.CommonGauge;
using Common.Scripts.UI;
using ExposureUnnoticed2.Master.Rank;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.RankConfirmPanel
{
	public class RankConfirmPanelView : BasePanelView
	{
		[SerializeField]
		private Transform rankItemParent;

		[SerializeField]
		private Transform releaseItemParent;

		[SerializeField]
		private GameObject rankItemPrefab;

		[SerializeField]
		private GameObject releaseItemPrefab;

		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		[Header("ランク")]
		[SerializeField]
		private TextMeshProUGUI currentRankName;

		[SerializeField]
		private TextMeshProUGUI nextRankName;

		[SerializeField]
		private TextMeshProUGUI currentExp;

		[SerializeField]
		private GaugeFillView expGauge;

		private new Action onClose;

		private int currentSelectIndex;

		private void Initialize(Action onClose)
		{
		}

		private void ToPadModeIfNeed()
		{
		}

		private void OnClickRankItem(MRankRecord record)
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
