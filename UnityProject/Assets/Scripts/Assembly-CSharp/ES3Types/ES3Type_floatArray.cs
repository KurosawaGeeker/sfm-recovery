namespace ES3Types
{
	public class ES3Type_floatArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x557350.
		public ES3Type_floatArray()
			: base(typeof(float[]), ES3Types.ES3Type_float.Instance)
		{
			Instance = this;
		}
	}
}
