using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	[ES3Properties(new string[] { "filterMode", "anisoLevel", "wrapMode", "mipMapBias", "rawTextureData" })]
	[Preserve]
	public class ES3Type_Texture2D : ES3UnityObjectType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x58c5c0.
		public ES3Type_Texture2D()
			: base(typeof(UnityEngine.Texture2D))
		{
			isValueType = false;
			isES3TypeUnityObject = true;
			Instance = this;
		}

		protected override void WriteUnityObject(object obj, ES3Writer writer)
		{
		}

		protected override void ReadUnityObject<T>(ES3Reader reader, object obj)
		{
		}

		protected override object ReadUnityObject<T>(ES3Reader reader)
		{
			return null;
		}

		protected bool IsReadable(Texture2D instance)
		{
			return false;
		}
	}
}
