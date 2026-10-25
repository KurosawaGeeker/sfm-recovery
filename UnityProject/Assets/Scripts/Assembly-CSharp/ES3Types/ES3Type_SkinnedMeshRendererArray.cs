namespace ES3Types
{
	public class ES3Type_SkinnedMeshRendererArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x58a340.
		public ES3Type_SkinnedMeshRendererArray()
			: base(typeof(UnityEngine.SkinnedMeshRenderer[]), ES3Types.ES3Type_SkinnedMeshRenderer.Instance)
		{
			Instance = this;
		}
	}
}
