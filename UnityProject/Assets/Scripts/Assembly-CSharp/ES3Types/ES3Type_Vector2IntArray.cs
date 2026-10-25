namespace ES3Types
{
	public class ES3Type_Vector2IntArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x58f270.
		public ES3Type_Vector2IntArray()
			: base(typeof(UnityEngine.Vector2Int[]), ES3Types.ES3Type_Vector2Int.Instance)
		{
			Instance = this;
		}
	}
}
