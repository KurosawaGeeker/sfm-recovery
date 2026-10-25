using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3Type_IntPtr : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x5577e0.
		public ES3Type_IntPtr()
			: base(typeof(System.IntPtr))
		{
			isPrimitive = true;
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x7cebc0
			return (T)(object)(new System.IntPtr(reader.Read_long()));
		}
	}
}
