using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Skill.SkillSlow
{
	[CreateAssetMenu(fileName = "MSkillSlow", menuName = "ScriptableObjects/MSkillSlow")]
	public class MSkillSlow : ScriptableObject
	{
		[SerializeField]
		private RSkillSlow[] records;

		private static Dictionary<int, RSkillSlow> dict;

		private static void CheckInitialize()
        {
            if (dict != null) return;
            dict = new Dictionary<int, RSkillSlow>();
            foreach (RSkillSlow record in global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MSkillSlow.records)
                dict.Add(record.Level, record);
        }

		public static RSkillSlow Get(int level)
		{
			CheckInitialize();
            return dict.TryGetValue(level, out RSkillSlow record) ? record : null;
		}

		public static Dictionary<int, RSkillSlow> GetAll()
		{
			CheckInitialize();
            return dict;
		}
	}
}
