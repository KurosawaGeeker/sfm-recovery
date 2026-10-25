using UnityEngine.Scripting;

namespace ES3Types
{
	[ES3Properties(new string[] { "keys", "preWrapMode", "postWrapMode" })]
	[Preserve]
	public class ES3Type_AnimationCurve : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x5637a0.
		public ES3Type_AnimationCurve()
			: base(typeof(UnityEngine.AnimationCurve))
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
