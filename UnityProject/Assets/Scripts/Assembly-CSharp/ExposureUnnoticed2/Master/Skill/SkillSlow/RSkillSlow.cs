using UnityEngine;

namespace ExposureUnnoticed2.Master.Skill.SkillSlow
{
	[CreateAssetMenu(fileName = "RSkillSlow", menuName = "ScriptableObjects/RSkillSlow")]
	public class RSkillSlow : ScriptableObject
	{
		[SerializeField]
		public int Level;

		[SerializeField]
		public float MaxTimeTank;

		[SerializeField]
		public int NextLevelUpRp;
	}
}
