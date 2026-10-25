using UnityEngine.Scripting;

namespace ES3Types
{
	[ES3Properties(new string[] { "x", "y", "z" })]
	[Preserve]
	public class ES3Type_Vector3 : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x58f3d0.
		public ES3Type_Vector3()
			: base(typeof(UnityEngine.Vector3))
		{
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x58F510
			UnityEngine.Vector3 value = (UnityEngine.Vector3)obj;
			writer.WriteProperty("x", value.x, ES3Type_float.Instance);
			writer.WriteProperty("y", value.y, ES3Type_float.Instance);
			writer.WriteProperty("z", value.z, ES3Type_float.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x909E60
			return new UnityEngine.Vector3(reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance));
		}
	}
}
