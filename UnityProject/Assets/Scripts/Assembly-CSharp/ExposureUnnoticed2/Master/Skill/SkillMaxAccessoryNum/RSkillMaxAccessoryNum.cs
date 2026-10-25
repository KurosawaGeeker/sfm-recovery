using UnityEngine;

namespace ExposureUnnoticed2.Master.Skill.SkillMaxAccessoryNum
{
	[CreateAssetMenu(fileName = "RSkillMaxAccessoryNum", menuName = "ScriptableObjects/RSkillMaxAccessoryNum")]
	public class RSkillMaxAccessoryNum : ScriptableObject
	{
		[SerializeField]
		public int Level;

		[SerializeField]
		public int MaxNum;

		[SerializeField]
		public int NextLevelUpRp;
	}
}
