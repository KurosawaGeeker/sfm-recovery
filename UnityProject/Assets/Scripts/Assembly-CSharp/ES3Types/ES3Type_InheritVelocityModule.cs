using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "enabled", "mode", "curve", "curveMultiplier" })]
	public class ES3Type_InheritVelocityModule : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x56b8f0.
		public ES3Type_InheritVelocityModule()
			: base(typeof(UnityEngine.ParticleSystem.InheritVelocityModule))
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
