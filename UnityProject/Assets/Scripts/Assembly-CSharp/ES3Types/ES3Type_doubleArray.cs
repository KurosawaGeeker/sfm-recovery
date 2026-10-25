namespace ES3Types
{
	public class ES3Type_doubleArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x555890.
		public ES3Type_doubleArray()
			: base(typeof(double[]), ES3Types.ES3Type_double.Instance)
		{
			Instance = this;
		}
	}
}
