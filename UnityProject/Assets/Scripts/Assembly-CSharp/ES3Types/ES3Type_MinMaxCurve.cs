using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "mode", "curveMultiplier", "curveMax", "curveMin", "constantMax", "constantMin", "constant", "curve" })]
	public class ES3Type_MinMaxCurve : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x5726a0.
		public ES3Type_MinMaxCurve()
			: base(typeof(UnityEngine.ParticleSystem.MinMaxCurve))
		{
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
		}

		[Preserve]
		public override object Read<T>(ES3Reader reader)
		{
			return null;
		}

		[Preserve]
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
		}
	}
}
