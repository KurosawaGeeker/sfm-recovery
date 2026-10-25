using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "x", "y", "z" })]
	public class ES3Type_Vector3Int : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x58f840.
		public ES3Type_Vector3Int()
			: base(typeof(UnityEngine.Vector3Int))
		{
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x58F980
			UnityEngine.Vector3Int value = (UnityEngine.Vector3Int)obj;
			writer.WriteProperty("x", value.x, ES3Type_int.Instance);
			writer.WriteProperty("y", value.y, ES3Type_int.Instance);
			writer.WriteProperty("z", value.z, ES3Type_int.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x90B560
			return new UnityEngine.Vector3Int(reader.ReadProperty<int>(ES3Type_int.Instance), reader.ReadProperty<int>(ES3Type_int.Instance), reader.ReadProperty<int>(ES3Type_int.Instance));
		}
	}
}
