using System;
using ExposureUnnoticed2.Master.Rank;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.MissionMenuPanel
{
	public class RankConfirmItemView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI rewardText;

		[SerializeField]
		private TextMeshProUGUI rankNameText;

		private Action<MRankRecord> onClick;

		private MRankRecord record;

		public void Initialize(MRankRecord record, Action<MRankRecord> onClick)
		{
		}

		public void OnClick()
		{
		}
	}
}
