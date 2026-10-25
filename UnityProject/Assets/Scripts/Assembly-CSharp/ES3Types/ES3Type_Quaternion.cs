using UnityEngine.Scripting;

namespace ES3Types
{
	[ES3Properties(new string[] { "x", "y", "z", "w" })]
	[Preserve]
	public class ES3Type_Quaternion : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x583d90.
		public ES3Type_Quaternion()
			: base(typeof(UnityEngine.Quaternion))
		{
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x583ED0
			UnityEngine.Quaternion value = (UnityEngine.Quaternion)obj;
			writer.WriteProperty("x", value.x, ES3Type_float.Instance);
			writer.WriteProperty("y", value.y, ES3Type_float.Instance);
			writer.WriteProperty("z", value.z, ES3Type_float.Instance);
			writer.WriteProperty("w", value.w, ES3Type_float.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x8700A0
			return new UnityEngine.Quaternion(reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance));
		}
	}
}
