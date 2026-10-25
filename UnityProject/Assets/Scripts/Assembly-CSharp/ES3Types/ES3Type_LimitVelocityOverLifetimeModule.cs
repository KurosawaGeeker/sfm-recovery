using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[]
	{
		"enabled", "limitX", "limitXMultiplier", "limitY", "limitYMultiplier", "limitZ", "limitZMultiplier", "limit", "limitMultiplier", "dampen",
		"separateAxes", "space"
	})]
	public class ES3Type_LimitVelocityOverLifetimeModule : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x56ddb0.
		public ES3Type_LimitVelocityOverLifetimeModule()
			: base(typeof(UnityEngine.ParticleSystem.LimitVelocityOverLifetimeModule))
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
