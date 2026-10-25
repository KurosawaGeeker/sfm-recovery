using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Skill.SkillSlow
{
	[CreateAssetMenu(fileName = "MSkillStamina", menuName = "ScriptableObjects/MSkillStamina")]
	public class MSkillStamina : ScriptableObject
	{
		[SerializeField]
		private RSkillStamina[] records;

		private static Dictionary<int, RSkillStamina> dict;

		private static void CheckInitialize()
		{
			// Recovered from GameAssembly.dll RVA 0x465170
			if (dict != null) return;
			dict = new Dictionary<int, RSkillStamina>();
			foreach (RSkillStamina record in global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MSkillStamina.records)
			    dict.Add(record.Level, record);
		}

		public static RSkillStamina Get(int level)
		{
			// Recovered from GameAssembly.dll RVA 0x465340
			CheckInitialize();
			return dict.TryGetValue(level, out RSkillStamina record) ? record : null;
		}

		public static Dictionary<int, RSkillStamina> GetAll()
		{
			// Recovered from GameAssembly.dll RVA 0x4653E0
			CheckInitialize();
			return dict;
		}
	}
}
