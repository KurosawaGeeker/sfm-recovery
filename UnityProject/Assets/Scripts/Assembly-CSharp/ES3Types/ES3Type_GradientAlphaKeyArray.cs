namespace ES3Types
{
	public class ES3Type_GradientAlphaKeyArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x56afa0.
		public ES3Type_GradientAlphaKeyArray()
			: base(typeof(UnityEngine.GradientAlphaKey[]), ES3Types.ES3Type_GradientAlphaKey.Instance)
		{
			Instance = this;
		}
	}
}
