namespace ES3Types
{
	public class ES3Type_DateTimeArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x555230.
		public ES3Type_DateTimeArray()
			: base(typeof(System.DateTime[]), ES3Types.ES3Type_DateTime.Instance)
		{
			Instance = this;
		}
	}
}
