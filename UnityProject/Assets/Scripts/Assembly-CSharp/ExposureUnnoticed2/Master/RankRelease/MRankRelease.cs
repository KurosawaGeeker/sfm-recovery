using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Master.RankRelease
{
	[CreateAssetMenu(fileName = "MRankRelease", menuName = "ScriptableObjects/MRankRelease")]
	public class MRankRelease : ScriptableObject
	{
		[SerializeField]
		private MRankReleaseRecord[] records;

		private static Dictionary<RankReleaseType, MRankReleaseRecord> dict;

		private static void CheckInitialize()
		{
if (dict != null) return;
dict = new Dictionary<RankReleaseType, MRankReleaseRecord>();
foreach (var record in global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MRankRelease.records)
    dict.Add(record.Type, record);
}

		public static List<MRankReleaseRecord> GetByRank(int rank)
		{
CheckInitialize();
var result = new List<MRankReleaseRecord>();
foreach (var pair in dict) if (pair.Value.ReleaseRank == rank) result.Add(pair.Value);
return result;
}

		public static MRankReleaseRecord Get(RankReleaseType type)
		{
            CheckInitialize();
            return dict.TryGetValue(type, out MRankReleaseRecord record) ? record : null;
        }

		public static Dictionary<RankReleaseType, MRankReleaseRecord> GetAll()
		{
			return null;
		}
	}
}
