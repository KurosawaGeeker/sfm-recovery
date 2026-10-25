using UnityEngine;

namespace ExposureUnnoticed2.Master.ReinforceEffect
{
	[CreateAssetMenu(fileName = "RReinforceEffect", menuName = "ScriptableObjects/RReinforceEffect")]
	public class RReinforceEffect : ScriptableObject
	{
		[SerializeField]
		public string NameKey;

		[SerializeField]
		public ReinforceEffectType ReinforceEffectType;

		[SerializeField]
		public int EffectAmount100;

		[SerializeField]
		public int EffectRank;

		[SerializeField]
		public bool IsNegativeEffect;

		[SerializeField]
		public bool IsNeutralEffect;

		public int Id { get; set; }

		public string Suffix => null;
	}
}
