using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	[ES3Properties(new string[] { "x", "y", "z", "w" })]
	[Preserve]
	public class ES3Type_Vector4 : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x58fcb0.
		public ES3Type_Vector4()
			: base(typeof(UnityEngine.Vector4))
		{
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x58FDF0
			UnityEngine.Vector4 value = (UnityEngine.Vector4)obj;
			writer.WriteProperty("x", value.x, ES3Type_float.Instance);
			writer.WriteProperty("y", value.y, ES3Type_float.Instance);
			writer.WriteProperty("z", value.z, ES3Type_float.Instance);
			writer.WriteProperty("w", value.w, ES3Type_float.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x90CF60
			return new UnityEngine.Vector4(reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance));
		}

		public static bool Equals(Vector4 a, Vector4 b)
		{
			return false;
		}
	}
}
