using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Master.DroneMission
{
	[CreateAssetMenu(fileName = "MDroneMission", menuName = "ScriptableObjects/MDroneMission")]
	public class MDroneMission : ScriptableObject
	{
		[SerializeField]
		private RDroneMission[] records;

		private static Dictionary<int, RDroneMission> dict;

		private static void CheckInitialize()
		{
            // Complete original0x469840; publish dict before asset reads and assign original ordinal IDs.
            if (dict != null) return;
            dict = new Dictionary<int, RDroneMission>();
            var source = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MDroneMission.records;
            for (int i = 0; i < source.Length; i++)
            {
                var record = source[i];
                record.Id = i;
                dict.Add(i, record);
            }
        }

		public static RDroneMission Get(int id)
		{
			return null;
		}

		public static Dictionary<int, RDroneMission> GetAll()
		{
            // Complete original0x469AC0.
            CheckInitialize();
            return dict;
        }
	}
}
