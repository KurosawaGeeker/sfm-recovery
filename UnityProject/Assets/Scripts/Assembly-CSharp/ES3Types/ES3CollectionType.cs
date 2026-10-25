using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public abstract class ES3CollectionType : ES3Type
	{
		public ES3Type elementType;

		public abstract object Read(ES3Reader reader);

		public abstract void ReadInto(ES3Reader reader, object obj);

		public abstract void Write(object obj, ES3Writer writer, ES3.ReferenceMode memberReferenceMode);

		// Recovered original constructor RVA 0x54BAF0.
		public ES3CollectionType(Type type)
			: base(type)
		{
			elementType = ES3Internal.ES3TypeMgr.GetOrCreateES3Type(ES3Internal.ES3Reflection.GetElementTypes(type)[0], false);
			isCollection = true;
			if (elementType == null) isUnsupported = true;
		}

		// Recovered original constructor RVA 0x54BC10.
		public ES3CollectionType(Type type, ES3Type elementType)
			: base(type)
		{
			this.elementType = elementType;
			isCollection = true;
		}

		[Preserve]
		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x54BC80
			Write(obj, writer, ES3.ReferenceMode.ByRefAndValue);
		}

		protected virtual bool ReadICollection<T>(ES3Reader reader, ICollection<T> collection, ES3Type elementType)
		{
			// Recovered from GameAssembly.dll RVA 0x74BB50
			if (reader.StartReadCollection()) return false;
			while (reader.StartReadCollectionItem())
			{
			    collection.Add(reader.Read<T>(elementType));
			    if (reader.EndReadCollectionItem()) break;
			}
			reader.EndReadCollection();
			return true;
		}

		protected virtual void ReadICollectionInto<T>(ES3Reader reader, ICollection<T> collection, ES3Type elementType)
		{
		}

		[Preserve]
		protected virtual void ReadICollectionInto(ES3Reader reader, ICollection collection, ES3Type elementType)
		{
			// Recovered from GameAssembly.dll RVA 0x54BCB0
			if (reader.StartReadCollection()) throw new System.NullReferenceException("The Collection we are trying to load is stored as null, which is not allowed when using ReadInto methods.");
			int count = 0;
			foreach (object value in collection)
			{
			    ++count;
			    if (!reader.StartReadCollectionItem()) break;
			    reader.ReadInto<object>(value, elementType);
			    if (reader.EndReadCollectionItem()) break;
			    if (count == collection.Count) throw new System.IndexOutOfRangeException("The collection we are loading is longer than the collection provided as a parameter.");
			}
			if (count != collection.Count) throw new System.IndexOutOfRangeException("The collection we are loading is shorter than the collection provided as a parameter.");
			reader.EndReadCollection();
		}
	}
}
