using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "center", "size", "enabled", "isTrigger", "contactOffset", "sharedMaterial" })]
	public class ES3Type_BoxCollider : ES3ComponentType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x5599f0.
		public ES3Type_BoxCollider()
			: base(typeof(UnityEngine.BoxCollider))
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
