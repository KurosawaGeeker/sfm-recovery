namespace ES3Types
{
	public class ES3Type_sbyteArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x558010.
		public ES3Type_sbyteArray()
			: base(typeof(sbyte[]), ES3Types.ES3Type_sbyte.Instance)
		{
			Instance = this;
		}
	}
}
