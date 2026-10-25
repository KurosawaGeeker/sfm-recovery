using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3Type_float : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x557180.
		public ES3Type_float()
			: base(typeof(float))
		{
			isPrimitive = true;
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x5572C0
			if (writer is null) throw new System.NullReferenceException();
			writer.WritePrimitive((float)obj);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x92c8b0
			return (T)(object)(reader.Read_float());
		}
	}
}
