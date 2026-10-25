using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3Type_ulong : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x558e30.
		public ES3Type_ulong()
			: base(typeof(ulong))
		{
			isPrimitive = true;
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x558F70
			if (writer is null) throw new System.NullReferenceException();
			writer.WritePrimitive((ulong)obj);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x931b00
			return (T)(object)(reader.Read_ulong());
		}
	}
}
