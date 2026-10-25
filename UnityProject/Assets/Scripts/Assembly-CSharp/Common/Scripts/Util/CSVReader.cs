using System.Collections.Generic;
using UnityEngine;

namespace Common.Scripts.Util
{
	public class CSVReader : MonoBehaviour
	{
		public static List<string[]> Load(string filename)
		{
			// Recovered from GameAssembly.dll RVA 0x57E6A0
			var result = new List<string[]>();
			TextAsset asset = UnityEngine.Resources.Load(filename) as TextAsset;
			var reader = new System.IO.StringReader(asset.text);
			while (reader.Peek() != -1) result.Add(reader.ReadLine().Split('\t'));
			return result;
		}
	}
}
