using UnityEngine;

namespace ExposureUnnoticed2.Master.Cosplay
{
	[CreateAssetMenu(fileName = "RCosplay", menuName = "ScriptableObjects/RCosplay")]
	public class RCosplay : ScriptableObject
	{
		[SerializeField]
		public CosplayType Type;

		[SerializeField]
		public string SeriesNameKey;

		[SerializeField]
		private RCosplayParts[] PartsRecords;

		public void CheckInitialize()
		{
			// Recovered from GameAssembly.dll RVA 0x46A720
			for (int i = 0; i < PartsRecords.Length; ++i)
			{
			    PartsRecords[i].Id = i;
			    PartsRecords[i].ParentCosplayType = Type;
			}
		}

		public RCosplayParts Get(int id)
		{
			// Recovered from GameAssembly.dll RVA 0x46A7B0
			if (id < PartsRecords.Length) return PartsRecords[id];
			throw new System.Exception("PartsRecordsのLengthを超えています  id:" + id + " records.Length:" + PartsRecords.Length);
		}

		public RCosplayParts[] GetAll()
		{
			// Recovered from GameAssembly.dll RVA 0x3F88B0
			return PartsRecords;
		}
	}
}
