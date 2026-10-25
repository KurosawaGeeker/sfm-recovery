using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[]
	{
		"texture", "uvRect", "onCullStateChanged", "maskable", "color", "raycastTarget", "useLegacyMeshGeneration", "material", "useGUILayout", "enabled",
		"hideFlags"
	})]
	public class ES3Type_RawImage : ES3ComponentType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x5608e0.
		public ES3Type_RawImage()
			: base(typeof(UnityEngine.UI.RawImage))
		{
			isValueType = false;
			isES3TypeUnityObject = true;
			priority = 1;
			Instance = this;
		}

		protected override void WriteComponent(object obj, ES3Writer writer)
		{
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
		}
	}
}
