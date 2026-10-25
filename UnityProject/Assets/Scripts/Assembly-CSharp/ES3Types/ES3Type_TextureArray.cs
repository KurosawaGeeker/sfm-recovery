namespace ES3Types
{
	public class ES3Type_TextureArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x58c460.
		public ES3Type_TextureArray()
			: base(typeof(UnityEngine.Texture[]), ES3Types.ES3Type_Texture.Instance)
		{
			Instance = this;
		}
	}
}
