namespace ES3Types
{
	public class ES3Type_KeyframeArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x56c160.
		public ES3Type_KeyframeArray()
			: base(typeof(UnityEngine.Keyframe[]), ES3Types.ES3Type_Keyframe.Instance)
		{
			Instance = this;
		}
	}
}
