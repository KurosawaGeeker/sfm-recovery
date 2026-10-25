using System;
using ES3Internal;
using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3ConcurrentDictionaryType : ES3Type
	{
		public ES3Type keyType;

		public ES3Type valueType;

		protected ES3Reflection.ES3ReflectedMethod readMethod;

		protected ES3Reflection.ES3ReflectedMethod readIntoMethod;

		public ES3ConcurrentDictionaryType(Type type)
			: base(null)
		{
		}

		public ES3ConcurrentDictionaryType(Type type, ES3Type keyType, ES3Type valueType)
			: base(null)
		{
		}

		public override void Write(object obj, ES3Writer writer)
		{
		}

		public void Write(object obj, ES3Writer writer, ES3.ReferenceMode memberReferenceMode)
		{
		}

		public override object Read<T>(ES3Reader reader)
		{
			return null;
		}

		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
		}

		public object Read(ES3Reader reader)
		{
			return null;
		}

		public void ReadInto(ES3Reader reader, object obj)
		{
		}
	}
}
