using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ES3Internal
{
	[Serializable]
	[DisallowMultipleComponent]
	public abstract class ES3ReferenceMgrBase : MonoBehaviour
	{
		internal object _lock;

		public const string referencePropertyName = "_ES3Ref";

		private static ES3ReferenceMgrBase _current;

		private static HashSet<ES3ReferenceMgrBase> mgrs;

		[NonSerialized]
		public List<UnityEngine.Object> excludeObjects;

		private static System.Random rng;

		[HideInInspector]
		public bool openPrefabs;

		public List<ES3Prefab> prefabs;

		[SerializeField]
		public ES3IdRefDictionary idRef;

		private ES3RefIdDictionary _refId;

		public static ES3ReferenceMgrBase Current => null;

		public bool IsInitialised => false;

		public ES3RefIdDictionary refId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public ES3GlobalReferences GlobalReferences => null;

		public static ES3ReferenceMgrBase GetManagerFromScene(Scene scene)
		{
			return null;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Init()
		{
		}

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		public void Merge(ES3ReferenceMgrBase otherMgr)
		{
		}

		public long Get(UnityEngine.Object obj)
		{
			return 0L;
		}

		internal UnityEngine.Object Get(long id, Type type, bool suppressWarnings = false)
		{
			return null;
		}

		public UnityEngine.Object Get(long id, bool suppressWarnings = false)
		{
			return null;
		}

		public ES3Prefab GetPrefab(long id, bool suppressWarnings = false)
		{
			return null;
		}

		public long GetPrefab(ES3Prefab prefabToFind, bool suppressWarnings = false)
		{
			return 0L;
		}

		public long Add(UnityEngine.Object obj)
		{
			return 0L;
		}

		public long Add(UnityEngine.Object obj, long id)
		{
			return 0L;
		}

		public bool AddPrefab(ES3Prefab prefab)
		{
			return false;
		}

		public void Remove(UnityEngine.Object obj)
		{
		}

		public void Remove(long referenceID)
		{
		}

		public void RemoveNullValues()
		{
		}

		public void RemoveNullOrInvalidValues()
		{
		}

		public void Clear()
		{
		}

		public bool Contains(UnityEngine.Object obj)
		{
			return false;
		}

		public bool Contains(long referenceID)
		{
			return false;
		}

		public void ChangeId(long oldId, long newId)
		{
		}

		internal static long GetNewRefID()
		{
			return 0L;
		}

		internal static bool CanBeSaved(UnityEngine.Object obj)
		{
			return false;
		}
	}
}
