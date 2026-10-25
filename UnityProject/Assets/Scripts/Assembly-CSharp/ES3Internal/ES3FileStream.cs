using System;
using System.IO;

namespace ES3Internal
{
	public class ES3FileStream : FileStream
	{
		private bool isDisposed;

		public ES3FileStream(string path, ES3FileMode fileMode, int bufferSize, bool useAsync)
            : base(GetPath(path, fileMode), GetFileMode(fileMode), GetFileAccess(fileMode), FileShare.None, bufferSize, useAsync)
        {
        }

		protected static string GetPath(string path, ES3FileMode fileMode)
		{
			// Recovered from GameAssembly.dll RVA 0x5A2D80
			string directory = ES3IO.GetDirectoryPath(path, '/');
			if (fileMode == ES3FileMode.Read) return path;
			if (directory != ES3IO.persistentDataPath) ES3IO.CreateDirectory(directory);
			return fileMode == ES3FileMode.Write ? path + ES3IO.temporaryFileSuffix : path;
		}

		protected static FileMode GetFileMode(ES3FileMode fileMode)
		{
			// Recovered from GameAssembly.dll RVA 0x5A2EB0
			return fileMode == ES3FileMode.Read ? FileMode.Open : fileMode == ES3FileMode.Write ? FileMode.Create : FileMode.Append;
		}

		protected static FileAccess GetFileAccess(ES3FileMode fileMode)
		{
			// Recovered from GameAssembly.dll RVA 0x5A2ED0
			return fileMode == ES3FileMode.Read ? FileAccess.Read : FileAccess.Write;
		}

		protected override void Dispose(bool disposing)
		{
			// Recovered from GameAssembly.dll RVA 0x5A2EE0
			if (isDisposed) return;
			isDisposed = true;
			base.Dispose(disposing);
		}
	}
}
