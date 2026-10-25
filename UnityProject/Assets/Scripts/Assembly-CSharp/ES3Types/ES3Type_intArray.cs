namespace ES3Types
{
	public class ES3Type_intArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x557680.
		public ES3Type_intArray()
			: base(typeof(int[]), ES3Types.ES3Type_int.Instance)
		{
			Instance = this;
		}
	}
}
