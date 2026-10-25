using System.IO;

namespace ES3Internal
{
	internal class ES3ResourcesStream : MemoryStream
	{
		public bool Exists => Length > 0;

		public ES3ResourcesStream(string path) : base(GetData(path), true)
        {
        }

		private static byte[] GetData(string path)
		{
			// Recovered from GameAssembly.dll RVA 0x5A36F0
			var asset = UnityEngine.Resources.Load(path) as UnityEngine.TextAsset;
			return asset == null ? new byte[0] : asset.bytes;
		}

		protected override void Dispose(bool disposing)
		{
			// Recovered from GameAssembly.dll RVA 0x5A3870
			base.Dispose(disposing);
		}
	}
}
