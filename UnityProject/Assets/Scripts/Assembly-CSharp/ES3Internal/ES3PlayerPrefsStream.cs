using System.IO;

namespace ES3Internal
{
	internal class ES3PlayerPrefsStream : MemoryStream
	{
		private string path;

		private bool append;

		private bool isWriteStream;

		private bool isDisposed;

		public ES3PlayerPrefsStream(string path) : base(GetData(path, false), true)
        {
            this.path = path;
            append = false;
        }

		public ES3PlayerPrefsStream(string path, int bufferSize, bool append = false) : base(bufferSize)
        {
            this.path = path;
            this.append = append;
            isWriteStream = true;
        }

		private static byte[] GetData(string path, bool isWriteStream)
		{
			// Recovered from GameAssembly.dll RVA 0x5A3020
			// Original isWriteStream parameter is unused.
			if (!UnityEngine.PlayerPrefs.HasKey(path)) throw new FileNotFoundException("File \"" + path + "\" could not be found in PlayerPrefs");
			return System.Convert.FromBase64String(UnityEngine.PlayerPrefs.GetString(path));
		}

		protected override void Dispose(bool disposing)
		{
			// Recovered from GameAssembly.dll RVA 0x5A31D0
			if (isDisposed) return;
			isDisposed = true;
			if (isWriteStream && Length > 0)
			{
			    if (!append)
			        UnityEngine.PlayerPrefs.SetString(path + ES3IO.temporaryFileSuffix, System.Convert.ToBase64String(ToArray()));
			    else
			    {
			        byte[] previous = System.Convert.FromBase64String(UnityEngine.PlayerPrefs.GetString(path));
			        byte[] current = ToArray();
			        byte[] combined = new byte[previous.Length + current.Length];
			        System.Buffer.BlockCopy(previous, 0, combined, 0, previous.Length);
			        System.Buffer.BlockCopy(current, 0, combined, previous.Length, current.Length);
			        UnityEngine.PlayerPrefs.SetString(path, System.Convert.ToBase64String(combined));
			        UnityEngine.PlayerPrefs.Save();
			    }
			    UnityEngine.PlayerPrefs.SetString("timestamp_" + path, System.DateTime.UtcNow.Ticks.ToString());
			}
			base.Dispose(disposing);
		}
	}
}
