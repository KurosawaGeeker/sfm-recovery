using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3Type_ushort : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x559160.
		public ES3Type_ushort()
			: base(typeof(ushort))
		{
			isPrimitive = true;
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x5592A0
			if (writer is null) throw new System.NullReferenceException();
			writer.WritePrimitive((ushort)obj);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x9328c0
			return (T)(object)(reader.Read_ushort());
		}
	}
}
