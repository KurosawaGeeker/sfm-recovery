using UnityEngine.Scripting;

namespace ES3Types
{
	[ES3Properties(new string[] { "enabled", "x", "y", "z", "xMultiplier", "yMultiplier", "zMultiplier", "space" })]
	[Preserve]
	public class ES3Type_VelocityOverLifetimeModule : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x590200.
		public ES3Type_VelocityOverLifetimeModule()
			: base(typeof(UnityEngine.ParticleSystem.VelocityOverLifetimeModule))
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
