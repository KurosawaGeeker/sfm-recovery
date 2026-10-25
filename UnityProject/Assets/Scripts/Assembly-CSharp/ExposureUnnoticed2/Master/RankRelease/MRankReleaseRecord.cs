using UnityEngine;

namespace ExposureUnnoticed2.Master.RankRelease
{
	[CreateAssetMenu(fileName = "MRankReleaseRecord", menuName = "ScriptableObjects/MRankReleaseRecord")]
	public class MRankReleaseRecord : ScriptableObject
	{
		[SerializeField]
		public RankReleaseType Type;

		[SerializeField]
		public string NameKey;

		[SerializeField]
		public string HideNameKey;

		[SerializeField]
		public string CategoryNameKey;

		[SerializeField]
		public int ReleaseRank;
	}
}
