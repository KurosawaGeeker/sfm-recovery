using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3Type_sbyte : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x557e40.
		public ES3Type_sbyte()
			: base(typeof(sbyte))
		{
			isPrimitive = true;
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x557F80
			if (writer is null) throw new System.NullReferenceException();
			writer.WritePrimitive((sbyte)obj);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x92f1c0
			return (T)(object)(reader.Read_sbyte());
		}
	}
}
