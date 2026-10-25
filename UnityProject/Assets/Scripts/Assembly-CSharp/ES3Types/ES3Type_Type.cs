using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { })]
	public class ES3Type_Type : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x554370.
		public ES3Type_Type()
			: base(typeof(System.Type))
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
	}
}
