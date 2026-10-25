namespace ES3Types
{
	public class ES3Type_GameObjectArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x56a7e0.
		public ES3Type_GameObjectArray()
			: base(typeof(UnityEngine.GameObject[]), ES3Types.ES3Type_GameObject.Instance)
		{
			Instance = this;
		}
	}
}
