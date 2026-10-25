using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "hideFlags" })]
	public class ES3Type_Flare : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x567b80.
		public ES3Type_Flare()
			: base(typeof(UnityEngine.Flare))
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
