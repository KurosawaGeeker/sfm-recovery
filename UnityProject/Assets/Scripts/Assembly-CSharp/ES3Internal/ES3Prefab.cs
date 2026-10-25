using System.Collections.Generic;
using UnityEngine;

namespace ES3Internal
{
	public class ES3Prefab : MonoBehaviour
	{
		public long prefabId;

		public ES3RefIdDictionary localRefs;

		public void Awake()
		{
		}

		public long Get(Object obj)
		{
			return 0L;
		}

		public long Add(Object obj)
		{
			return 0L;
		}

		public Dictionary<string, string> GetReferences()
		{
			return null;
		}

		public void ApplyReferences(Dictionary<long, long> localToGlobal)
		{
		}

		public static long GetNewRefID()
		{
			return 0L;
		}
	}
}
