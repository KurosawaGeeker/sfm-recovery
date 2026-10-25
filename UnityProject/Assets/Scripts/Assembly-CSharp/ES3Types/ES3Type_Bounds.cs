using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "center", "size" })]
	public class ES3Type_Bounds : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x5647f0.
		public ES3Type_Bounds()
			: base(typeof(UnityEngine.Bounds))
		{
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x564930
			UnityEngine.Bounds value = (UnityEngine.Bounds)obj;
			writer.WriteProperty("center", value.center, ES3Type_Vector3.Instance);
			writer.WriteProperty("size", value.size, ES3Type_Vector3.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x76AFC0
			return new UnityEngine.Bounds(reader.ReadProperty<UnityEngine.Vector3>(ES3Type_Vector3.Instance), reader.ReadProperty<UnityEngine.Vector3>(ES3Type_Vector3.Instance));
		}
	}
}
