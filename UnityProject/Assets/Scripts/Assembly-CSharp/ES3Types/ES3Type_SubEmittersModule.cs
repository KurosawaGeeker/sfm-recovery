using UnityEngine.Scripting;

namespace ES3Types
{
	[ES3Properties(new string[] { "properties", "systems", "types" })]
	[Preserve]
	public class ES3Type_SubEmittersModule : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x58bb90.
		public ES3Type_SubEmittersModule()
			: base(typeof(UnityEngine.ParticleSystem.SubEmittersModule))
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
