namespace ES3Types
{
	public class ES3Type_GradientColorKeyArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x56b3d0.
		public ES3Type_GradientColorKeyArray()
			: base(typeof(UnityEngine.GradientColorKey[]), ES3Types.ES3Type_GradientColorKey.Instance)
		{
			Instance = this;
		}
	}
}
