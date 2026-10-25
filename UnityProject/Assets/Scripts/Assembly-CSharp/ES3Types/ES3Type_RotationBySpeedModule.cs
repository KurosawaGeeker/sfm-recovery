using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "enabled", "x", "xMultiplier", "y", "yMultiplier", "z", "zMultiplier", "separateAxes", "range" })]
	public class ES3Type_RotationBySpeedModule : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x585c40.
		public ES3Type_RotationBySpeedModule()
			: base(typeof(UnityEngine.ParticleSystem.RotationBySpeedModule))
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
