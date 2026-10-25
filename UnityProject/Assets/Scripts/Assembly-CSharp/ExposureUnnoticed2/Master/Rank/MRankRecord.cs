using UnityEngine;

namespace ExposureUnnoticed2.Master.Rank
{
	[CreateAssetMenu(fileName = "MRankRecord", menuName = "ScriptableObjects/MRankRecord")]
	public class MRankRecord : ScriptableObject
	{
		[SerializeField]
		public int Rank;

		[SerializeField]
		public string NameKey;

		[SerializeField]
		public int NeedRp;

		[SerializeField]
		public float DokidokiBoostScale = 1f;

		[SerializeField]
		public int NeedFearUpRp;

		[SerializeField]
		public int FearLevelBias;

		[SerializeField]
		public int LostRpByGameOver;
	}
}
