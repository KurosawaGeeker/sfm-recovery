namespace ES3Types
{
	public class ES3Type_longArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x557ce0.
		public ES3Type_longArray()
			: base(typeof(long[]), ES3Types.ES3Type_long.Instance)
		{
			Instance = this;
		}
	}
}
