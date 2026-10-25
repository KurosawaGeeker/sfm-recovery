using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3Type_char : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x554c60.
		public ES3Type_char()
			: base(typeof(char))
		{
			isPrimitive = true;
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x554DA0
			if (writer is null) throw new System.NullReferenceException();
			writer.WritePrimitive((char)obj);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x91c9e0
			return (T)(object)(reader.Read_char());
		}
	}
}
