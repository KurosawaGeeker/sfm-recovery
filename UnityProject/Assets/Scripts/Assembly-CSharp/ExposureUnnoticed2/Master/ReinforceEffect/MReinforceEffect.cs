using System.Collections.Generic;
using ExposureUnnoticed2.ObjectUI.BuffPanel;
using UnityEngine;

namespace ExposureUnnoticed2.Master.ReinforceEffect
{
	[CreateAssetMenu(fileName = "MReinforceEffect", menuName = "ScriptableObjects/MReinforceEffect")]
	public class MReinforceEffect : ScriptableObject
	{
		public const int NoEffectId = 0;

		[SerializeField]
		private RReinforceEffect[] records;

		private static Dictionary<int, RReinforceEffect> dict;

		private static Dictionary<ReinforceEffectType, int> typeIsNegativeDict;

		private static void CheckInitialize()
		{
			// Recovered from GameAssembly.dll RVA 0x465C70
			if (dict != null) return;
			dict = new Dictionary<int, RReinforceEffect>();
			typeIsNegativeDict = new Dictionary<ReinforceEffectType, int>();
			int id = 1;
			foreach (var record in global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MReinforceEffect.records)
			{
			    record.Id = id;
			    dict.Add(id, record);
			    if (!typeIsNegativeDict.ContainsKey(record.ReinforceEffectType))
			    {
			        int direction = 1;
			        if (record.IsNeutralEffect) direction = 0;
			        else if (record.IsNegativeEffect)
			        {
			            if (record.EffectAmount100 > 0) direction = -1;
			        }
			        else if (record.EffectAmount100 < 0) direction = -1;
			        typeIsNegativeDict.Add(record.ReinforceEffectType, direction);
			    }
			    id = unchecked(id + 1);
			}
		}

		public static RReinforceEffect Get(int id)
		{
			// Recovered from GameAssembly.dll RVA 0x466020
			CheckInitialize();
			return dict.TryGetValue(id, out RReinforceEffect record) ? record : null;
		}

		public static int IsPlusNegativeEffect(ReinforceEffectType type)
		{
			// Recovered from GameAssembly.dll RVA 0x4660C0
			CheckInitialize();
			return typeIsNegativeDict[type];
		}

		public static Dictionary<int, RReinforceEffect> GetAll()
		{
			// Recovered from GameAssembly.dll RVA 0x466140
			CheckInitialize();
			return dict;
		}

		public static ReinforceEffectType ToReinforceEffectType(BuffPanelItemView.BuffType type)
		{
			return ReinforceEffectType.RpBonus;
		}
	}
}
