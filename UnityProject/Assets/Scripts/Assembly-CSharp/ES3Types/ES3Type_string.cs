using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3Type_string : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x5584a0.
		public ES3Type_string()
			: base(typeof(string))
		{
			isPrimitive = true;
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x5585E0
			if (writer is null) throw new System.NullReferenceException();
			writer.WritePrimitive((string)obj);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x930670
			return reader.Read_string();
		}
	}
}
