using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "enabled", "multiplier" })]
	public class ES3Type_ExternalForcesModule : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x567830.
		public ES3Type_ExternalForcesModule()
			: base(typeof(UnityEngine.ParticleSystem.ExternalForcesModule))
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
