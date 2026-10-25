using ExposureUnnoticed2.Master.Cosplay;
using ExposureUnnoticed2.Master.Item;
using ExposureUnnoticed2.Master.Stage;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Achievement
{
	[CreateAssetMenu(fileName = "RAchievement", menuName = "ScriptableObjects/RAchievement")]
	public class RAchievement : ScriptableObject
	{
		[SerializeField]
		public string NameKey;

		[SerializeField]
		public AchievementRewardType RewordRewardType;

		[SerializeField]
		public CosplayType RewardCosplay;

		[SerializeField]
		public ItemType RewardItem;

		[SerializeField]
		public int RewardRp;

		[SerializeField]
		public AchievementConditionType AchievementConditionType;

		[SerializeField]
		public StageType ConditionStageType;

		[SerializeField]
		public int ConditionHighScore;

		[SerializeField]
		public int ConditionPercent100;

		public int Id { get; set; }

		public bool CalcIsAchieved()
		{
            if ((int)AchievementConditionType == 1)
                return ConditionHighScore <= global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.StatistisMaxEarnRp;
            float total = 0f;
            float achieved = 0f;
            if ((int)AchievementConditionType == 2)
            {
                foreach (var pair in global::System.Linq.Enumerable.OrderBy(global::ExposureUnnoticed2.Master.Mission.MMission.Get(ConditionStageType).GetAll(), o => o.Value.ReleaseRank))
                {
                    total += 1f;
                    if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetMissionAchieveCount(pair.Value.Id) > 0) achieved += 1f;
                }
                return ConditionPercent100 * .01f * total <= achieved;
            }
            if ((int)AchievementConditionType != 3) return false;
            foreach (var stage in global::ExposureUnnoticed2.Master.Mission.MMission.GetAll())
            {
                foreach (var pair in global::System.Linq.Enumerable.OrderBy(stage.Value.GetAll(), o => o.Value.ReleaseRank))
                {
                    total += 1f;
                    if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetMissionAchieveCount(pair.Value.Id) > 0) achieved += 1f;
                }
            }
            foreach (var pair in global::ExposureUnnoticed2.Master.DroneMission.MDroneMission.GetAll())
            {
                total += 1f;
                if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetDroneMissionAchieveCount(pair.Value) > 0) achieved += 1f;
            }
            return total <= achieved;
        }
	}
}
