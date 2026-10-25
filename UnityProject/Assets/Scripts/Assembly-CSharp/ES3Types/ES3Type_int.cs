using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3Type_int : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x5574b0.
		public ES3Type_int()
			: base(typeof(int))
		{
			isPrimitive = true;
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x5575F0
			if (writer is null) throw new System.NullReferenceException();
			writer.WritePrimitive((int)obj);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x92d660
			return (T)(object)(reader.Read_int());
		}
	}
}
