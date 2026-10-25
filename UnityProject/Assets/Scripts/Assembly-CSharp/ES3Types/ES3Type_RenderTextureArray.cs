namespace ES3Types
{
	public class ES3Type_RenderTextureArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x585ae0.
		public ES3Type_RenderTextureArray()
			: base(typeof(UnityEngine.RenderTexture[]), ES3Types.ES3Type_RenderTexture.Instance)
		{
			Instance = this;
		}
	}
}
