using System.IO;

namespace ES3Internal
{
	public static class ES3Stream
	{
		public static Stream CreateStream(ES3Settings settings, ES3FileMode fileMode)
		{
			// Recovered from GameAssembly.dll RVA 0x5A3880
			// The original always constructs FileInfo before dispatching, including non-file locations.
			new FileInfo(settings.FullPath);
			Stream stream = null;
			if (settings.location == ES3.Location.InternalMS)
			{
			    if (fileMode == ES3FileMode.Read) return null;
			    stream = new MemoryStream(settings.bufferSize);
			}
			else if (settings.location == ES3.Location.File)
			{
			    if (fileMode == ES3FileMode.Read && !ES3IO.FileExists(settings.FullPath)) return null;
			    stream = new ES3FileStream(settings.FullPath, fileMode, settings.bufferSize, false);
			}
			else if (settings.location == ES3.Location.PlayerPrefs)
			{
			    if (fileMode == ES3FileMode.Read)
			    {
			        if (!UnityEngine.PlayerPrefs.HasKey(settings.FullPath)) return null;
			        stream = new ES3PlayerPrefsStream(settings.FullPath);
			    }
			    else stream = new ES3PlayerPrefsStream(settings.FullPath, settings.bufferSize, fileMode == ES3FileMode.Append);
			}
			else if (settings.location == ES3.Location.Resources)
			{
			    if (fileMode != ES3FileMode.Read) throw new System.NotSupportedException("Cannot write to Resources folder at runtime. Use a different save location at runtime instead.");
			    stream = new ES3ResourcesStream(settings.FullPath);
			    if (stream.Length < 1) { stream.Dispose(); return null; }
			}
			return CreateStream(stream, settings, fileMode);
		}

		public static Stream CreateStream(Stream stream, ES3Settings settings, ES3FileMode fileMode)
		{
			// Recovered from GameAssembly.dll RVA 0x5A3E80
			if (settings.encryptionType != ES3.EncryptionType.None && stream.GetType() != typeof(UnbufferedCryptoStream))
			{
			    EncryptionAlgorithm algorithm = settings.encryptionType == ES3.EncryptionType.AES ? new AESEncryptionAlgorithm() : null;
			    stream = new UnbufferedCryptoStream(stream, fileMode == ES3FileMode.Read, settings.encryptionPassword, settings.bufferSize, algorithm);
			}
			if (settings.compressionType != ES3.CompressionType.None && stream.GetType() != typeof(System.IO.Compression.GZipStream)
			    && settings.compressionType == ES3.CompressionType.Gzip)
			    stream = new System.IO.Compression.GZipStream(stream, fileMode == ES3FileMode.Read ? System.IO.Compression.CompressionMode.Decompress : System.IO.Compression.CompressionMode.Compress);
			return stream;
		}

		public static void CopyTo(Stream source, Stream destination)
		{
			// Recovered from GameAssembly.dll RVA 0x5A42B0
			source.CopyTo(destination);
		}
	}
}
