namespace ES3Types
{
	public class ES3Type_ImageArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x55ce10.
		public ES3Type_ImageArray()
			: base(typeof(UnityEngine.UI.Image[]), ES3Types.ES3Type_Image.Instance)
		{
			Instance = this;
		}
	}
}
