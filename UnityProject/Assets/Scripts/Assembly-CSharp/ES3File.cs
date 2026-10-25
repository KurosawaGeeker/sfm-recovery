using System;
using System.Collections.Generic;
using System.ComponentModel;
using ES3Internal;

public class ES3File
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Dictionary<string, ES3File> cachedFiles = new Dictionary<string, ES3File>();

	public ES3Settings settings;

	private Dictionary<string, ES3Data> cache;

	private bool syncWithFile;

	private DateTime timestamp;

	// Recovered original constructor RVA 0x3C5430.
public ES3File()
    : this(new ES3Settings(true), true)
{
}

	// Recovered original constructor RVA 0x3C54A0.
public ES3File(string filePath)
    : this(new ES3Settings(filePath, (ES3Settings)null), true)
{
}

	// Recovered original constructor RVA 0x3C5570.
public ES3File(string filePath, ES3Settings settings)
    : this(new ES3Settings(filePath, settings), true)
{
}

	// Recovered original constructor RVA 0x3C5660.
public ES3File(ES3Settings settings)
    : this(settings, true)
{
}

	// Recovered original constructor RVA 0x3C5670.
public ES3File(bool syncWithFile)
    : this(new ES3Settings(true), syncWithFile)
{
}

	// Recovered original constructor RVA 0x3C56F0.
public ES3File(string filePath, bool syncWithFile)
    : this(new ES3Settings(filePath, (ES3Settings)null), syncWithFile)
{
}

	// Recovered original constructor RVA 0x3C57D0.
public ES3File(string filePath, ES3Settings settings, bool syncWithFile)
    : this(new ES3Settings(filePath, settings), syncWithFile)
{
}

	public ES3File(ES3Settings settings, bool syncWithFile)
	{
			// Recovered from GameAssembly.dll RVA 0x3C58C0
			cache = new Dictionary<string, ES3Data>();
			timestamp = DateTime.UtcNow;
			this.settings = settings;
			this.syncWithFile = syncWithFile;
			if (syncWithFile)
			{
			    ES3Settings readSettings = (ES3Settings)settings.Clone();
			    readSettings.typeChecking = true;
			    using (ES3Reader reader = ES3Reader.Create(readSettings))
			    {
			        if (reader == null) return;
			        foreach (KeyValuePair<string, ES3Data> entry in reader.RawEnumerator) cache[entry.Key] = entry.Value;
			    }
			    timestamp = ES3.GetTimestamp(readSettings);
			}
		}

	public ES3File(byte[] bytes, ES3Settings settings = null)
	{
			// Recovered from GameAssembly.dll RVA 0x3C5DD0
			cache = new Dictionary<string, ES3Data>();
			timestamp = DateTime.UtcNow;
			this.settings = settings ?? new ES3Settings(true);
			syncWithFile = true;
			SaveRaw(bytes, settings);
		}

	public void Sync()
	{
			// Recovered from GameAssembly.dll RVA 0x3C5FE0
			Sync(settings);
		}

	public void Sync(string filePath, ES3Settings settings = null)
	{
			// Recovered from GameAssembly.dll RVA 0x3C5FF0
			Sync(new ES3Settings(filePath, settings));
		}

	public void Sync(ES3Settings settings = null)
	{
			// Recovered from GameAssembly.dll RVA 0x3C60E0
			if (settings is null) settings = new ES3Settings(true);
			if (cache.Count == 0)
			{
			    ES3.DeleteFile(settings);
			    return;
			}
			bool capturedSync = syncWithFile;
			System.IO.Stream stream = ES3Stream.CreateStream(settings, ES3FileMode.Write);
			using (ES3Writer writer = stream is null ? null : ES3Writer.Create(stream, settings, true, !capturedSync))
			{
			    foreach (var entry in cache)
			    {
			        Type valueType = entry.Value.type is null ? typeof(object) : entry.Value.type.type;
			        writer.Write(entry.Key, valueType, entry.Value.bytes);
			    }
			    writer.Save(!syncWithFile);
			}
		}

	public void Clear()
	{
			// Recovered from GameAssembly.dll RVA 0x3C64D0
			cache.Clear();
		}

	public string[] GetKeys()
	{
			// Recovered from GameAssembly.dll RVA 0x3C6560
			var keys = cache.Keys;
			string[] result = new string[keys.Count];
			keys.CopyTo(result, 0);
			return result;
		}

	public void Save<T>(string key, T value)
	{
			// Recovered from GameAssembly.dll RVA 0x74F470
			ES3Settings effective = (ES3Settings)settings.Clone();
			effective.encryptionType = ES3.EncryptionType.None;
			effective.compressionType = ES3.CompressionType.None;
			Type valueType = (object)value == null ? typeof(T) : value.GetType();
			Dictionary<string, ES3Data> targetCache = cache;
			ES3Types.ES3Type storedType = ES3TypeMgr.GetOrCreateES3Type(valueType, true);
			byte[] serialized = ES3.Serialize<T>(value, effective);
			targetCache[key] = new ES3Data(storedType, serialized);
		}

	public void SaveRaw(byte[] bytes, ES3Settings settings = null)
	{
			// Recovered from GameAssembly.dll RVA 0x3C6630
			if (settings is null) settings = new ES3Settings(true);
			ES3Settings effective = (ES3Settings)settings.Clone();
			effective.typeChecking = true;
			using (ES3Reader reader = ES3Reader.Create(bytes, effective))
			{
			    if (reader is null) return;
			    foreach (KeyValuePair<string, ES3Data> entry in reader.RawEnumerator)
			        cache[entry.Key] = entry.Value;
			}
		}

	public void AppendRaw(byte[] bytes, ES3Settings settings = null)
	{
			// Recovered from GameAssembly.dll RVA 0x3C6A10
			if (settings is null) settings = new ES3Settings(true);
			SaveRaw(bytes, settings);
		}

	public object Load(string key)
	{
			// Recovered from GameAssembly.dll RVA 0x3C6A90
			return Load<object>(key);
		}

	public object Load(string key, object defaultValue)
	{
			// Recovered from GameAssembly.dll RVA 0x3C6AE0
			return Load<object>(key, defaultValue);
		}

	public T Load<T>(string key)
	{
			// Recovered from GameAssembly.dll RVA 0x74C830
			ES3Data data;
			if (!cache.TryGetValue(key, out data)) throw new System.Collections.Generic.KeyNotFoundException("Key \"" + key + "\" was not found in this ES3File. Use Load<T>(key, defaultValue) if you want to return a default value if the key does not exist.");
			ES3Settings readSettings = (ES3Settings)settings.Clone();
			readSettings.encryptionType = ES3.EncryptionType.None;
			readSettings.compressionType = ES3.CompressionType.None;
			if (typeof(T) == typeof(object)) return (T)ES3.Deserialize(data.type, data.bytes, readSettings);
			return ES3.Deserialize<T>(data.bytes, readSettings);
		}

	public T Load<T>(string key, T defaultValue)
	{
			// Recovered from GameAssembly.dll RVA 0x74D660
			ES3Data data;
			if (!cache.TryGetValue(key, out data)) return defaultValue;
			ES3Settings readSettings = (ES3Settings)settings.Clone();
			readSettings.encryptionType = ES3.EncryptionType.None;
			readSettings.compressionType = ES3.CompressionType.None;
			if (typeof(T) == typeof(object)) return (T)ES3.Deserialize(data.type, data.bytes, readSettings);
			return ES3.Deserialize<T>(data.bytes, readSettings);
		}

	public void LoadInto<T>(string key, T obj) where T : class
	{
			// Recovered from GameAssembly.dll RVA 0x74E430
			ES3Data data;
			if (!cache.TryGetValue(key, out data)) throw new System.Collections.Generic.KeyNotFoundException("Key \"" + key + "\" was not found in this ES3File. Use Load<T>(key, defaultValue) if you want to return a default value if the key does not exist.");
			ES3Settings readSettings = (ES3Settings)settings.Clone();
			readSettings.encryptionType = ES3.EncryptionType.None;
			readSettings.compressionType = ES3.CompressionType.None;
			ES3Types.ES3Type type = typeof(T) == typeof(object) ? data.type : ES3TypeMgr.GetOrCreateES3Type(typeof(T), true);
			ES3.DeserializeInto<T>(type, data.bytes, obj, readSettings);
		}

	public byte[] LoadRawBytes()
	{
			// Recovered from GameAssembly.dll RVA 0x3C6B40
			ES3Settings effective = (ES3Settings)settings.Clone();
			if (!effective.postprocessRawCachedData)
			{
			    effective.encryptionType = ES3.EncryptionType.None;
			    effective.compressionType = ES3.CompressionType.None;
			}
			return GetBytes(effective);
		}

	public string LoadRawString()
	{
			// Recovered from GameAssembly.dll RVA 0x3C6BE0
			if (cache.Count == 0) return string.Empty;
			System.Text.Encoding encoding = settings.encoding;
			byte[] bytes = LoadRawBytes();
			return encoding.GetString(bytes);
		}

	internal byte[] GetBytes(ES3Settings settings = null)
	{
			// Recovered from GameAssembly.dll RVA 0x3C6C90
			if (cache.Count == 0) return new byte[0];
			if (settings is null) settings = this.settings;
			using (var memory = new System.IO.MemoryStream())
			{
			    var effective = (ES3Settings)settings.Clone();
			    effective.location = ES3.Location.InternalMS;
			    if (!effective.postprocessRawCachedData)
			    {
			        effective.encryptionType = ES3.EncryptionType.None;
			        effective.compressionType = ES3.CompressionType.None;
			    }
			    using (ES3Writer writer = ES3Writer.Create(ES3Internal.ES3Stream.CreateStream(memory, effective, ES3Internal.ES3FileMode.Write), effective, true, false))
			    {
			        foreach (var entry in cache) writer.Write(entry.Key, entry.Value.type.type, entry.Value.bytes);
			        writer.Save(false);
			    }
			    return memory.ToArray();
			}
		}

	public void DeleteKey(string key)
	{
			// Recovered from GameAssembly.dll RVA 0x3C7160
			cache.Remove(key);
		}

	public bool KeyExists(string key)
	{
			// Recovered from GameAssembly.dll RVA 0x3C71C0
			return cache.ContainsKey(key);
		}

	public int Size()
	{
			// Recovered from GameAssembly.dll RVA 0x3C7240
			int size = 0;
			foreach (var item in cache) size = unchecked(size + item.Value.bytes.Length);
			return size;
		}

	public Type GetKeyType(string key)
	{
			// Recovered from GameAssembly.dll RVA 0x3C73F0
			ES3Data data;
			if (!cache.TryGetValue(key, out data)) throw new System.Collections.Generic.KeyNotFoundException("Key \"" + key + "\" was not found in this ES3File. Use Load<T>(key, defaultValue) if you want to return a default value if the key does not exist.");
			return data.type.type;
		}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static ES3File GetOrCreateCachedFile(ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3C74D0
			ES3File file;
			if (!cachedFiles.TryGetValue(settings.path, out file))
			{
			    file = new ES3File(settings, false);
			    cachedFiles.Add(settings.path, file);
			    file.syncWithFile = true;
			}
			file.settings = settings;
			return file;
		}

	internal static void CacheFile(ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3C7690
			if (settings.location == ES3.Location.Cache)
			{
			    settings = (ES3Settings)settings.Clone();
			    settings.location = ES3Settings.defaultSettings.location != ES3.Location.Cache ? ES3Settings.defaultSettings.location : ES3.Location.File;
			}
			if (!ES3.FileExists(settings)) return;
			ES3Settings rawSettings = (ES3Settings)settings.Clone();
			rawSettings.encryptionType = ES3.EncryptionType.None;
			rawSettings.compressionType = ES3.CompressionType.None;
			string capturedPath = settings.path;
			Dictionary<string, ES3File> capturedFiles = cachedFiles;
			byte[] raw = ES3.LoadRawBytes(rawSettings);
			ES3File file = new ES3File(raw, settings);
			capturedFiles[capturedPath] = file;
		}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static void Store(ES3Settings settings = null)
	{
			// Recovered from GameAssembly.dll RVA 0x3C78F0
			if (settings is null) settings = new ES3Settings(new Enum[] { ES3.Location.File });
			else if (settings.location == ES3.Location.Cache)
			{
			    settings = (ES3Settings)settings.Clone();
			    settings.location = ES3Settings.defaultSettings.location != ES3.Location.Cache ? ES3Settings.defaultSettings.location : ES3.Location.File;
			}
			ES3File file;
			if (!cachedFiles.TryGetValue(settings.path, out file))
			    throw new System.IO.FileNotFoundException("The file '" + settings.path + "' could not be stored because it could not be found in the cache.");
			file.Sync(settings);
		}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static void RemoveCachedFile(ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3C7C40
			cachedFiles.Remove(settings.path);
		}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static void CopyCachedFile(ES3Settings oldSettings, ES3Settings newSettings)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static void DeleteKey(string key, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3C80A0
			ES3File file;
			if (!cachedFiles.TryGetValue(settings.path, out file)) return;
			file.DeleteKey(key);
		}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool KeyExists(string key, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3C8190
			ES3File file;
			if (!cachedFiles.TryGetValue(settings.path, out file)) return false;
			return file.KeyExists(key);
		}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool FileExists(ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3C82B0
			return cachedFiles.ContainsKey(settings.path);
		}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static string[] GetKeys(ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3C8360
			ES3File file;
			if (!cachedFiles.TryGetValue(settings.path, out file)) throw new System.IO.FileNotFoundException("Could not get keys from the file '" + settings.path + "' because it could not be found in the cache.");
			return System.Linq.Enumerable.ToArray(file.cache.Keys);
		}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static string[] GetFiles()
	{
			// Recovered from GameAssembly.dll RVA 0x3C8550
			return System.Linq.Enumerable.ToArray(cachedFiles.Keys);
		}

	internal static DateTime GetTimestamp(ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3C8670
			ES3File file;
			if (!cachedFiles.TryGetValue(settings.path, out file)) return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return file.timestamp;
		}
}
