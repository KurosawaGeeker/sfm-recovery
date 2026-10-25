using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Stage
{
	[CreateAssetMenu(fileName = "MStage", menuName = "ScriptableObjects/MStage")]
	public class MStage : ScriptableObject
	{
		[SerializeField]
		private MStageRecord[] stages;

		private static Dictionary<StageType, MStageRecord> dict;

		private static List<MStageRecord> list;

		private static SortedDictionary<float, StageType> sortTypeDict = new SortedDictionary<float, StageType>();

		private static void CheckInitialize()
		{
			// Recovered from GameAssembly.dll RVA 0x464060
			if (dict != null) return;
			dict = new Dictionary<StageType, MStageRecord>();
			list = new List<MStageRecord>();
			foreach (MStageRecord record in global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MStage.stages)
			{
			    dict.Add(record.StageType, record);
			    list.Add(record);
			}
		}

		public static MStageRecord Get(StageType type)
		{
			// Recovered from GameAssembly.dll RVA 0x4643D0
			CheckInitialize();
			return dict.TryGetValue(type, out MStageRecord record) ? record : null;
		}

		public static Dictionary<StageType, MStageRecord> GetAll()
		{
			// Recovered from GameAssembly.dll RVA 0x464480
			CheckInitialize();
			return dict;
		}

		public static MStageRecord GetByIndex(int index)
		{
			// Recovered from GameAssembly.dll RVA 0x4644E0
			return list[index];
		}

		public static StageType GetRandomType(StageType excludeType = StageType.None)
		{
			// Recovered from GameAssembly.dll RVA 0x464580
			CheckInitialize();
			sortTypeDict.Clear();
			foreach (var item in dict)
			{
			    MStageRecord record = item.Value;
			    if (!record.IsGoalTarget || record.StageType == excludeType) continue;
			    float value = UnityEngine.Random.value;
			    if (!sortTypeDict.ContainsKey(value)) sortTypeDict.Add(value, record.StageType);
			}
			return System.Linq.Enumerable.First(sortTypeDict).Value;
		}

		public static StageType NextStageTypeOnPath(StageType start, StageType goal)
		{
			// Recovered from GameAssembly.dll RVA 0x4648D0
			CheckInitialize();
			return goal;
		}
	}
}
