using System.Collections.Generic;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Pants
{
	[CreateAssetMenu(fileName = "MPants", menuName = "ScriptableObjects/MPants")]
	public class MPants : ScriptableObject
	{
		[SerializeField]
		private RPants[] records;

		private static Dictionary<PantsType, RPants> dict;

		private static void CheckInitialize()
		{
		}

		public static RPants Get(PantsType level)
		{
			return null;
		}

		public static Dictionary<PantsType, RPants> GetAll()
		{
			return null;
		}
	}
}
