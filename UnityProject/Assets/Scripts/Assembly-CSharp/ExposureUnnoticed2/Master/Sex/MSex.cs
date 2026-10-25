using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Sex
{
	[CreateAssetMenu(fileName = "MSex", menuName = "ScriptableObjects/MSex")]
	public class MSex : ScriptableObject
	{
		[SerializeField]
		private RSex[] sexs;

		private static Dictionary<SexType, RSex> dict;

		private static void CheckInitialize()
		{
		}

		public static RSex Get(SexType type)
		{
			return null;
		}

		public static Dictionary<SexType, RSex> GetAll()
		{
			return null;
		}
	}
}
