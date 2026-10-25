namespace ES3Types
{
	public class ES3Type_shortArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x558340.
		public ES3Type_shortArray()
			: base(typeof(short[]), ES3Types.ES3Type_short.Instance)
		{
			Instance = this;
		}
	}
}
