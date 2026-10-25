using System.Collections.Generic;
using ExposureUnnoticed2.Master.Stage;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Mission
{
	[CreateAssetMenu(fileName = "MMission", menuName = "ScriptableObjects/MMission")]
	public class MMission : ScriptableObject
	{
		[SerializeField]
		private MMissionArea[] records;

		private static Dictionary<StageType, MMissionArea> dict;

		private static void CheckInitialize()
		{
			// Recovered from GameAssembly.dll RVA 0x466D80
			if (dict != null) return;
			dict = new Dictionary<StageType, MMissionArea>();
			foreach (MMissionArea record in global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MMission.records)
			{
			    dict.Add(record.StageType, record);
			    record.Initialize();
			}
		}

		public static MMissionArea Get(StageType type)
		{
CheckInitialize();
return dict.TryGetValue(type, out var record) ? record : null;
}

		public static Dictionary<StageType, MMissionArea> GetAll()
		{
			// Recovered from GameAssembly.dll RVA 0x4670D0
			CheckInitialize();
			return dict;
		}
	}
}
