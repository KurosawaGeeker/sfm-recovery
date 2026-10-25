namespace ES3Types
{
	public class ES3Type_PhysicsMaterial2DArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x583c30.
		public ES3Type_PhysicsMaterial2DArray()
			: base(typeof(UnityEngine.PhysicsMaterial2D[]), ES3Types.ES3Type_PhysicsMaterial2D.Instance)
		{
			Instance = this;
		}
	}
}
