namespace ES3Types
{
	public class ES3Type_ushortArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x559330.
		public ES3Type_ushortArray()
			: base(typeof(ushort[]), ES3Types.ES3Type_ushort.Instance)
		{
			Instance = this;
		}
	}
}
