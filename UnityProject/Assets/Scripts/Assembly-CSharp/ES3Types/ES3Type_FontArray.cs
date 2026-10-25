namespace ES3Types
{
	public class ES3Type_FontArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x5681e0.
		public ES3Type_FontArray()
			: base(typeof(UnityEngine.Font[]), ES3Types.ES3Type_Font.Instance)
		{
			Instance = this;
		}
	}
}
