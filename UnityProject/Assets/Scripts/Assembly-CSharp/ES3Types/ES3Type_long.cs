using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3Type_long : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x557b10.
		public ES3Type_long()
			: base(typeof(long))
		{
			isPrimitive = true;
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x557C50
			if (writer is null) throw new System.NullReferenceException();
			writer.WritePrimitive((long)obj);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x92e410
			return (T)(object)(reader.Read_long());
		}
	}
}
