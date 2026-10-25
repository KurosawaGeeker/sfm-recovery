using UnityEngine.Scripting;

namespace ES3Types
{
	[ES3Properties(new string[] { "time", "value", "inTangent", "outTangent" })]
	[Preserve]
	public class ES3Type_Keyframe : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x56bde0.
		public ES3Type_Keyframe()
			: base(typeof(UnityEngine.Keyframe))
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
