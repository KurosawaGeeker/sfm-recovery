using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "enabled", "x", "xMultiplier", "y", "yMultiplier", "z", "zMultiplier", "separateAxes" })]
	public class ES3Type_RotationOverLifetimeModule : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x5864b0.
		public ES3Type_RotationOverLifetimeModule()
			: base(typeof(UnityEngine.ParticleSystem.RotationOverLifetimeModule))
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
