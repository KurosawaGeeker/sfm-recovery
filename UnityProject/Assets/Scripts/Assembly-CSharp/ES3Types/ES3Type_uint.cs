using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3Type_uint : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x5587d0.
		public ES3Type_uint()
			: base(typeof(uint))
		{
			isPrimitive = true;
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x558910
			if (writer is null) throw new System.NullReferenceException();
			writer.WritePrimitive((uint)obj);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x930d50
			return (T)(object)(reader.Read_uint());
		}
	}
}
