using UnityEngine.Scripting;

namespace ES3Types
{
	[ES3Properties(new string[] { "enabled", "size", "sizeMultiplier", "x", "xMultiplier", "y", "yMultiplier", "z", "zMultiplier", "separateAxes" })]
	[Preserve]
	public class ES3Type_SizeOverLifetimeModule : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x5887b0.
		public ES3Type_SizeOverLifetimeModule()
			: base(typeof(UnityEngine.ParticleSystem.SizeOverLifetimeModule))
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
