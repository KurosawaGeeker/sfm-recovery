using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3Type_decimal : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x555390.
		public ES3Type_decimal()
			: base(typeof(System.Decimal))
		{
			isPrimitive = true;
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x5554D0
			if (writer is null) throw new System.NullReferenceException();
			writer.WritePrimitive((decimal)obj);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x91d810
			return (T)(object)(reader.Read_decimal());
		}
	}
}
