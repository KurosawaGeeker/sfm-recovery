using UnityEngine;

namespace ExposureUnnoticed2.Master.Pants
{
	[CreateAssetMenu(fileName = "RBra", menuName = "ScriptableObjects/RBra")]
	public class RBra : ScriptableObject
	{
		[SerializeField]
		public BraType Type;

		[SerializeField]
		public string NameKey;

		[SerializeField]
		public int Price;
	}
}
