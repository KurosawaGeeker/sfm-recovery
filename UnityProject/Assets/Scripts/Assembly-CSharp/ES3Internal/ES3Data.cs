using System;
using ES3Types;

namespace ES3Internal
{
	public struct ES3Data
	{
		public ES3Type type;

		public byte[] bytes;

		public ES3Data(Type type, byte[] bytes)
		{
			// Recovered from GameAssembly.dll RVA 0x5921C0
			this.type = type == null ? null : ES3TypeMgr.GetOrCreateES3Type(type, true);
			this.bytes = bytes;
		}

		public ES3Data(ES3Type type, byte[] bytes)
		{
			// Recovered from GameAssembly.dll RVA 0x592300
			this.type = type;
			this.bytes = bytes;
		}
	}
}
