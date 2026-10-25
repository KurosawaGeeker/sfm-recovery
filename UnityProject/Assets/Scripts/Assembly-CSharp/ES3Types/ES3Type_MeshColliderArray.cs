namespace ES3Types
{
	public class ES3Type_MeshColliderArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x55d500.
		public ES3Type_MeshColliderArray()
			: base(typeof(UnityEngine.MeshCollider[]), ES3Types.ES3Type_MeshCollider.Instance)
		{
			Instance = this;
		}
	}
}
