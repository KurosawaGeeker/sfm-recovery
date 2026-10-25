using ExposureUnnoticed2.Master.ReinforceEffect;
using ExposureUnnoticed2.Object3D.Player.Scripts.Other;
using ExposureUnnoticed2.ObjectUI.PcMenuPanel.ReinforcePanel;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.BuffPanel
{
	public class BuffPanelItemView : MonoBehaviour
	{
		public enum BuffType
		{
			RpBonus = 0,
			Strange = 1,
			Dokidoki = 2,
			Stamina = 3,
			EcstasyGauge = 4,
			MissionGauge = 5,
			SlowGauge = 6,
			SkillCoolTime = 7,
			GameOverPenaltyRp = 8,
			GameOverPenaltyRank = 9,
			MentalDecreaseScale = 10,
			DashSpeed = 11,
			StaminaRegenerate = 12,
			RpBonusReinforce = 13,
			ReduceFootStepSound = 14,
			UnlockSpeed = 15,
			RpBonusLastMult = 16
		}

		[SerializeField]
		private TextMeshProUGUI sourceName;

		[SerializeField]
		private TextMeshProUGUI effectAmount;

		[SerializeField]
		private ReinforceEffectRankIconView iconView;

		public void Initialize(BuffModel model, BuffType buffType)
		{
		}

		public void Initialize(int amount, BuffType buffType)
		{
		}

		public void Initialize(RReinforceEffect record)
		{
		}
	}
}
