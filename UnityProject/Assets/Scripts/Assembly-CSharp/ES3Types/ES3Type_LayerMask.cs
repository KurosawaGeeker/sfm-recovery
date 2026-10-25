using UnityEngine.Scripting;

namespace ES3Types
{
	[ES3Properties(new string[] { "colorKeys", "alphaKeys", "mode" })]
	[Preserve]
	public class ES3Type_LayerMask : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x56c2c0.
		public ES3Type_LayerMask()
			: base(typeof(UnityEngine.LayerMask))
		{
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x7D2210
			UnityEngine.LayerMask value = default;
			string property;
			while ((property = reader.ReadPropertyName()) != null)
			{
			    if (property == "value") value.value = reader.Read<int>(ES3Type_int.Instance);
			    else reader.Skip();
			}
			return value;
		}
	}
}
