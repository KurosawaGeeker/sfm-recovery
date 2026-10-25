namespace ES3Types
{
	public class ES3Type_BoundsArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x564ae0.
		public ES3Type_BoundsArray()
			: base(typeof(UnityEngine.Bounds[]), ES3Types.ES3Type_Bounds.Instance)
		{
			Instance = this;
		}
	}
}
