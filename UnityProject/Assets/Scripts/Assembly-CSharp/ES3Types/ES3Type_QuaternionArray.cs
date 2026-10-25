namespace ES3Types
{
	public class ES3Type_QuaternionArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x584110.
		public ES3Type_QuaternionArray()
			: base(typeof(UnityEngine.Quaternion[]), ES3Types.ES3Type_Quaternion.Instance)
		{
			Instance = this;
		}
	}
}
