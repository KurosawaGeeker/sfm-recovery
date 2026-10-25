using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "x", "y", "width", "height" })]
	public class ES3Type_Rect : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x584270.
		public ES3Type_Rect()
			: base(typeof(UnityEngine.Rect))
		{
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x5843B0
			UnityEngine.Rect value = (UnityEngine.Rect)obj;
			writer.WriteProperty("x", value.x, ES3Type_float.Instance);
			writer.WriteProperty("y", value.y, ES3Type_float.Instance);
			writer.WriteProperty("width", value.width, ES3Type_float.Instance);
			writer.WriteProperty("height", value.height, ES3Type_float.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x872900
			return new UnityEngine.Rect(reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance));
		}
	}
}
