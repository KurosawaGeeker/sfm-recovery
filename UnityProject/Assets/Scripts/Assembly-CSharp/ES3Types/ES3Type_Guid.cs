using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "value" })]
	public class ES3Type_Guid : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x56b530.
		public ES3Type_Guid()
			: base(typeof(System.Guid))
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
