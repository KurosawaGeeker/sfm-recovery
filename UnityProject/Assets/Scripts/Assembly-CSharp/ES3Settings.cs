using System;
using System.ComponentModel;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

[IncludeInSettings(true)]
public class ES3Settings : ICloneable
{
	private static ES3Settings _defaults;

	private static ES3Defaults _defaultSettingsScriptableObject;

	private const string defaultSettingsPath = "ES3/ES3Defaults";

	private static ES3Settings _unencryptedUncompressedSettings;

	private static readonly string[] resourcesExtensions = new string[] {".txt", ".htm", ".html", ".xml", ".bytes", ".json", ".csv", ".yaml", ".fnt"};

	[SerializeField]
	private ES3.Location _location;

	public string path;

	public ES3.EncryptionType encryptionType;

	public ES3.CompressionType compressionType;

	public string encryptionPassword;

	public ES3.Directory directory;

	public ES3.Format format;

	public bool prettyPrint;

	public int bufferSize;

	public Encoding encoding;

	public bool saveChildren;

	public bool postprocessRawCachedData;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool typeChecking;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool safeReflection;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public ES3.ReferenceMode memberReferenceMode;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public ES3.ReferenceMode referenceMode;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int serializationDepthLimit;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public string[] assemblyNames;

	public static ES3Defaults defaultSettingsScriptableObject { get { if (_defaultSettingsScriptableObject == null) _defaultSettingsScriptableObject = UnityEngine.Resources.Load<ES3Defaults>("ES3/ES3Defaults"); return _defaultSettingsScriptableObject; } }

	public static ES3Settings defaultSettings { get { if (_defaults == null && defaultSettingsScriptableObject != null) _defaults = defaultSettingsScriptableObject.settings; return _defaults; } }

	internal static ES3Settings unencryptedUncompressedSettings { get { if (_unencryptedUncompressedSettings == null) _unencryptedUncompressedSettings = new ES3Settings(ES3.EncryptionType.None, ES3.CompressionType.None); return _unencryptedUncompressedSettings; } }

	public ES3.Location location
	{
		get
		{
			// Recovered from GameAssembly.dll RVA 0x3CC590
			if (_location == ES3.Location.File && (UnityEngine.Application.platform == (RuntimePlatform)17 || UnityEngine.Application.platform == (RuntimePlatform)31)) return ES3.Location.PlayerPrefs;
			return _location;
		}
		set
		{
			// Recovered from GameAssembly.dll RVA 0x3CB800
			_location = value;
		}
	}

	public string FullPath
    {
        get
        {
            if (path == null) throw new AccessViolationException("The 'path' field of this ES3Settings is null, indicating that it was not possible to load the default settings from Resources. Please check that the ES3 Default Settings.prefab exists in Assets/Plugins/Resources/ES3/");
            if (IsAbsolute(path)) return path;
            if (location == ES3.Location.File)
            {
                if (directory == ES3.Directory.PersistentDataPath) return ES3Internal.ES3IO.persistentDataPath + "/" + path;
                if (directory == ES3.Directory.DataPath) return UnityEngine.Application.dataPath + "/" + path;
                throw new NotImplementedException("File directory \"" + directory + "\" has not been implemented.");
            }
            if (location == ES3.Location.Resources)
            {
                string extension = System.IO.Path.GetExtension(path);
                foreach (string supported in resourcesExtensions)
                    if (extension == supported) return path.Replace(extension, string.Empty);
                throw new ArgumentException("Extension of file in Resources must be .json, .bytes, .txt, .csv, .htm, .html, .xml, .yaml or .fnt, but path given was \"" + path + "\"");
            }
            return path;
        }
    }

	// Recovered original constructor RVA 0x3CCAA0
	public ES3Settings(string path = null, ES3Settings settings = null) : this(true)
	{
if (settings != null) settings.CopyInto(this);
if (path != null) this.path = path;
	}

	// Recovered original constructor RVA 0x3CCB40
	public ES3Settings(string path, params Enum[] enums) : this(enums)
	{
if (path != null) this.path = path;
	}

	// Recovered original constructor RVA 0x3CCBC0
	public ES3Settings(params Enum[] enums) : this(true)
	{
foreach (Enum value in enums)
{
    if (value is ES3.EncryptionType enc) encryptionType = enc;
    else if (value is ES3.Location loc) _location = loc;
    else if (value is ES3.CompressionType compression) compressionType = compression;
    else if (value is ES3.ReferenceMode reference) referenceMode = reference;
    else if (value is ES3.Format fmt) format = fmt;
    else if (value is ES3.Directory dir) directory = dir;
}
	}

	// Recovered original constructor RVA 0x3CCE60
	public ES3Settings(ES3.EncryptionType encryptionType, string encryptionPassword) : this(true)
	{
this.encryptionType = encryptionType;
this.encryptionPassword = encryptionPassword;
	}

	// Recovered original constructor RVA 0x3CCEF0
	public ES3Settings(string path, ES3.EncryptionType encryptionType, string encryptionPassword, ES3Settings settings = null) : this(true)
	{
if (settings != null) settings.CopyInto(this);
if (path != null) this.path = path;
this.encryptionType = encryptionType;
this.encryptionPassword = encryptionPassword;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public ES3Settings(bool applyDefaults)
	{
			// Recovered from GameAssembly.dll RVA 0x3CCFF0
			path = "SaveFile.es3";
			encryptionPassword = "password";
			prettyPrint = true;
			bufferSize = 2048;
			encoding = Encoding.UTF8;
			saveChildren = true;
			typeChecking = true;
			safeReflection = true;
			referenceMode = ES3.ReferenceMode.ByRefAndValue;
			serializationDepthLimit = 64;
			assemblyNames = new string[] {"Assembly-CSharp-firstpass", "Assembly-CSharp"};
			if (applyDefaults && defaultSettings != null) _defaults.CopyInto(this);
		}

	private static bool IsAbsolute(string path)
	{
			// Recovered from GameAssembly.dll RVA 0x3CD2F0
			return (path.Length > 0 && (path[0] == '/' || path[0] == '\\')) || (path.Length > 1 && path[1] == ':');
		}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public object Clone()
	{
			// Recovered from GameAssembly.dll RVA 0x3CD330
			var copy = new ES3Settings(true);
			CopyInto(copy);
			return copy;
		}

	private void CopyInto(ES3Settings newSettings)
	{
			// Recovered from GameAssembly.dll RVA 0x3CD3A0
			newSettings._location = _location;
			newSettings.directory = directory;
			newSettings.format = format;
			newSettings.prettyPrint = prettyPrint;
			newSettings.path = path;
			newSettings.encryptionType = encryptionType;
			newSettings.encryptionPassword = encryptionPassword;
			newSettings.compressionType = compressionType;
			newSettings.bufferSize = bufferSize;
			newSettings.encoding = encoding;
			newSettings.typeChecking = typeChecking;
			newSettings.safeReflection = safeReflection;
			newSettings.referenceMode = referenceMode;
			newSettings.memberReferenceMode = memberReferenceMode;
			newSettings.assemblyNames = assemblyNames;
			newSettings.saveChildren = saveChildren;
			newSettings.serializationDepthLimit = serializationDepthLimit;
			newSettings.postprocessRawCachedData = postprocessRawCachedData;
		}
}
