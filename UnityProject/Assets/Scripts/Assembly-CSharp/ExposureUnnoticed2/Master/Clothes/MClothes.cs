using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Clothes
{
	[CreateAssetMenu(fileName = "MClothes", menuName = "ScriptableObjects/MClothes")]
	public class MClothes : ScriptableObject
	{
		[SerializeField]
		private MClothesRecord[] clothes;

		private static Dictionary<ClothesType, MClothesRecord> dict;

		private static void CheckInitialize()
		{
			// Recovered from GameAssembly.dll RVA 0x46B930
			if (dict != null) return;
			dict = new Dictionary<ClothesType, MClothesRecord>();
			foreach (MClothesRecord record in global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MClothes.clothes)
			    dict.Add(record.ClothesType, record);
		}

		public static MClothesRecord Get(ClothesType type)
		{
			// Recovered from GameAssembly.dll RVA 0x46BB00
			CheckInitialize();
			return dict.TryGetValue(type, out MClothesRecord record) ? record : null;
		}

		public static Dictionary<ClothesType, MClothesRecord> GetAll()
		{
			// Recovered from GameAssembly.dll RVA 0x46BBA0
			CheckInitialize();
			return dict;
		}
	}
}
