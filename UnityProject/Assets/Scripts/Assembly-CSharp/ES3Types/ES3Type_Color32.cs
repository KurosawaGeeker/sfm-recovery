using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "r", "g", "b", "a" })]
	public class ES3Type_Color32 : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x566490.
		public ES3Type_Color32()
			: base(typeof(UnityEngine.Color32))
		{
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x5665D0
			UnityEngine.Color32 value = (UnityEngine.Color32)obj;
			writer.WriteProperty("r", value.r, ES3Type_byte.Instance);
			writer.WriteProperty("g", value.g, ES3Type_byte.Instance);
			writer.WriteProperty("b", value.b, ES3Type_byte.Instance);
			writer.WriteProperty("a", value.a, ES3Type_byte.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x78FC40
			return new UnityEngine.Color32(reader.ReadProperty<byte>(ES3Type_byte.Instance), reader.ReadProperty<byte>(ES3Type_byte.Instance), reader.ReadProperty<byte>(ES3Type_byte.Instance), reader.ReadProperty<byte>(ES3Type_byte.Instance));
		}

		public static bool Equals(Color32 a, Color32 b)
		{
			return false;
		}
	}
}
