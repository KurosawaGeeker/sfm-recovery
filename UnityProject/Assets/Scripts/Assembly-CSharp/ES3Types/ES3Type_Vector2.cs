using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "x", "y" })]
	public class ES3Type_Vector2 : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x58ebc0.
		public ES3Type_Vector2()
			: base(typeof(UnityEngine.Vector2))
		{
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x58ED00
			UnityEngine.Vector2 value = (UnityEngine.Vector2)obj;
			writer.WriteProperty("x", value.x, ES3Type_float.Instance);
			writer.WriteProperty("y", value.y, ES3Type_float.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x907830
			return new UnityEngine.Vector2(reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance));
		}
	}
}
