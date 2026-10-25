using UnityEngine.Scripting;

namespace ES3Types
{
	[ES3Properties(new string[] { "alpha", "time" })]
	[Preserve]
	public class ES3Type_GradientAlphaKey : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x56acf0.
		public ES3Type_GradientAlphaKey()
			: base(typeof(UnityEngine.GradientAlphaKey))
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
