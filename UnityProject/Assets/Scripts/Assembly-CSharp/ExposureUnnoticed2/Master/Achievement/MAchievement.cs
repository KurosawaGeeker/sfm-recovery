using System.Collections.Generic;
using ExposureUnnoticed2.Master.Cosplay;
using ExposureUnnoticed2.Master.Item;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Achievement
{
	[CreateAssetMenu(fileName = "MAchievement", menuName = "ScriptableObjects/MAchievement")]
	public class MAchievement : ScriptableObject
	{
		[SerializeField]
		private RAchievement[] records;

		private static Dictionary<int, RAchievement> dict;

		private static void CheckInitialize()
		{
            if (dict != null) return;
            dict = new Dictionary<int, RAchievement>();
            var source = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MAchievement.records;
            for (int i = 0; i < source.Length; i++)
            {
                var record = source[i];
                record.Id = i;
                dict.Add(i, record);
            }
        }

		public static RAchievement Get(int id)
		{
			return null;
		}

		public static Dictionary<int, RAchievement> GetAll()
		{
            CheckInitialize();
            return dict;
        }

		public static bool IsRelease(CosplayType cosplayType)
		{
            CheckInitialize();
            foreach (var pair in dict)
            {
                var record = pair.Value;
                if ((int)record.RewordRewardType == 1 && record.RewardCosplay == cosplayType)
                    return global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.AchievementReceiveSet.Contains(record.Id);
            }
            return true;
		}

		public static bool IsRelease(ItemType itemType)
		{
            CheckInitialize();
            foreach (var pair in dict)
            {
                var record = pair.Value;
                if ((int)record.RewordRewardType == 2 && record.RewardItem == itemType)
                    return global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.AchievementReceiveSet.Contains(record.Id);
            }
            return true;
		}
	}
}
