using UnityEngine.Scripting;

namespace ES3Types
{
	[ES3Properties(new string[] { "color", "time" })]
	[Preserve]
	public class ES3Type_GradientColorKey : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x56b100.
		public ES3Type_GradientColorKey()
			: base(typeof(UnityEngine.GradientColorKey))
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
