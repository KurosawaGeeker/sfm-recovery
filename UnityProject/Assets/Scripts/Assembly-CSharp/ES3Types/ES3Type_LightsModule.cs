using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[]
	{
		"enabled", "ratio", "useRandomDistribution", "light", "useParticleColor", "sizeAffectsRange", "alphaAffectsIntensity", "range", "rangeMultiplier", "intensity",
		"intensityMultiplier", "maxLights"
	})]
	public class ES3Type_LightsModule : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x56d3b0.
		public ES3Type_LightsModule()
			: base(typeof(UnityEngine.ParticleSystem.LightsModule))
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
