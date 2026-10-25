using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Rank
{
	[CreateAssetMenu(fileName = "MRank", menuName = "ScriptableObjects/MRank")]
	public class MRank : ScriptableObject
	{
		public static int MaxRank;

		public static int MaxFearBias;

		[SerializeField]
		private MRankRecord[] records;

		private static Dictionary<int, MRankRecord> dict;

		private static void CheckInitialize()
		{
			// Recovered from GameAssembly.dll RVA 0x466860
			if (dict != null) return;
			dict = new Dictionary<int, MRankRecord>();
			foreach (MRankRecord record in global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MRank.records)
			{
			    dict.Add(record.Rank, record);
			    if (record.Rank > MaxRank) MaxRank = record.Rank;
			    if (record.FearLevelBias > MaxFearBias) MaxFearBias = record.FearLevelBias;
			}
		}

		public static MRankRecord CalcCurrentRank(double totalRp)
		{
			// Recovered from GameAssembly.dll RVA 0x466A90
			CheckInitialize();
			MRankRecord current = GetByRank(0);
			foreach (MRankRecord record in dict.Values)
			{
			    if (!(totalRp >= record.NeedRp)) break;
			    current = record;
			}
			return current;
		}

		public static MRankRecord GetByRank(int rank)
		{
			// Recovered from GameAssembly.dll RVA 0x466C80
			CheckInitialize();
			MRankRecord record;
			return dict.TryGetValue(rank, out record) ? record : null;
		}

		public static Dictionary<int, MRankRecord> GetAll()
		{
			// Recovered from GameAssembly.dll RVA 0x466D20
			CheckInitialize();
			return dict;
		}
	}
}
