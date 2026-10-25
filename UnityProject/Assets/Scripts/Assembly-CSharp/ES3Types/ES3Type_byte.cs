using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3Type_byte : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x5548b0.
		public ES3Type_byte()
			: base(typeof(byte))
		{
			isPrimitive = true;
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x5549F0
			if (writer is null) throw new System.NullReferenceException();
			writer.WritePrimitive((byte)obj);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x91b150
			return (T)(object)(reader.Read_byte());
		}
	}
}
