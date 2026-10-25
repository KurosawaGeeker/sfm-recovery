using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Skill.SkillMaxAccessoryNum
{
	[CreateAssetMenu(fileName = "MSkillMaxAccessoryNum", menuName = "ScriptableObjects/MSkillMaxAccessoryNum")]
	public class MSkillMaxAccessoryNum : ScriptableObject
	{
		[SerializeField]
		private RSkillMaxAccessoryNum[] records;

		private static Dictionary<int, RSkillMaxAccessoryNum> dict;

		private static void CheckInitialize()
		{
            // Recovered complete original native body.
            if (dict != null) return;
            dict = new Dictionary<int, RSkillMaxAccessoryNum>();
            RSkillMaxAccessoryNum[] source = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MSkillMaxAccessoryNum.records;
            for (int i = 0; i < source.Length; i++)
            {
                RSkillMaxAccessoryNum record = source[i];
                dict.Add(record.Level, record);
            }
        }

		public static RSkillMaxAccessoryNum Get(int level)
		{
            // Recovered complete original native body.
            CheckInitialize();
            return dict.TryGetValue(level, out RSkillMaxAccessoryNum record) ? record : null;
        }

		public static Dictionary<int, RSkillMaxAccessoryNum> GetAll()
		{
            // Recovered complete original native body.
            CheckInitialize();
            return dict;
        }
	}
}
