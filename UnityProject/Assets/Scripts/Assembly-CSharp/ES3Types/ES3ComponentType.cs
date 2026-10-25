using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public abstract class ES3ComponentType : ES3UnityObjectType
	{
		protected const string gameObjectPropertyName = "goID";

		// Recovered original constructor RVA 0x551530.
		public ES3ComponentType(Type type)
			: base(type)
		{
		}

		protected abstract void WriteComponent(object obj, ES3Writer writer);

		protected abstract void ReadComponent<T>(ES3Reader reader, object obj);

		protected override void WriteUnityObject(object obj, ES3Writer writer)
		{
		}

		protected override void ReadUnityObject<T>(ES3Reader reader, object obj)
		{
		}

		protected override object ReadUnityObject<T>(ES3Reader reader)
		{
			return null;
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			return null;
		}

		private static Component GetOrAddComponent(GameObject go, Type type)
		{
			return null;
		}

		public static Component CreateComponent(Type type)
		{
			return null;
		}
	}
}
