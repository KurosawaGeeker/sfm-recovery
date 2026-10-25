using System.Collections.Generic;
using ExposureUnnoticed2.Master.Stage;
using UnityEngine;

namespace ExposureUnnoticed2.Master.FastTravel
{
	[CreateAssetMenu(fileName = "MFastTravel", menuName = "ScriptableObjects/MFastTravel")]
	public class MFastTravel : ScriptableObject
	{
		[SerializeField]
		private RFastTravel[] records;

		private static Dictionary<int, RFastTravel> dict;

		private static void CheckInitialize()
		{
            // Recovered complete original native body.
            if (dict != null) return;
            dict = new Dictionary<int, RFastTravel>();
            RFastTravel[] source = ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MFastTravel.records;
            for (int i = 0; i < source.Length; i++)
            {
                RFastTravel record = source[i];
                record.Id = i;
                dict.Add(i, record);
            }
        }

		public static RFastTravel Get(int id)
		{
            // Recovered complete original native body.
            CheckInitialize();
            return dict.TryGetValue(id, out RFastTravel record) ? record : null;
        }

		public static List<RFastTravel> CreateListByStage(StageType type)
		{
            // Recovered complete original native body.
            CheckInitialize();
            List<RFastTravel> result = new List<RFastTravel>();
            foreach (RFastTravel record in dict.Values)
                if (record.StageType == type) result.Add(record);
            return result;
        }

		public static Dictionary<int, RFastTravel> GetAll()
		{
            // Recovered complete original native body.
            CheckInitialize();
            return dict;
        }
	}
}
