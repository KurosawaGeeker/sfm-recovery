using System.Collections.Generic;

namespace ExposureUnnoticed2.Scripts.Base
{
	public class Localizer
	{
		public enum Language
		{
			Ja = 0,
			En = 1,
			Sc = 2,
			Tc = 3,
			Ko = 4
		}

		private static Dictionary<string, string[]> localizeData;

		public static void LoadLocalizeDataIfNot()
		{
			// Recovered from GameAssembly.dll RVA 0x4B9220
			if (localizeData != null) return;
			var records = global::Common.Scripts.Util.CSVReader.Load("dictionary");
			localizeData = new Dictionary<string, string[]>();
			foreach (string[] record in records)
			{
			    if (record[1] == "" || localizeData.ContainsKey(record[1])) continue;
			    localizeData.Add(record[1], new[] { record[2], record[3], record[4], record[5], record[6] });
			}
		}

		public static string Trans(string key)
		{
			// Recovered from GameAssembly.dll RVA 0x4B9690
			if (localizeData == null) LoadLocalizeDataIfNot();
			if (!localizeData.ContainsKey(key)) return key ?? "";
			return localizeData[key][(int)global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.Language].Replace("<n>", "\n");
		}

		public static string Trans(string key, string param)
		{
			// Recovered from GameAssembly.dll RVA 0x4B9810
			string result = Trans(key);
			if (result.Contains("{0}")) result = result.Replace("{0}", param);
			return result;
		}

		public static string Trans(string key, string param, string param2)
		{
			// Recovered from GameAssembly.dll RVA 0x4B98A0
			string result = Trans(key);
			if (result.Contains("{0}")) result = result.Replace("{0}", param);
			if (result.Contains("{1}")) result = result.Replace("{1}", param2);
			return result;
		}

		public static string Trans(string key, string[] param)
		{
			// Recovered from GameAssembly.dll RVA 0x4B9980
			string result = Trans(key);
			var builder = new System.Text.StringBuilder();
			for (int i = 0; i < param.Length; ++i)
			{
			    builder.Append("{").Append(i).Append("}");
			    string token = builder.ToString();
			    if (result.Contains(token)) result = result.Replace(token, param[i]);
			    builder.Length = 0;
			}
			return result;
		}
	}
}
