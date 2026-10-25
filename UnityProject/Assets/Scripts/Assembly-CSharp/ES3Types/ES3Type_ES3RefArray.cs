namespace ES3Types
{
	public class ES3Type_ES3RefArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x556ab0.
		public ES3Type_ES3RefArray()
			: base(typeof(ES3Ref[]), ES3Types.ES3Type_ES3Ref.Instance)
		{
			Instance = this;
		}
	}
}
