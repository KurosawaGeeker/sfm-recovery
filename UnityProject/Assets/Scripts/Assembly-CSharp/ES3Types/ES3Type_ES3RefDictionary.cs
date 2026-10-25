namespace ES3Types
{
	public class ES3Type_ES3RefDictionary : ES3DictionaryType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x556e70.
		public ES3Type_ES3RefDictionary()
			: base(typeof(System.Collections.Generic.Dictionary<ES3Ref, ES3Ref>), ES3Types.ES3Type_ES3Ref.Instance, ES3Types.ES3Type_ES3Ref.Instance)
		{
			Instance = this;
		}
	}
}
