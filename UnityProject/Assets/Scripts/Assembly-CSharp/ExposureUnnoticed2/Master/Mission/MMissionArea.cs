using System.Collections.Generic;
using ExposureUnnoticed2.Master.Stage;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Mission
{
	[CreateAssetMenu(fileName = "MMissionArea", menuName = "ScriptableObjects/MMissionArea")]
	public class MMissionArea : ScriptableObject
	{
		[SerializeField]
		private MMissionOverrideRecord[] records;

		private Dictionary<int, MMissionOverrideRecord> dict = new Dictionary<int, MMissionOverrideRecord>();

		[field: SerializeField]
		public StageType StageType { get; private set; }

		public void Initialize()
		{
			// Recovered from GameAssembly.dll RVA 0x467140
			for (int i = 0; i < records.Length; ++i)
			{
			    MMissionOverrideRecord record = records[i];
			    record.StageType = StageType;
			    record.Id = (int)StageType * 10000 + i;
			    dict.Add(record.Id, record);
			}
		}

		public MMissionOverrideRecord Get(int id)
		{
			return null;
		}

		public Dictionary<int, MMissionOverrideRecord> GetAll()
		{
			// Recovered from GameAssembly.dll RVA 0x3F88B0
			return dict;
		}
	}
}
