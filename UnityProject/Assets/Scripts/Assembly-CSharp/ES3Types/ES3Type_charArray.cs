namespace ES3Types
{
	public class ES3Type_charArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x554e30.
		public ES3Type_charArray()
			: base(typeof(char[]), ES3Types.ES3Type_char.Instance)
		{
			Instance = this;
		}
	}
}
