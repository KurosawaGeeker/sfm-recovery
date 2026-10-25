namespace ES3Types
{
	public class ES3Type_decimalArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x555560.
		public ES3Type_decimalArray()
			: base(typeof(System.Decimal[]), ES3Types.ES3Type_decimal.Instance)
		{
			Instance = this;
		}
	}
}
