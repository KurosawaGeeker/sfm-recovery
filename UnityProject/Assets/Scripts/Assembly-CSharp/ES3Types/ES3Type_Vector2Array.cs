namespace ES3Types
{
	public class ES3Type_Vector2Array : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x58ee70.
		public ES3Type_Vector2Array()
			: base(typeof(UnityEngine.Vector2[]), ES3Types.ES3Type_Vector2.Instance)
		{
			Instance = this;
		}
	}
}
