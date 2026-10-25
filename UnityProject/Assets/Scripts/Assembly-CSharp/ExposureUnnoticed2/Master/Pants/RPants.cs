using ExposureUnnoticed2.Object3D.Player.Scripts;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Pants
{
	[CreateAssetMenu(fileName = "RPants", menuName = "ScriptableObjects/RPants")]
	public class RPants : ScriptableObject
	{
		[SerializeField]
		public PantsType Type;

		[SerializeField]
		public string NameKey;

		[SerializeField]
		public int Price;
	}
}
