namespace ES3Types
{
	public class ES3Type_Texture2DArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x58ce40.
		public ES3Type_Texture2DArray()
			: base(typeof(UnityEngine.Texture2D[]), ES3Types.ES3Type_Texture2D.Instance)
		{
			Instance = this;
		}
	}
}
