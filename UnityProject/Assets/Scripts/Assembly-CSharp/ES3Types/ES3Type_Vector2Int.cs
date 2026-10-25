using UnityEngine.Scripting;

namespace ES3Types
{
	[ES3Properties(new string[] { "x", "y" })]
	[Preserve]
	public class ES3Type_Vector2Int : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x58efd0.
		public ES3Type_Vector2Int()
			: base(typeof(UnityEngine.Vector2Int))
		{
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x58F110
			UnityEngine.Vector2Int value = (UnityEngine.Vector2Int)obj;
			writer.WriteProperty("x", value.x, ES3Type_int.Instance);
			writer.WriteProperty("y", value.y, ES3Type_int.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x908A20
			return new UnityEngine.Vector2Int(reader.ReadProperty<int>(ES3Type_int.Instance), reader.ReadProperty<int>(ES3Type_int.Instance));
		}
	}
}
