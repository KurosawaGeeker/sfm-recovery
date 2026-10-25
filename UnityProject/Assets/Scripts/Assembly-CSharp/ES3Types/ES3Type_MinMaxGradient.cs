using UnityEngine.Scripting;

namespace ES3Types
{
	[ES3Properties(new string[] { "mode", "gradientMax", "gradientMin", "colorMax", "colorMin", "color", "gradient" })]
	[Preserve]
	public class ES3Type_MinMaxGradient : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x572b80.
		public ES3Type_MinMaxGradient()
			: base(typeof(UnityEngine.ParticleSystem.MinMaxGradient))
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
