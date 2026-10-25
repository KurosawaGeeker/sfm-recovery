using System;
using ES3Types;
using Unity.VisualScripting;
using UnityEngine;

[IncludeInSettings(true)]
public class ES3
{
	public enum Location
	{
		File = 0,
		PlayerPrefs = 1,
		InternalMS = 2,
		Resources = 3,
		Cache = 4
	}

	public enum Directory
	{
		PersistentDataPath = 0,
		DataPath = 1
	}

	public enum EncryptionType
	{
		None = 0,
		AES = 1
	}

	public enum CompressionType
	{
		None = 0,
		Gzip = 1
	}

	public enum Format
	{
		JSON = 0
	}

	public enum ReferenceMode
	{
		ByRef = 0,
		ByValue = 1,
		ByRefAndValue = 2
	}

	public enum ImageType
	{
		JPEG = 0,
		PNG = 1
	}

	public static void Save(string key, object value)
	{
			// Recovered from GameAssembly.dll RVA 0x3BBA60
			Save<object>(key, value, new ES3Settings(true));
		}

	public static void Save(string key, object value, string filePath)
	{
			// Recovered from GameAssembly.dll RVA 0x3BBAF0
			Save<object>(key, value, new ES3Settings(filePath, (ES3Settings)null));
		}

	public static void Save(string key, object value, string filePath, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3BBBE0
			Save<object>(key, value, new ES3Settings(filePath, settings));
		}

	public static void Save(string key, object value, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3BBCF0
			Save<object>(key, value, settings);
		}

	public static void Save<T>(string key, T value)
	{
			// Recovered from GameAssembly.dll RVA 0x747EC0
			Save<T>(key, value, new ES3Settings(true));
		}

	public static void Save<T>(string key, T value, string filePath)
	{
			// Recovered from GameAssembly.dll RVA 0x748D40
			Save<T>(key, value, new ES3Settings(filePath, (ES3Settings)null));
		}

	public static void Save<T>(string key, T value, string filePath, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x749810
			Save<T>(key, value, new ES3Settings(filePath, settings));
		}

	public static void Save<T>(string key, T value, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x748B90
			if (settings.location == Location.Cache)
			{
			    ES3File.GetOrCreateCachedFile(settings).Save<T>(key, value);
			    return;
			}
			using (ES3Writer writer = ES3Writer.Create(settings))
			{
			    writer.Write<T>(key, value);
			    writer.Save();
			}
		}

	public static void SaveRaw(byte[] bytes)
	{
	}

	public static void SaveRaw(byte[] bytes, string filePath)
	{
	}

	public static void SaveRaw(byte[] bytes, string filePath, ES3Settings settings)
	{
	}

	public static void SaveRaw(byte[] bytes, ES3Settings settings)
	{
	}

	public static void SaveRaw(string str)
	{
	}

	public static void SaveRaw(string str, string filePath)
	{
	}

	public static void SaveRaw(string str, string filePath, ES3Settings settings)
	{
	}

	public static void SaveRaw(string str, ES3Settings settings)
	{
	}

	public static void AppendRaw(byte[] bytes)
	{
	}

	public static void AppendRaw(byte[] bytes, string filePath, ES3Settings settings)
	{
	}

	public static void AppendRaw(byte[] bytes, ES3Settings settings)
	{
	}

	public static void AppendRaw(string str)
	{
	}

	public static void AppendRaw(string str, string filePath)
	{
	}

	public static void AppendRaw(string str, string filePath, ES3Settings settings)
	{
	}

	public static void AppendRaw(string str, ES3Settings settings)
	{
	}

	public static void SaveImage(Texture2D texture, string imagePath)
	{
	}

	public static void SaveImage(Texture2D texture, string imagePath, ES3Settings settings)
	{
	}

	public static void SaveImage(Texture2D texture, ES3Settings settings)
	{
	}

	public static void SaveImage(Texture2D texture, int quality, string imagePath)
	{
	}

	public static void SaveImage(Texture2D texture, int quality, string imagePath, ES3Settings settings)
	{
	}

	public static void SaveImage(Texture2D texture, int quality, ES3Settings settings)
	{
	}

	public static byte[] SaveImageToBytes(Texture2D texture, int quality, ImageType imageType)
	{
		return null;
	}

	public static object Load(string key)
	{
			// Recovered from GameAssembly.dll RVA 0x3BD2D0
			return Load<object>(key, new ES3Settings(true));
		}

	public static object Load(string key, string filePath)
	{
			// Recovered from GameAssembly.dll RVA 0x3BD350
			return Load<object>(key, new ES3Settings(filePath));
		}

	public static object Load(string key, string filePath, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3BD430
			return Load<object>(key, new ES3Settings(filePath, settings));
		}

	public static object Load(string key, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3BD530
			return Load<object>(key, settings);
		}

	public static object Load(string key, object defaultValue)
	{
			// Recovered from GameAssembly.dll RVA 0x3BD580
			return Load<object>(key, defaultValue, new ES3Settings(true));
		}

	public static object Load(string key, string filePath, object defaultValue)
	{
			// Recovered from GameAssembly.dll RVA 0x3BD610
			return Load<object>(key, defaultValue, new ES3Settings(filePath));
		}

	public static object Load(string key, string filePath, object defaultValue, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3BD700
			return Load<object>(key, defaultValue, new ES3Settings(filePath, settings));
		}

	public static object Load(string key, object defaultValue, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3BD810
			return Load<object>(key, defaultValue, settings);
		}

	public static T Load<T>(string key)
	{
			// Recovered from GameAssembly.dll RVA 0x7451D0
			return Load<T>(key, new ES3Settings(true));
		}

	public static T Load<T>(string key, string filePath)
	{
			// Recovered from GameAssembly.dll RVA 0x745940
			return Load<T>(key, new ES3Settings(filePath));
		}

	public static T Load<T>(string key, string filePath, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x7474F0
			return Load<T>(key, new ES3Settings(filePath, settings));
		}

	public static T Load<T>(string key, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x7452C0
			if (settings.location == Location.Cache) return ES3File.GetOrCreateCachedFile(settings).Load<T>(key);
			using (ES3Reader reader = ES3Reader.Create(settings))
			{
			    if (reader == null) throw new System.IO.FileNotFoundException("File \"" + settings.FullPath + "\" could not be found.");
			    return reader.Read<T>(key);
			}
		}

	public static T Load<T>(string key, T defaultValue)
	{
			// Recovered from GameAssembly.dll RVA 0x7454F0
			return Load<T>(key, defaultValue, new ES3Settings(true));
		}

	public static T Load<T>(string key, string filePath, T defaultValue)
	{
			// Recovered from GameAssembly.dll RVA 0x7467D0
			return Load<T>(key, defaultValue, new ES3Settings(filePath));
		}

	public static T Load<T>(string key, string filePath, T defaultValue, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x747660
			return Load<T>(key, defaultValue, new ES3Settings(filePath, settings));
		}

	public static T Load<T>(string key, T defaultValue, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x7465F0
			if (settings.location == Location.Cache) return ES3File.GetOrCreateCachedFile(settings).Load<T>(key, defaultValue);
			using (ES3Reader reader = ES3Reader.Create(settings))
			{
			    if (reader == null) return defaultValue;
			    return reader.Read<T>(key, defaultValue);
			}
		}

	public static void LoadInto<T>(string key, object obj) where T : class
	{
			// Recovered from GameAssembly.dll RVA 0x7479B0
			LoadInto<object>(key, obj, new ES3Settings(true));
		}

	public static void LoadInto(string key, string filePath, object obj)
	{
			// Recovered from GameAssembly.dll RVA 0x3BD870
			LoadInto<object>(key, obj, new ES3Settings(filePath));
		}

	public static void LoadInto(string key, string filePath, object obj, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3BD960
			LoadInto<object>(key, obj, new ES3Settings(filePath, settings));
		}

	public static void LoadInto(string key, object obj, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3BDA70
			LoadInto<object>(key, obj, settings);
		}

	public static void LoadInto<T>(string key, T obj) where T : class
	{
			// Recovered from GameAssembly.dll RVA 0x747920
			LoadInto<T>(key, obj, new ES3Settings(true));
		}

	public static void LoadInto<T>(string key, string filePath, T obj) where T : class
	{
			// Recovered from GameAssembly.dll RVA 0x747CC0
			LoadInto<T>(key, obj, new ES3Settings(filePath));
		}

	public static void LoadInto<T>(string key, string filePath, T obj, ES3Settings settings) where T : class
	{
			// Recovered from GameAssembly.dll RVA 0x747DC0
			LoadInto<T>(key, obj, new ES3Settings(filePath, settings));
		}

	public static void LoadInto<T>(string key, T obj, ES3Settings settings) where T : class
	{
			// Recovered from GameAssembly.dll RVA 0x747A40
			if (obj.GetType().IsValueType) throw new System.InvalidOperationException("ES3.LoadInto can only be used with reference types, but the data you're loading is a value type. Use ES3.Load instead.");
			if (settings.location == Location.Cache)
			{
			    ES3File.GetOrCreateCachedFile(settings).LoadInto<T>(key, obj);
			    return;
			}
			using (ES3Reader reader = ES3Reader.Create(settings))
			{
			    if (reader == null) throw new System.IO.FileNotFoundException("File \"" + settings.FullPath + "\" could not be found.");
			    reader.ReadInto<T>(key, obj);
			}
		}

	public static string LoadString(string key, string defaultValue, ES3Settings settings)
	{
		return null;
	}

	public static string LoadString(string key, string defaultValue, string filePath = null, ES3Settings settings = null)
	{
		return null;
	}

	public static byte[] LoadRawBytes()
	{
			// Recovered from GameAssembly.dll RVA 0x3BDC20
			return LoadRawBytes(new ES3Settings(true));
		}

	public static byte[] LoadRawBytes(string filePath)
	{
			// Recovered from GameAssembly.dll RVA 0x3BDC80
			return LoadRawBytes(new ES3Settings(filePath, (ES3Settings)null));
		}

	public static byte[] LoadRawBytes(string filePath, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3BDD40
			return LoadRawBytes(new ES3Settings(filePath, settings));
		}

	public static byte[] LoadRawBytes(ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3BDE10
			if (settings is null) throw new System.NullReferenceException();
			if (settings.location == Location.Cache) return ES3File.GetOrCreateCachedFile(settings).LoadRawBytes();
			using (System.IO.Stream stream = ES3Internal.ES3Stream.CreateStream(settings, ES3Internal.ES3FileMode.Read))
			{
			    if (stream is null) throw new System.IO.FileNotFoundException("File " + settings.path + " could not be found");
			    if (stream.GetType() == typeof(System.IO.Compression.GZipStream))
			    {
			        using (var memory = new System.IO.MemoryStream())
			        {
			            ES3Internal.ES3Stream.CopyTo(stream, memory);
			            return memory.ToArray();
			        }
			    }
			    byte[] bytes = new byte[unchecked((int)stream.Length)];
			    stream.Read(bytes, 0, bytes.Length);
			    return bytes;
			}
		}

	public static string LoadRawString()
	{
			// Recovered from GameAssembly.dll RVA 0x3BE230
			ES3Settings effective = new ES3Settings(true);
			byte[] bytes = LoadRawBytes(effective);
			return effective.encoding.GetString(bytes, 0, bytes.Length);
		}

	public static string LoadRawString(string filePath)
	{
			// Recovered from GameAssembly.dll RVA 0x3BE2C0
			ES3Settings effective = new ES3Settings(filePath, (ES3Settings)null);
			byte[] bytes = LoadRawBytes(effective);
			return effective.encoding.GetString(bytes, 0, bytes.Length);
		}

	public static string LoadRawString(string filePath, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3BE3C0
			ES3Settings effective = new ES3Settings(filePath, settings);
			byte[] bytes = LoadRawBytes(effective);
			return effective.encoding.GetString(bytes, 0, bytes.Length);
		}

	public static string LoadRawString(ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3BE4D0
			ES3Settings effective = settings;
			byte[] bytes = LoadRawBytes(effective);
			return effective.encoding.GetString(bytes, 0, bytes.Length);
		}

	public static Texture2D LoadImage(string imagePath)
	{
		return null;
	}

	public static Texture2D LoadImage(string imagePath, ES3Settings settings)
	{
		return null;
	}

	public static Texture2D LoadImage(ES3Settings settings)
	{
		return null;
	}

	public static Texture2D LoadImage(byte[] bytes)
	{
		return null;
	}

	public static AudioClip LoadAudio(string audioFilePath, AudioType audioType)
	{
		return null;
	}

	public static AudioClip LoadAudio(string audioFilePath, AudioType audioType, ES3Settings settings)
	{
		return null;
	}

	public static byte[] Serialize<T>(T value, ES3Settings settings = null)
	{
			// Recovered from GameAssembly.dll RVA 0x749E30
			return Serialize(value, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(T), true), settings);
		}

	internal static byte[] Serialize(object value, ES3Type type, ES3Settings settings = null)
	{
			// Recovered from GameAssembly.dll RVA 0x3BF050
			if (settings is null) settings = new ES3Settings();
			using (var memory = new System.IO.MemoryStream())
			using (System.IO.Stream stream = ES3Internal.ES3Stream.CreateStream(memory, settings, ES3Internal.ES3FileMode.Write))
			{
			    using (ES3Writer writer = ES3Writer.Create(stream, settings, false, false))
			        writer.Write(value, type, settings.referenceMode);
			    return memory.ToArray();
			}
		}

	public static T Deserialize<T>(byte[] bytes, ES3Settings settings = null)
	{
			// Recovered from GameAssembly.dll RVA 0x744660
			return (T)Deserialize(ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(T), true), bytes, settings);
		}

	internal static object Deserialize(ES3Type type, byte[] bytes, ES3Settings settings = null)
	{
			// Recovered from GameAssembly.dll RVA 0x3BF2E0
			if (settings == null) settings = new ES3Settings(true);
			using (var stream = new System.IO.MemoryStream(bytes, false))
			using (System.IO.Stream wrapped = ES3Internal.ES3Stream.CreateStream(stream, settings, ES3Internal.ES3FileMode.Read))
			using (ES3Reader reader = ES3Reader.Create(wrapped, settings, false))
			    return reader.Read<object>(type);
		}

	public static void DeserializeInto<T>(byte[] bytes, T obj, ES3Settings settings = null) where T : class
	{
			// Recovered from GameAssembly.dll RVA 0x744E60
			DeserializeInto<T>(ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(T), true), bytes, obj, settings);
		}

	public static void DeserializeInto<T>(ES3Type type, byte[] bytes, T obj, ES3Settings settings = null) where T : class
	{
			// Recovered from GameAssembly.dll RVA 0x744F90
			if (settings == null) settings = new ES3Settings(true);
			using (var stream = new System.IO.MemoryStream(bytes, false))
			using (ES3Reader reader = ES3Reader.Create(stream, settings, false))
			    reader.ReadInto<T>(obj, type);
		}

	public static byte[] EncryptBytes(byte[] bytes, string password = null)
	{
		return null;
	}

	public static byte[] DecryptBytes(byte[] bytes, string password = null)
	{
		return null;
	}

	public static string EncryptString(string str, string password = null)
	{
		return null;
	}

	public static string DecryptString(string str, string password = null)
	{
		return null;
	}

	public static byte[] CompressBytes(byte[] bytes)
	{
		return null;
	}

	public static byte[] DecompressBytes(byte[] bytes)
	{
		return null;
	}

	public static string CompressString(string str)
	{
		return null;
	}

	public static string DecompressString(string str)
	{
		return null;
	}

	public static void DeleteFile()
	{
			// Recovered from GameAssembly.dll RVA 0x3C0370
			DeleteFile(new ES3Settings(true));
		}

	public static void DeleteFile(string filePath)
	{
			// Recovered from GameAssembly.dll RVA 0x3C03D0
			DeleteFile(new ES3Settings(filePath, (ES3Settings)null));
		}

	public static void DeleteFile(string filePath, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3C0490
			DeleteFile(new ES3Settings(filePath, settings));
		}

	public static void DeleteFile(ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3C0560
			if (settings.location == Location.File) ES3Internal.ES3IO.DeleteFile(settings.FullPath);
			else if (settings.location == Location.PlayerPrefs) UnityEngine.PlayerPrefs.DeleteKey(settings.FullPath);
			else if (settings.location == Location.Cache) ES3File.RemoveCachedFile(settings);
			else if (settings.location == Location.Resources) throw new NotSupportedException("Deleting files from Resources is not possible.");
		}

	public static void CopyFile(string oldFilePath, string newFilePath)
	{
	}

	public static void CopyFile(string oldFilePath, string newFilePath, ES3Settings oldSettings, ES3Settings newSettings)
	{
	}

	public static void CopyFile(ES3Settings oldSettings, ES3Settings newSettings)
	{
	}

	public static void RenameFile(string oldFilePath, string newFilePath)
	{
	}

	public static void RenameFile(string oldFilePath, string newFilePath, ES3Settings oldSettings, ES3Settings newSettings)
	{
	}

	public static void RenameFile(ES3Settings oldSettings, ES3Settings newSettings)
	{
	}

	public static void CopyDirectory(string oldDirectoryPath, string newDirectoryPath)
	{
	}

	public static void CopyDirectory(string oldDirectoryPath, string newDirectoryPath, ES3Settings oldSettings, ES3Settings newSettings)
	{
	}

	public static void CopyDirectory(ES3Settings oldSettings, ES3Settings newSettings)
	{
	}

	public static void RenameDirectory(string oldDirectoryPath, string newDirectoryPath)
	{
	}

	public static void RenameDirectory(string oldDirectoryPath, string newDirectoryPath, ES3Settings oldSettings, ES3Settings newSettings)
	{
	}

	public static void RenameDirectory(ES3Settings oldSettings, ES3Settings newSettings)
	{
	}

	public static void DeleteDirectory(string directoryPath)
	{
	}

	public static void DeleteDirectory(string directoryPath, ES3Settings settings)
	{
	}

	public static void DeleteDirectory(ES3Settings settings)
	{
	}

	public static void DeleteKey(string key)
	{
	}

	public static void DeleteKey(string key, string filePath)
	{
	}

	public static void DeleteKey(string key, string filePath, ES3Settings settings)
	{
	}

	public static void DeleteKey(string key, ES3Settings settings)
	{
	}

	public static bool KeyExists(string key)
	{
			// Recovered from GameAssembly.dll RVA 0x3C2A30
			return KeyExists(key, new ES3Settings(true));
		}

	public static bool KeyExists(string key, string filePath)
	{
			// Recovered from GameAssembly.dll RVA 0x3C2AA0
			return KeyExists(key, new ES3Settings(filePath, (ES3Settings)null));
		}

	public static bool KeyExists(string key, string filePath, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3C2B60
			return KeyExists(key, new ES3Settings(filePath, settings));
		}

	public static bool KeyExists(string key, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3C2C50
			if (settings.location == Location.Cache) return ES3File.KeyExists(key, settings);
			using (ES3Reader reader = ES3Reader.Create(settings))
			{
			    if (reader == null) return false;
			    return reader.Goto(key);
			}
		}

	public static bool FileExists()
	{
			// Recovered from GameAssembly.dll RVA 0x3C2E80
			return FileExists(new ES3Settings(true));
		}

	public static bool FileExists(string filePath)
	{
			// Recovered from GameAssembly.dll RVA 0x3C2EE0
			return FileExists(new ES3Settings(filePath, (ES3Settings)null));
		}

	public static bool FileExists(string filePath, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3C2FA0
			return FileExists(new ES3Settings(filePath, settings));
		}

	public static bool FileExists(ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3C3070
			if (settings.location == Location.File) return ES3Internal.ES3IO.FileExists(settings.FullPath);
			if (settings.location == Location.PlayerPrefs) return UnityEngine.PlayerPrefs.HasKey(settings.FullPath);
			if (settings.location == Location.Cache) return ES3File.FileExists(settings);
			if (settings.location == Location.Resources) return UnityEngine.Resources.Load(settings.FullPath) != null;
			return false;
		}

	public static bool DirectoryExists(string folderPath)
	{
		return false;
	}

	public static bool DirectoryExists(string folderPath, ES3Settings settings)
	{
		return false;
	}

	public static bool DirectoryExists(ES3Settings settings)
	{
		return false;
	}

	public static string[] GetKeys()
	{
			// Recovered from GameAssembly.dll RVA 0x3C35C0
			return GetKeys(new ES3Settings(true));
		}

	public static string[] GetKeys(string filePath)
	{
			// Recovered from GameAssembly.dll RVA 0x3C3620
			return GetKeys(new ES3Settings(filePath, (ES3Settings)null));
		}

	public static string[] GetKeys(string filePath, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3C36E0
			return GetKeys(new ES3Settings(filePath, settings));
		}

	public static string[] GetKeys(ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3C37B0
			if (settings.location == Location.Cache) return ES3File.GetKeys(settings);
			var keys = new System.Collections.Generic.List<string>();
			using (ES3Reader reader = ES3Reader.Create(settings))
			{
			    foreach (string key in reader.Properties)
			    {
			        keys.Add(key);
			        reader.Skip();
			    }
			}
			return keys.ToArray();
		}

	public static string[] GetFiles()
	{
		return null;
	}

	public static string[] GetFiles(string directoryPath)
	{
		return null;
	}

	public static string[] GetFiles(string directoryPath, ES3Settings settings)
	{
		return null;
	}

	public static string[] GetFiles(ES3Settings settings)
	{
		return null;
	}

	public static string[] GetDirectories()
	{
		return null;
	}

	public static string[] GetDirectories(string directoryPath)
	{
		return null;
	}

	public static string[] GetDirectories(string directoryPath, ES3Settings settings)
	{
		return null;
	}

	public static string[] GetDirectories(ES3Settings settings)
	{
		return null;
	}

	public static void CreateBackup()
	{
	}

	public static void CreateBackup(string filePath)
	{
	}

	public static void CreateBackup(string filePath, ES3Settings settings)
	{
	}

	public static void CreateBackup(ES3Settings settings)
	{
	}

	public static bool RestoreBackup(string filePath)
	{
		return false;
	}

	public static bool RestoreBackup(string filePath, ES3Settings settings)
	{
		return false;
	}

	public static bool RestoreBackup(ES3Settings settings)
	{
		return false;
	}

	public static DateTime GetTimestamp()
	{
			// Recovered from GameAssembly.dll RVA 0x3C49B0
			return GetTimestamp(new ES3Settings(true));
		}

	public static DateTime GetTimestamp(string filePath)
	{
			// Recovered from GameAssembly.dll RVA 0x3C4A10
			return GetTimestamp(new ES3Settings(filePath, (ES3Settings)null));
		}

	public static DateTime GetTimestamp(string filePath, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3C4AD0
			return GetTimestamp(new ES3Settings(filePath, settings));
		}

	public static DateTime GetTimestamp(ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3C4BA0
			if (settings.location == Location.File) return ES3Internal.ES3IO.GetTimestamp(settings.FullPath);
			if (settings.location == Location.PlayerPrefs) return new DateTime(long.Parse(UnityEngine.PlayerPrefs.GetString("timestamp_" + settings.FullPath, "0")), DateTimeKind.Utc);
			if (settings.location == Location.Cache) return ES3File.GetTimestamp(settings);
			return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		}

	public static void StoreCachedFile()
	{
			// Recovered from GameAssembly.dll RVA 0x3C4E30
			ES3File.Store(null);
		}

	public static void StoreCachedFile(string filePath)
	{
			// Recovered from GameAssembly.dll RVA 0x3C4E80
			ES3File.Store(new ES3Settings(filePath, (ES3Settings)null));
		}

	public static void StoreCachedFile(string filePath, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3C4F70
			ES3File.Store(new ES3Settings(filePath, settings));
		}

	public static void StoreCachedFile(ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3C5070
			ES3File.Store(settings);
		}

	public static void CacheFile()
	{
			// Recovered from GameAssembly.dll RVA 0x3C50C0
			ES3File.CacheFile(new ES3Settings(true));
		}

	public static void CacheFile(string filePath)
	{
			// Recovered from GameAssembly.dll RVA 0x3C5150
			ES3File.CacheFile(new ES3Settings(filePath, (ES3Settings)null));
		}

	public static void CacheFile(string filePath, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3C5240
			ES3File.CacheFile(new ES3Settings(filePath, settings));
		}

	public static void CacheFile(ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3C5340
			ES3File.CacheFile(settings);
		}

	public static void Init()
	{
	}
}
