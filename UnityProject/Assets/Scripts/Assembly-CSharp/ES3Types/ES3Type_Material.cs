using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "shader", "renderQueue", "shaderKeywords", "globalIlluminationFlags", "properties" })]
	public class ES3Type_Material : ES3UnityObjectType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x570310.
		public ES3Type_Material()
			: base(typeof(UnityEngine.Material))
		{
			isValueType = false;
			isES3TypeUnityObject = true;
			Instance = this;
		}

		protected override void WriteUnityObject(object obj, ES3Writer writer)
		{
		}

		protected override object ReadUnityObject<T>(ES3Reader reader)
		{
			return null;
		}

		protected override void ReadUnityObject<T>(ES3Reader reader, object obj)
		{
		}
	}
}
