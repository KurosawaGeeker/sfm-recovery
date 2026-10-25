using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Cosplay
{
	[CreateAssetMenu(fileName = "MCosplay", menuName = "ScriptableObjects/MCosplay")]
	public class MCosplay : ScriptableObject
	{
		[SerializeField]
		private RCosplay[] records;

		private static Dictionary<CosplayType, RCosplay> dict;

		private static List<RCosplay> list;

		public static int CosplayNum => dict.Count;

		private static void CheckInitialize()
		{
			// Recovered from GameAssembly.dll RVA 0x46A1D0
			if (dict != null) return;
			dict = new Dictionary<CosplayType, RCosplay>();
			list = new List<RCosplay>();
			foreach (RCosplay record in global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MCosplay.records)
			{
			    dict.Add(record.Type, record);
			    list.Add(record);
			    record.CheckInitialize();
			}
		}

		public static RCosplayParts GetParts(int uniqueId)
		{
			// Recovered from GameAssembly.dll RVA 0x46A540
			return Get((CosplayType)(uniqueId / 10000)).Get(uniqueId % 10000);
		}

		public static RCosplay Get(CosplayType type)
		{
			// Recovered from GameAssembly.dll RVA 0x46A5A0
			CheckInitialize();
			return dict.TryGetValue(type, out RCosplay record) ? record : null;
		}

		public static Dictionary<CosplayType, RCosplay> GetAll()
		{
			// Recovered from GameAssembly.dll RVA 0x46A640
			CheckInitialize();
			return dict;
		}

		public static RCosplay GetByIndex(int index)
		{
			// Recovered from GameAssembly.dll RVA 0x46A690
			return list[index];
		}
	}
}
