using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Pants
{
	[CreateAssetMenu(fileName = "MBra", menuName = "ScriptableObjects/MBra")]
	public class MBra : ScriptableObject
	{
		[SerializeField]
		private RBra[] records;

		private static Dictionary<BraType, RBra> dict;

		private static void CheckInitialize()
		{
		}

		public static RBra Get(BraType level)
		{
			return null;
		}

		public static Dictionary<BraType, RBra> GetAll()
		{
			return null;
		}
	}
}
