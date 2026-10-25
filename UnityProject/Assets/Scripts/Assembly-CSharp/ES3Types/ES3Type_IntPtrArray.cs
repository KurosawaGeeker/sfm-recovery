namespace ES3Types
{
	public class ES3Type_IntPtrArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x5579b0.
		public ES3Type_IntPtrArray()
			: base(typeof(System.IntPtr[]), ES3Types.ES3Type_IntPtr.Instance)
		{
			Instance = this;
		}
	}
}
