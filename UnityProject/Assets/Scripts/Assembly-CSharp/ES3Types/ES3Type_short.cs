using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3Type_short : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x558170.
		public ES3Type_short()
			: base(typeof(short))
		{
			isPrimitive = true;
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x5582B0
			if (writer is null) throw new System.NullReferenceException();
			writer.WritePrimitive((short)obj);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x92ff70
			return (T)(object)(reader.Read_short());
		}
	}
}
