using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public abstract class ES3UnityObjectType : ES3ObjectType
	{
		// Recovered original constructor RVA 0x551530.
		public ES3UnityObjectType(Type type)
			: base(type)
		{
			isValueType = false;
			isES3TypeUnityObject = true;
		}

		protected abstract void WriteUnityObject(object obj, ES3Writer writer);

		protected abstract void ReadUnityObject<T>(ES3Reader reader, object obj);

		protected abstract object ReadUnityObject<T>(ES3Reader reader);

		protected override void WriteObject(object obj, ES3Writer writer)
		{
		}

		public virtual void WriteObject(object obj, ES3Writer writer, ES3.ReferenceMode mode)
		{
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			return null;
		}

		protected bool WriteUsingDerivedType(object obj, ES3Writer writer, ES3.ReferenceMode mode)
		{
			return false;
		}
	}
}
