using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3Type_double : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x5556c0.
		public ES3Type_double()
			: base(typeof(double))
		{
			isPrimitive = true;
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x555800
			if (writer is null) throw new System.NullReferenceException();
			writer.WritePrimitive((double)obj);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x91e630
			return (T)(object)(reader.Read_double());
		}
	}
}
