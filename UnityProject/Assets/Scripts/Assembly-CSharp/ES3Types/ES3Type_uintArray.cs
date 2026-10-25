namespace ES3Types
{
	public class ES3Type_uintArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x5589a0.
		public ES3Type_uintArray()
			: base(typeof(uint[]), ES3Types.ES3Type_uint.Instance)
		{
			Instance = this;
		}
	}
}
