namespace ExposureUnnoticed2.Master.Cosplay
{
	public class CosplayPartsOptionData
	{
		[ES3Serializable]
		public int r;

		[ES3Serializable]
		public int g;

		[ES3Serializable]
		public int b;

		[ES3Serializable]
		public int shadeColor;

		[ES3Serializable]
		public int materialIndex;

		[ES3Serializable]
		public bool boolOption1;

		[ES3Serializable]
		public bool boolOption2;

		[ES3Serializable]
		public bool invisible;

		private int uniqueId;

		public CosplayPartsOptionData(int uniqueId)
		{
			// Recovered from GameAssembly.dll RVA 0x46AEB0
			this.uniqueId = uniqueId;
			b = 255;
			g = 255;
			r = 255;
			shadeColor = 0;
		}

		public CosplayPartsOptionData(int uniqueId, CosplayPartsOptionData original)
		{
			// Recovered from GameAssembly.dll RVA 0x46AED0
			this.uniqueId = uniqueId;
			r = original.r;
			g = original.g;
			b = original.b;
			shadeColor = original.shadeColor;
			materialIndex = original.materialIndex;
			boolOption1 = original.boolOption1;
			boolOption2 = original.boolOption2;
			invisible = original.invisible;
		}
	}
}
