namespace ES3Types
{
	public class ES3Type_Color32Array : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x566840.
		public ES3Type_Color32Array()
			: base(typeof(UnityEngine.Color32[]), ES3Types.ES3Type_Color32.Instance)
		{
			Instance = this;
		}
	}
}
