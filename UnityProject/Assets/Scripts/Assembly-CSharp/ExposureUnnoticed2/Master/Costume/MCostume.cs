using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Costume
{
	[CreateAssetMenu(fileName = "MCostume", menuName = "ScriptableObjects/MCostume")]
	public class MCostume : ScriptableObject
	{
		[SerializeField]
		private RCostume[] records;

		private static Dictionary<CostumeType, RCostume> dict;

		private static void CheckInitialize()
		{
            // Recovered complete original native body.
            if (dict != null) return;
            dict = new Dictionary<CostumeType, RCostume>();
            var source = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MCostume.records;
            for (int i = 0; i < source.Length; i++)
            {
                var record = source[i];
                dict.Add(record.Type, record);
            }
        }

		public static RCostume Get(CostumeType type)
		{
            // Recovered complete original native body.
            CheckInitialize();
            return dict.TryGetValue(type, out RCostume record) ? record : null;
        }

		public static Dictionary<CostumeType, RCostume> GetAll()
		{
			return null;
		}
	}
}
