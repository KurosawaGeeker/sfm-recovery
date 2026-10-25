namespace ES3Types
{
	public class ES3Type_UIntPtrArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x558cd0.
		public ES3Type_UIntPtrArray()
			: base(typeof(System.UIntPtr[]), ES3Types.ES3Type_UIntPtr.Instance)
		{
			Instance = this;
		}
	}
}
