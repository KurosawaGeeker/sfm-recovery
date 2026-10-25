namespace ES3Types
{
	public class ES3Type_Vector3IntArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x58fb50.
		public ES3Type_Vector3IntArray()
			: base(typeof(UnityEngine.Vector3Int[]), ES3Types.ES3Type_Vector3Int.Instance)
		{
			Instance = this;
		}
	}
}
