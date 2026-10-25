namespace ES3Types
{
	public class ES3Type_AudioClipArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x563ff0.
		public ES3Type_AudioClipArray()
			: base(typeof(UnityEngine.AudioClip[]), ES3Types.ES3Type_AudioClip.Instance)
		{
			Instance = this;
		}
	}
}
