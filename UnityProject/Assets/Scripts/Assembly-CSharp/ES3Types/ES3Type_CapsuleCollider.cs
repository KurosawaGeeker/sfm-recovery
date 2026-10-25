using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "center", "radius", "height", "direction", "enabled", "isTrigger", "contactOffset", "sharedMaterial" })]
	public class ES3Type_CapsuleCollider : ES3ComponentType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x55bae0.
		public ES3Type_CapsuleCollider()
			: base(typeof(UnityEngine.CapsuleCollider))
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
