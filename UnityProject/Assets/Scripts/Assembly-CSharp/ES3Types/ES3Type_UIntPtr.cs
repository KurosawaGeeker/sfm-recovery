using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3Type_UIntPtr : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x558b00.
		public ES3Type_UIntPtr()
			: base(typeof(System.UIntPtr))
		{
			isPrimitive = true;
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x9069a0
			return reader.Read_ulong();
		}
	}
}
