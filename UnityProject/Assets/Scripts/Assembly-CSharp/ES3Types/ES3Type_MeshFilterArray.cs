namespace ES3Types
{
	public class ES3Type_MeshFilterArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x55d890.
		public ES3Type_MeshFilterArray()
			: base(typeof(UnityEngine.MeshFilter[]), ES3Types.ES3Type_MeshFilter.Instance)
		{
			Instance = this;
		}
	}
}
