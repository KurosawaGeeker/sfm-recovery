using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3Type_ES3Ref : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x556630.
		public ES3Type_ES3Ref()
			: base(typeof(long))
		{
			isPrimitive = true;
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x7a1ed0
			return (T)(object)(new ES3Ref(reader.Read_ref()));
		}
	}
}
