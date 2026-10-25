namespace ES3Types
{
	public class ES3Type_ColorArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x566330.
		public ES3Type_ColorArray()
			: base(typeof(UnityEngine.Color[]), ES3Types.ES3Type_Color.Instance)
		{
			Instance = this;
		}
	}
}
