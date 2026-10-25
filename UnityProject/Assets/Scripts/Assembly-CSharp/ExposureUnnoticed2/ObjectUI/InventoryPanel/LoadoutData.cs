using System.Collections.Generic;
using ExposureUnnoticed2.Master.Cosplay;
using ExposureUnnoticed2.Master.Costume;

namespace ExposureUnnoticed2.ObjectUI.InventoryPanel
{
	public class LoadoutData
	{
		[ES3Serializable]
		public List<int> CosplayPartsList;

		[ES3Serializable]
		public List<CostumeType> CostumeList;

		[ES3Serializable]
		public Dictionary<int, CosplayPartsOptionData> CosplayPartsOptionDataDict;
	// Recovered original member RVA 0x4EFA60: .ctor
		public LoadoutData()
        {
            CosplayPartsList = new List<int>();
            CostumeList = new List<CostumeType>();
            CosplayPartsOptionDataDict = new Dictionary<int, CosplayPartsOptionData>();
        }
	}
}
