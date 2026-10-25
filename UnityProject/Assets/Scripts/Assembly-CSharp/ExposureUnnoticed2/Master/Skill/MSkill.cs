using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Skill
{
	[CreateAssetMenu(fileName = "MSkill", menuName = "ScriptableObjects/MSkill")]
	public class MSkill : ScriptableObject
	{
		[SerializeField]
		private MSkillRecord[] skills;

		private static Dictionary<SkillType, MSkillRecord> dict;

		private static List<MSkillRecord> poseList;

		private static void CheckInitialize()
		{
			// Recovered from GameAssembly.dll RVA 0x464A20
			if (dict != null) return;
			dict = new Dictionary<SkillType, MSkillRecord>();
			poseList = new List<MSkillRecord>();
			foreach (MSkillRecord record in global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MSkill.skills)
			{
			    dict.Add(record.Type, record);
			    if ((int)record.CategoryType == 1) poseList.Add(record);
			}
		}

		public static MSkillRecord Get(SkillType type)
		{
			// Recovered from GameAssembly.dll RVA 0x464D10
			CheckInitialize();
			return dict.TryGetValue(type, out MSkillRecord record) ? record : null;
		}

		public static MSkillRecord GetByIndex(int index)
		{
			// Recovered from GameAssembly.dll RVA 0x464DB0
			return poseList[index];
		}

		public static Dictionary<SkillType, MSkillRecord> GetAll()
		{
			// Recovered from GameAssembly.dll RVA 0x464E40
			CheckInitialize();
			return dict;
		}
	}
}
