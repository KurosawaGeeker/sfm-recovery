using System;
using System.Collections.Generic;
using UnityEngine;

namespace ES3Internal
{
	[Serializable]
	public abstract class ES3SerializableDictionary<TKey, TVal> : Dictionary<TKey, TVal>, ISerializationCallbackReceiver
	{
		[SerializeField]
		private List<TKey> _Keys;

		[SerializeField]
		private List<TVal> _Values;

		protected abstract bool KeysAreEqual(TKey a, TKey b);

		protected abstract bool ValuesAreEqual(TVal a, TVal b);

		public void OnBeforeSerialize()
		{
		}

		public void OnAfterDeserialize()
		{
		}

		public int RemoveNullValues()
		{
			return 0;
		}

		public bool ChangeKey(TKey oldKey, TKey newKey)
		{
			return false;
		}
	}
}
