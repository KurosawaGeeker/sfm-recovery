namespace ES3Types
{
	public class ES3Type_BoneWeightArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x564690.
		public ES3Type_BoneWeightArray()
			: base(typeof(UnityEngine.BoneWeight[]), ES3Types.ES3Type_BoneWeight.Instance)
		{
			Instance = this;
		}
	}
}
