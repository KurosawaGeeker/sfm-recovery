namespace ES3Types
{
	public class ES3Type_BurstArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x5651d0.
		public ES3Type_BurstArray()
			: base(typeof(UnityEngine.ParticleSystem.Burst[]), ES3Types.ES3Type_Burst.Instance)
		{
			Instance = this;
		}
	}
}
