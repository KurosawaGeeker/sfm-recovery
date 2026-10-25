using UnityEngine.Scripting;

namespace ES3Types
{
	[ES3Properties(new string[] { "sharedMesh", "convex", "inflateMesh", "skinWidth", "enabled", "isTrigger", "contactOffset", "sharedMaterial" })]
	[Preserve]
	public class ES3Type_MeshCollider : ES3ComponentType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x55cf70.
		public ES3Type_MeshCollider()
			: base(typeof(UnityEngine.MeshCollider))
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
