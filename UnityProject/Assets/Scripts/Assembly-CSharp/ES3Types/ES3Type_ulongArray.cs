namespace ES3Types
{
	public class ES3Type_ulongArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x559000.
		public ES3Type_ulongArray()
			: base(typeof(ulong[]), ES3Types.ES3Type_ulong.Instance)
		{
			Instance = this;
		}
	}
}
