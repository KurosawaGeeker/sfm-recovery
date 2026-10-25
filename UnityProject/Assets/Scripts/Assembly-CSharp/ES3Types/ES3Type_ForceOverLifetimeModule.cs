using UnityEngine.Scripting;

namespace ES3Types
{
	[ES3Properties(new string[] { "enabled", "x", "y", "z", "xMultiplier", "yMultiplier", "zMultiplier", "space", "randomized" })]
	[Preserve]
	public class ES3Type_ForceOverLifetimeModule : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x568340.
		public ES3Type_ForceOverLifetimeModule()
			: base(typeof(UnityEngine.ParticleSystem.ForceOverLifetimeModule))
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
