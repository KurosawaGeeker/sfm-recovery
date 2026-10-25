using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3Type_DateTime : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x554f90.
		public ES3Type_DateTime()
			: base(typeof(System.DateTime))
		{
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x5550D0
			System.DateTime value = (System.DateTime)obj;
			writer.WriteProperty("ticks", value.Ticks, ES3Type_long.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x79A800
			return new System.DateTime(reader.ReadProperty<long>(ES3Type_long.Instance));
		}
	}
}
