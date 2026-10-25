namespace ES3Types
{
	public class ES3Type_MeshRendererArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x55e6b0.
		public ES3Type_MeshRendererArray()
			: base(typeof(UnityEngine.MeshRenderer[]), ES3Types.ES3Type_MeshRenderer.Instance)
		{
			Instance = this;
		}
	}
}
