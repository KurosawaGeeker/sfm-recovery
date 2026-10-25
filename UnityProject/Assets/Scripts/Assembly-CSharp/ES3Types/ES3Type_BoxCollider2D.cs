using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "size", "density", "isTrigger", "usedByEffector", "offset", "sharedMaterial", "enabled" })]
	public class ES3Type_BoxCollider2D : ES3ComponentType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x559f70.
		public ES3Type_BoxCollider2D()
			: base(typeof(UnityEngine.BoxCollider2D))
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
