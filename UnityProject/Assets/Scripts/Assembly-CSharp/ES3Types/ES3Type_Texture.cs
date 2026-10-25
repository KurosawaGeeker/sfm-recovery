using UnityEngine.Scripting;

namespace ES3Types
{
	[ES3Properties(new string[] { "filterMode", "anisoLevel", "wrapMode", "mipMapBias", "rawTextureData" })]
	[Preserve]
	public class ES3Type_Texture : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x58c130.
		public ES3Type_Texture()
			: base(typeof(UnityEngine.Texture))
		{
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
		}

		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
		}

		public override object Read<T>(ES3Reader reader)
		{
			return null;
		}
	}
}
