using UnityEngine;

namespace ExposureUnnoticed2.Master.Skill.SkillSlow
{
	[CreateAssetMenu(fileName = "RSkillStamina", menuName = "ScriptableObjects/RSkillStamina")]
	public class RSkillStamina : ScriptableObject
	{
		[SerializeField]
		public int Level;

		[SerializeField]
		public int NextLevelUpRp;

		[SerializeField]
		public float MaxStamina;

		[SerializeField]
		public float RegenerationCoolTime;

		[SerializeField]
		public float Regeneration;
	}
}
