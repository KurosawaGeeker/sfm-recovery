namespace ES3Types
{
	public class ES3Type_Vector3Array : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x58f6e0.
		public ES3Type_Vector3Array()
			: base(typeof(UnityEngine.Vector3[]), ES3Types.ES3Type_Vector3.Instance)
		{
			Instance = this;
		}
	}
}
