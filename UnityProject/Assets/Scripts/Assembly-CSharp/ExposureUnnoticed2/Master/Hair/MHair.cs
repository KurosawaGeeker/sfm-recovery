using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Hair
{
	[CreateAssetMenu(fileName = "MHair", menuName = "ScriptableObjects/MHair")]
	public class MHair : ScriptableObject
	{
		[SerializeField]
		private MHairRecord[] records;

		private static Dictionary<HairType, MHairRecord> dict;

		private static void CheckInitialize()
		{
			// Recovered from GameAssembly.dll RVA 0x468E00
			if (dict != null) return;
			dict = new Dictionary<HairType, MHairRecord>();
			foreach (MHairRecord record in global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MHair.records)
			    dict.Add(record.Type, record);
		}

		public static MHairRecord Get(HairType type)
		{
			// Recovered from GameAssembly.dll RVA 0x468FD0
			CheckInitialize();
			return dict.TryGetValue(type, out MHairRecord result) ? result : null;
		}

		public static Dictionary<HairType, MHairRecord> GetAll()
		{
			// Recovered from GameAssembly.dll RVA 0x469070
			CheckInitialize();
			return dict;
		}
	}
}
