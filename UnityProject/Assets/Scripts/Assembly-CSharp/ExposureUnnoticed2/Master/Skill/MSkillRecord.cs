using UnityEngine;

namespace ExposureUnnoticed2.Master.Skill
{
	[CreateAssetMenu(fileName = "MSkillRecord", menuName = "ScriptableObjects/MSkillRecord")]
	public class MSkillRecord : ScriptableObject
	{
		public string NameKey;

		public string DescriptionKey;

		public int InitRp;

		public int IncrementRp;

		public int MaxLevel;

		public int ReleaseRank;

		public bool IsPassiveToggle;

		public SkillType Type;

		public SkillCategoryType CategoryType;

		public SkillType ReleaseOrderTarget;

		public int RpBonus;
	}
}
