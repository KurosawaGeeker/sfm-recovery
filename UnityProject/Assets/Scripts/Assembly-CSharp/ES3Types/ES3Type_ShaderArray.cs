namespace ES3Types
{
	public class ES3Type_ShaderArray : ES3ArrayType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x586f40.
		public ES3Type_ShaderArray()
			: base(typeof(UnityEngine.Shader[]), ES3Types.ES3Type_Shader.Instance)
		{
			Instance = this;
		}
	}
}
