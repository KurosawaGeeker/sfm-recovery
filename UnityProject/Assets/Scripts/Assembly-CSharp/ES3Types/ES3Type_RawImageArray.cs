namespace ES3Types
{
	public class ES3Type_RawImageArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x561020.
		public ES3Type_RawImageArray()
			: base(typeof(UnityEngine.UI.RawImage[]), ES3Types.ES3Type_RawImage.Instance)
		{
			Instance = this;
		}
	}
}
