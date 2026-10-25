using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public abstract class ES3ScriptableObjectType : ES3UnityObjectType
	{
		public ES3ScriptableObjectType(Type type)
			: base(null)
		{
		}

		protected abstract void WriteScriptableObject(object obj, ES3Writer writer);

		protected abstract void ReadScriptableObject<T>(ES3Reader reader, object obj);

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
	}
}
