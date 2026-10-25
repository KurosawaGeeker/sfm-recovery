using UnityEngine.Scripting;

namespace ES3Types
{
	[ES3Properties(new string[]
	{
		"sprite", "overrideSprite", "type", "preserveAspect", "fillCenter", "fillMethod", "fillAmount", "fillClockwise", "fillOrigin", "alphaHitTestMinimumThreshold",
		"useSpriteMesh", "pixelsPerUnitMultiplier", "material", "onCullStateChanged", "maskable", "color", "raycastTarget", "useLegacyMeshGeneration", "useGUILayout", "enabled",
		"hideFlags"
	})]
	[Preserve]
	public class ES3Type_Image : ES3ComponentType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x55c380.
		public ES3Type_Image()
			: base(typeof(UnityEngine.UI.Image))
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
