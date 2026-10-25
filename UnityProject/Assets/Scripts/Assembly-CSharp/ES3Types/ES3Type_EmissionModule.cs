using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "enabled", "rateOverTime", "rateOverTimeMultiplier", "rateOverDistance", "rateOverDistanceMultiplier" })]
	public class ES3Type_EmissionModule : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x5671c0.
		public ES3Type_EmissionModule()
			: base(typeof(UnityEngine.ParticleSystem.EmissionModule))
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
