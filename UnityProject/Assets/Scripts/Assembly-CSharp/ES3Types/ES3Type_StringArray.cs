namespace ES3Types
{
	public class ES3Type_StringArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x558670.
		public ES3Type_StringArray()
			: base(typeof(string[]), ES3Types.ES3Type_string.Instance)
		{
			Instance = this;
		}
	}
}
