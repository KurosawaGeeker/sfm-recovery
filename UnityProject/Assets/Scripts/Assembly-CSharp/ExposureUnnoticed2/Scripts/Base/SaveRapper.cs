using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.Base
{
	public class SaveRapper
	{
		public enum SaveDataType
		{
			Interrupt = 0,
			AutoSave = 1,
			HomeSave = 2,
			ManualSlot = 3,
			System = 4,
			NewGame = 5
		}

		private const string SlotName = "";

		private const string AutoSaveSuffix = "a";

		private const string DirectoryPathPrefix = "SaveData/";

		private const string DirectoryPathBackupPrefix = "SaveDataBackup/";

		private const string Suffix = ".sd";

		private const string SystemDataPath = "SaveData/s.sd";

		private const string InterruptDataPath = "SaveData/i.sd";

		private const string HomeSaveDataPath = "SaveData/h.sd";

		public static bool IsSystemData;

		public static bool IsInterrupt;

		public static bool IsAutoSave;

		public static bool IsHomeSave;

		public static bool IsBackup;

		public static int CurrentSlot;

		private static StringBuilder sb = new StringBuilder();

		private static string currentDataPath;

		public static string GetSaveDataPath()
		{
			// Recovered from GameAssembly.dll RVA 0x4B9B00
			if (IsSystemData) return SystemDataPath;
			if (IsInterrupt) return InterruptDataPath;
			if (IsHomeSave) return HomeSaveDataPath;
			sb.Length = 0;
			sb.Append(IsBackup ? DirectoryPathBackupPrefix : DirectoryPathPrefix);
			sb.Append(IsAutoSave ? AutoSaveSuffix : "");
			sb.Append(SlotName);
			sb.Append(CurrentSlot);
			sb.Append(Suffix);
			return sb.ToString();
		}

		public static string GetSpecifiedSaveDataPath(SaveDataType type, int slotNum = -1)
		{
			// Recovered from GameAssembly.dll RVA 0x4B9DA0
			if (type == SaveDataType.System) return SystemDataPath;
			if (type == SaveDataType.Interrupt) return InterruptDataPath;
			if (type == SaveDataType.HomeSave) return HomeSaveDataPath;
			sb.Length = 0;
			sb.Append(IsBackup ? DirectoryPathBackupPrefix : DirectoryPathPrefix);
			sb.Append(type == SaveDataType.AutoSave ? AutoSaveSuffix : "");
			sb.Append(SlotName);
			sb.Append(slotNum);
			sb.Append(Suffix);
			return sb.ToString();
		}

		public static bool IsExistSaveData(SaveDataType type, int slot = -1)
		{
			// Recovered from GameAssembly.dll RVA 0x4B9FC0
			if ((type == SaveDataType.AutoSave || type == SaveDataType.ManualSlot) && slot == -1)
			    throw new System.Exception("スロットが指定されてないです");
			return ES3.FileExists(GetSpecifiedSaveDataPath(type, slot));
		}

		public static void PreProcess(bool isSystem, int slot, bool isBackUp)
		{
			// Recovered from GameAssembly.dll RVA 0x4BA110
			IsSystemData = isSystem;
			CurrentSlot = slot;
			IsBackup = isBackUp;
			currentDataPath = GetSaveDataPath();
		}

		public static void AfterProcess()
		{
			// Recovered from GameAssembly.dll RVA 0x4BA220
			IsBackup = false;
		}

		public static void Copy(string from, string to)
		{
		}

		public static bool IsExistFile(string path)
		{
			// Recovered from GameAssembly.dll RVA 0x3C2EE0
			return ES3.FileExists(path);
		}

		public static void Save(string key, int value)
		{
            // Original SaveRapper wrapper: preserve the existing ES3 settings and file protocol.
            ES3.Save<int>(key, value, currentDataPath);
        }

		public static void Save(string key, string value)
		{
		}

		public static void Save(string key, double value)
		{
		}

		public static void Save(string key, bool value)
		{
            // Original SaveRapper wrapper: preserve the existing ES3 settings and file protocol.
            ES3.Save<bool>(key, value, currentDataPath);
        }

		public static void Save(string key, Vector3 value)
		{
		}

		public static void Save(string key, Quaternion value)
		{
		}

		public static void Save<T1, T2>(string key, Dictionary<T1, T2> value)
		{
		}

		public static void Save<T1>(string key, List<T1> value)
		{
		}

		public static void Save<T1>(string key, T1 value)
		{
            // Original SaveRapper wrapper: preserve the existing ES3 settings and file protocol.
            ES3.Save<T1>(key, value, currentDataPath);
        }

		public static int Load(string key, int defaultValue)
		{
			// Recovered from GameAssembly.dll RVA 0x4BA9B0
			return ES3.Load<int>(key, currentDataPath, defaultValue);
		}

		public static double Load(string key, double defaultValue)
		{
			// Recovered from GameAssembly.dll RVA 0x4BAAF0
			return ES3.Load<double>(key, currentDataPath, defaultValue);
		}

		public static string Load(string key, string defaultValue)
		{
			// Recovered from GameAssembly.dll RVA 0x4BAC40
			return ES3.Load<string>(key, currentDataPath, defaultValue);
		}

		public static bool Load(string key, bool defaultValue)
		{
			// Recovered from GameAssembly.dll RVA 0x4BACD0
			return ES3.Load<bool>(key, currentDataPath, defaultValue);
		}

		public static Vector3 Load(string key, Vector3 defaultValue)
		{
			// Recovered from GameAssembly.dll RVA 0x4BAE20
			return ES3.Load<Vector3>(key, currentDataPath, defaultValue);
		}

		public static Quaternion Load(string key, Quaternion defaultValue)
		{
			// Recovered from GameAssembly.dll RVA 0x4BAFA0
			return ES3.Load<Quaternion>(key, currentDataPath, defaultValue);
		}

		public static Dictionary<T1, T2> Load<T1, T2>(string key, Dictionary<T1, T2> defaultValue)
		{
			// Recovered from GameAssembly.dll RVA 0x9FF0F0
			return ES3.Load<Dictionary<T1, T2>>(key, currentDataPath, defaultValue);
		}

		public static List<T1> Load<T1>(string key, List<T1> defaultValue)
		{
			// Recovered from GameAssembly.dll RVA 0x9FF0F0
			return ES3.Load<List<T1>>(key, currentDataPath, defaultValue);
		}

		public static T1 Load<T1>(string key, T1 defaultValue)
		{
			// Recovered from GameAssembly.dll RVA 0x9FEF00
			return ES3.Load<T1>(key, currentDataPath, defaultValue);
		}
	}
}
