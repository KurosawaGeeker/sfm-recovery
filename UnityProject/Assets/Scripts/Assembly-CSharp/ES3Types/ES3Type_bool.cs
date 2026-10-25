using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3Type_bool : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x554580.
		public ES3Type_bool()
			: base(typeof(bool))
		{
			isPrimitive = true;
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x5546C0
			if (writer is null) throw new System.NullReferenceException();
			writer.WritePrimitive((bool)obj);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x91a3b0
			return (T)(object)(reader.Read_bool());
		}
	}
}
