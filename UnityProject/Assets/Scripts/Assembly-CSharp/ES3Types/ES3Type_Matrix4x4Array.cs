namespace ES3Types
{
	public class ES3Type_Matrix4x4Array : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x571430.
		public ES3Type_Matrix4x4Array()
			: base(typeof(UnityEngine.Matrix4x4[]), ES3Types.ES3Type_Matrix4x4.Instance)
		{
			Instance = this;
		}
	}
}
