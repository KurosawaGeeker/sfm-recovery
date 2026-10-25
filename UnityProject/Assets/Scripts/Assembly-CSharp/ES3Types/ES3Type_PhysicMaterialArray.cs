namespace ES3Types
{
	public class ES3Type_PhysicMaterialArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x5837a0.
		public ES3Type_PhysicMaterialArray()
			: base(typeof(UnityEngine.PhysicMaterial[]), ES3Types.ES3Type_PhysicMaterial.Instance)
		{
			Instance = this;
		}
	}
}
