using UnityEngine;

namespace ExposureUnnoticed2.Master.Sex
{
	[CreateAssetMenu(fileName = "RSex", menuName = "ScriptableObjects/RSex")]
	public class RSex : ScriptableObject
	{
		public SexType Type;

		public string NameKey;
	}
}
