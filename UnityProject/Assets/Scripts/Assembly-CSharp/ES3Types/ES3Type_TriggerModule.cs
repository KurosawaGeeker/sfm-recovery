using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "enabled", "inside", "outside", "enter", "exit", "radiusScale" })]
	public class ES3Type_TriggerModule : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x58e620.
		public ES3Type_TriggerModule()
			: base(typeof(UnityEngine.ParticleSystem.TriggerModule))
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
