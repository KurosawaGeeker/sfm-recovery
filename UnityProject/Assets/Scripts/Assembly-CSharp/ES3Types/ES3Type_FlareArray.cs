namespace ES3Types
{
	public class ES3Type_FlareArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x567de0.
		public ES3Type_FlareArray()
			: base(typeof(UnityEngine.Flare[]), ES3Types.ES3Type_Flare.Instance)
		{
			Instance = this;
		}
	}
}
