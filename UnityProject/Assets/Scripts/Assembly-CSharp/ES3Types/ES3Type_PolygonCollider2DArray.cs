namespace ES3Types
{
	public class ES3Type_PolygonCollider2DArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x560780.
		public ES3Type_PolygonCollider2DArray()
			: base(typeof(UnityEngine.PolygonCollider2D[]), ES3Types.ES3Type_PolygonCollider2D.Instance)
		{
			Instance = this;
		}
	}
}
