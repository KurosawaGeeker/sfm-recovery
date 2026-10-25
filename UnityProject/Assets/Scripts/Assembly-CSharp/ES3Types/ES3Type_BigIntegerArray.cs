namespace ES3Types
{
	public class ES3Type_BigIntegerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x554210.
		public ES3Type_BigIntegerArray()
			: base(typeof(System.Numerics.BigInteger[]), ES3Types.ES3Type_BigInteger.Instance)
		{
			Instance = this;
		}
	}
}
