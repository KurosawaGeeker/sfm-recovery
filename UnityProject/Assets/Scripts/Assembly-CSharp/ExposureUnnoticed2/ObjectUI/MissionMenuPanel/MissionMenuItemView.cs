using ExposureUnnoticed2.Master.Mission;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.ObjectUI.Misc;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.InGame;
using ExposureUnnoticed2.Scripts.Mission;
using Ricimi;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.MissionMenuPanel
{
	public class MissionMenuItemView : MonoBehaviour
	{
		[SerializeField]
		private GameObject checkMarkObject;

		[SerializeField]
		private TextMeshProUGUI firstTimeTextObject;

		[SerializeField]
		private TextMeshProUGUI missionNameText;

		[SerializeField]
		private GameObject hideMissionName;

		[SerializeField]
		private TextMeshProUGUI rewardText;

		[SerializeField]
		private TextMeshProUGUI achievementsText;

		[SerializeField]
		private GameObject rankConditionMask;

		[SerializeField]
		private GameObject clearMask;

		[SerializeField]
		private TextMeshProUGUI rankConditionText;

		[SerializeField]
		private TextMeshProUGUI maxRpBonus;

		[SerializeField]
		private Ricimi.Gradient gradeImage;

		public void Initialize(MMissionOverrideRecord mission)
		{
			// Recovered original RVA 0x4DF580. Retain the original discarded
			// IsSuccessMission query and use the persisted completion count.
			var currentRank = PlayerController.Instance.Pca.PlayerState.CurrentRank;
			int achievements = GameStateFacade.GetMissionAchieveCount(mission.Id);
			_ = MissionManager.Instance.IsSuccessMission(mission);
			int reward = mission.RewardRankPoint;
			int rank = currentRank.Rank;
			int releaseRank = mission.ReleaseRank;
			string rewardLabel;
			switch (mission.OriginalRecord.MissionType)
			{
				// UI repair: translate the suffix before adding the numeric reward.
				// The recovered concatenated key is absent from the shipped dictionary.
				case MissionType.Time: rewardLabel = reward.ToString() + Localizer.Trans("ui_mission_rp_per_second"); break;
				case MissionType.Condition:
				case MissionType.Continue: rewardLabel = reward.ToString() + "RP"; break;
				default: throw new System.Runtime.CompilerServices.SwitchExpressionException(mission.OriginalRecord.MissionType);
			}
			rewardText.text = Localizer.Trans(rewardLabel);
			checkMarkObject.SetActive(achievements > 0);
			missionNameText.text = mission.GetMissionName();
			achievementsText.text = achievements.ToString();
			rankConditionMask.SetActive(rank < releaseRank);
			rankConditionText.text = Localizer.Trans("ui_release_at_rank", GameStateFacade.GetRankNameAutoHide(mission.ReleaseRank));
			missionNameText.gameObject.SetActive(releaseRank <= rank);
			hideMissionName.SetActive(rank < releaseRank);
			// Original helper RVA 0x6390 is Dictionary<int,int>.get_Item, not a default read.
			float bonus = (float)GameState.GameStateData.MissionMaxRpBonus100Dict[mission.Id] * 0.01f;
			maxRpBonus.SetText("x{0:2}", bonus);
			Color secondary = RpBonusColorSelector.GetTextColorSecondary(bonus);
			Color primary = RpBonusColorSelector.GetTextColor(bonus);
			gradeImage.gameObject.SetActive(bonus >= 1.5f);
			gradeImage.Color1 = RpBonusColorSelector.GetTextColor(bonus);
			gradeImage.Color2 = RpBonusColorSelector.GetTextColorSecondary(bonus);
			maxRpBonus.colorGradient = new VertexGradient(secondary, secondary, primary, primary);
			clearMask.SetActive(GameStateFacade.GetMissionSatisfy(mission.Id) >= 1f);
			if (SystemManager.IsTrial && releaseRank <= rank && !mission.Trial)
			{
				rankConditionMask.SetActive(true);
				rankConditionText.enabled = false;
				missionNameText.text = "???";
			}
		}
	}
}
