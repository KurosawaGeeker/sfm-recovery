namespace ES3Types
{
	public class ES3UserType_RigidbodyArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x562100.
		public ES3UserType_RigidbodyArray()
			: base(typeof(UnityEngine.Rigidbody[]), ES3Types.ES3Type_Rigidbody.Instance)
		{
			Instance = this;
		}
	}
}
