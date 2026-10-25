namespace ES3Types
{
	public class ES3Type_GuidArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x56b790.
		public ES3Type_GuidArray()
			: base(typeof(System.Guid[]), ES3Types.ES3Type_Guid.Instance)
		{
			Instance = this;
		}
	}
}
