using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Skill.SkillMental
{
	[CreateAssetMenu(fileName = "MSkillMental", menuName = "ScriptableObjects/MSkillMental")]
	public class MSkillMental : ScriptableObject
	{
		[SerializeField]
		private RSkillMental[] records;

		private static Dictionary<int, RSkillMental> dict;

		private static void CheckInitialize()
		{
			// Recovered from GameAssembly.dll RVA 0x465430
			if (dict != null) return;
			dict = new Dictionary<int, RSkillMental>();
			foreach (RSkillMental record in global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MSkillMental.records)
			    dict.Add(record.Level, record);
		}

		public static RSkillMental Get(int level)
		{
			// Recovered from GameAssembly.dll RVA 0x465600
			CheckInitialize();
			return dict.TryGetValue(level, out RSkillMental record) ? record : null;
		}

		public static Dictionary<int, RSkillMental> GetAll()
		{
			// Recovered from GameAssembly.dll RVA 0x4656A0
			CheckInitialize();
			return dict;
		}
	}
}
