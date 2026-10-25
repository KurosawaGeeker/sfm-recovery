using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3Type_byteArray : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x554a80.
		public ES3Type_byteArray()
			: base(typeof(byte[]))
		{
			isPrimitive = true;
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x554BC0
			if (writer is null) throw new System.NullReferenceException();
			writer.WritePrimitive((byte[])obj);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x91bd70
			return (T)(object)(reader.Read_byteArray());
		}
	}
}
