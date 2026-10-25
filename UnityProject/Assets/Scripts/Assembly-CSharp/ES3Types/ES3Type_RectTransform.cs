using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[]
	{
		"anchorMin", "anchorMax", "anchoredPosition", "sizeDelta", "pivot", "offsetMin", "offsetMax", "localPosition", "localRotation", "localScale",
		"parent", "hideFlags"
	})]
	public class ES3Type_RectTransform : ES3ComponentType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x5845f0.
		public ES3Type_RectTransform()
			: base(typeof(UnityEngine.RectTransform))
		{
			isValueType = false;
			isES3TypeUnityObject = true;
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
