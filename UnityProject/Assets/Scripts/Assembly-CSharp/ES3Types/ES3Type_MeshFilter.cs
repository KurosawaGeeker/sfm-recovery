using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "sharedMesh" })]
	public class ES3Type_MeshFilter : ES3ComponentType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x55d660.
		public ES3Type_MeshFilter()
			: base(typeof(UnityEngine.MeshFilter))
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
