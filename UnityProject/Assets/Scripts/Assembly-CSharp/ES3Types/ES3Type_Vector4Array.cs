namespace ES3Types
{
	public class ES3Type_Vector4Array : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x5900a0.
		public ES3Type_Vector4Array()
			: base(typeof(UnityEngine.Vector4[]), ES3Types.ES3Type_Vector4.Instance)
		{
			Instance = this;
		}
	}
}
