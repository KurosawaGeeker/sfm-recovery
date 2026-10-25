using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "bytes" })]
	public class ES3Type_BigInteger : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x553f30.
		public ES3Type_BigInteger()
			: base(typeof(System.Numerics.BigInteger))
		{
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
		}

		public override object Read<T>(ES3Reader reader)
		{
			return null;
		}
	}
}
