using System;

namespace ES3Internal
{
	public static class ES3IO
	{
		
		public enum ES3FileMode
		{
			Read = 0,
			Write = 1,
			Append = 2
		}

		internal static readonly string persistentDataPath = UnityEngine.Application.persistentDataPath;

		internal static readonly string dataPath = UnityEngine.Application.dataPath;

		internal const string backupFileSuffix = ".bac";

		internal const string temporaryFileSuffix = ".tmp";

		public static DateTime GetTimestamp(string filePath)
		{
			// Recovered from GameAssembly.dll RVA 0x5923B0
			if (!System.IO.File.Exists(filePath)) return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return System.IO.File.GetLastWriteTime(filePath).ToUniversalTime();
		}

		public static string GetExtension(string path)
		{
			return null;
		}

		public static void DeleteFile(string filePath)
		{
			// Recovered from GameAssembly.dll RVA 0x592610
			if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
		}

		public static bool FileExists(string filePath)
		{
			// Recovered from GameAssembly.dll RVA 0x592680
			return System.IO.File.Exists(filePath);
		}

		public static void MoveFile(string sourcePath, string destPath)
		{
			// Recovered from GameAssembly.dll RVA 0x592690
			System.IO.File.Move(sourcePath, destPath);
		}

		public static void CopyFile(string sourcePath, string destPath)
		{
		}

		public static void MoveDirectory(string sourcePath, string destPath)
		{
		}

		public static void CreateDirectory(string directoryPath)
		{
			// Recovered from GameAssembly.dll RVA 0x5926C0
			System.IO.Directory.CreateDirectory(directoryPath);
		}

		public static bool DirectoryExists(string directoryPath)
		{
			// Recovered from GameAssembly.dll RVA 0x5926D0
			return System.IO.Directory.Exists(directoryPath);
		}

		public static string GetDirectoryPath(string path, char seperator = '/')
		{
			// Recovered from GameAssembly.dll RVA 0x5926E0
			// Original optional separator parameter is ignored.
			int index = path.LastIndexOf(UsesForwardSlash(path) ? '/' : '\\');
			if (index == path.Length - 1) return path;
			if (index == -1) ES3Debug.LogError("Path provided is not a directory path as it contains no slashes.", null, 0);
			return path.Substring(0, index);
		}

		public static bool UsesForwardSlash(string path)
		{
			// Recovered from GameAssembly.dll RVA 0x5927F0
			return path.Contains("/");
		}

		public static string CombinePathAndFilename(string directoryPath, string fileOrDirectoryName)
		{
			// Recovered from GameAssembly.dll RVA 0x592850
			char last = directoryPath[directoryPath.Length - 1];
			if (last != '/' && last != '\\') directoryPath += "/";
			return directoryPath + fileOrDirectoryName;
		}

		public static string[] GetDirectories(string path, bool getFullPaths = true)
		{
			return null;
		}

		public static void DeleteDirectory(string directoryPath)
		{
		}

		public static string[] GetFiles(string path, bool getFullPaths = true)
		{
			return null;
		}

		public static byte[] ReadAllBytes(string path)
		{
			return null;
		}

		public static void WriteAllBytes(string path, byte[] bytes)
		{
		}

		public static void CommitBackup(ES3Settings settings)
		{
			// Recovered from GameAssembly.dll RVA 0x593020
			ES3Debug.Log("Committing backup for " + settings.path + " to storage location " + settings.location.ToString(), null, 0);
			string temporary = settings.FullPath + ".tmp";
			if (settings.location == ES3.Location.File)
			{
			    string backup = settings.FullPath + ".tmp.bak";
			    if (System.IO.File.Exists(settings.FullPath))
			    {
			        DeleteFile(backup);
			        System.IO.File.Copy(settings.FullPath, backup, false);
			        try
			        {
			            DeleteFile(settings.FullPath);
			            System.IO.File.Move(temporary, settings.FullPath);
			        }
			        catch (Exception exception)
			        {
			            try { DeleteFile(settings.FullPath); } catch { }
			            MoveFile(backup, settings.FullPath);
			            throw exception;
			        }
			        DeleteFile(backup);
			    }
			    else System.IO.File.Move(temporary, settings.FullPath);
			}
			else if (settings.location == ES3.Location.PlayerPrefs)
			{
			    UnityEngine.PlayerPrefs.SetString(settings.FullPath, UnityEngine.PlayerPrefs.GetString(temporary));
			    UnityEngine.PlayerPrefs.DeleteKey(temporary);
			    UnityEngine.PlayerPrefs.Save();
			}
		}
	}
}
