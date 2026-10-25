namespace ES3Types
{
	public class ES3Type_SpriteRendererArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x58ba30.
		public ES3Type_SpriteRendererArray()
			: base(typeof(UnityEngine.SpriteRenderer[]), ES3Types.ES3Type_SpriteRenderer.Instance)
		{
			Instance = this;
		}
	}
}
