using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "r", "g", "b", "a" })]
	public class ES3Type_Color : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x565fb0.
		public ES3Type_Color()
			: base(typeof(UnityEngine.Color))
		{
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x5660F0
			UnityEngine.Color value = (UnityEngine.Color)obj;
			writer.WriteProperty("r", value.r, ES3Type_float.Instance);
			writer.WriteProperty("g", value.g, ES3Type_float.Instance);
			writer.WriteProperty("b", value.b, ES3Type_float.Instance);
			writer.WriteProperty("a", value.a, ES3Type_float.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x78E1C0
			return new UnityEngine.Color(reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance));
		}
	}
}
