namespace ES3Types
{
	public class ES3Type_ES3PrefabInternal : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x549950.
		public ES3Type_ES3PrefabInternal()
			: base(typeof(ES3Types.ES3Type_ES3PrefabInternal))
		{
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
		}

		public override object Read<T>(ES3Reader reader)
		{
			return null;
		}

		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
		}
	}
}
