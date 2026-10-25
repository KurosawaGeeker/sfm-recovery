namespace ES3Types
{
	public class ES3Type_boolArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x554750.
		public ES3Type_boolArray()
			: base(typeof(bool[]), ES3Types.ES3Type_bool.Instance)
		{
			Instance = this;
		}
	}
}
