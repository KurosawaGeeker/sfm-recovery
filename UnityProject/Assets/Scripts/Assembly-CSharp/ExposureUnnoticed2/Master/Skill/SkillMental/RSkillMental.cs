using UnityEngine;

namespace ExposureUnnoticed2.Master.Skill.SkillMental
{
	[CreateAssetMenu(fileName = "RSkillMental", menuName = "ScriptableObjects/RSkillMental")]
	public class RSkillMental : ScriptableObject
	{
		[SerializeField]
		public int Level;

		[SerializeField]
		public int NextLevelUpRp;

		[SerializeField]
		public float MaxMental;
	}
}
