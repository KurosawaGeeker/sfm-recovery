namespace ES3Types
{
	public class ES3Type_MaterialArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x570e60.
		public ES3Type_MaterialArray()
			: base(typeof(UnityEngine.Material[]), ES3Types.ES3Type_Material.Instance)
		{
			Instance = this;
		}
	}
}
