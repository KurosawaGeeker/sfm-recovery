using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3QueueType : ES3CollectionType
	{
		// Recovered original constructor RVA 0x549DD0.
		public ES3QueueType(Type type)
			: base(type)
		{
		}

		public override void Write(object obj, ES3Writer writer, ES3.ReferenceMode memberReferenceMode)
		{
			// Recovered from GameAssembly.dll RVA 0x54F750
			System.Collections.ICollection collection = (System.Collections.ICollection)obj;
			if (elementType is null) throw new ArgumentNullException("ES3Type argument cannot be null.");
			int index = 0;
			foreach (object value in collection)
			{
			    if (writer is null) throw new NullReferenceException();
			    writer.StartWriteCollectionItem(index);
			    writer.Write(value, elementType, memberReferenceMode);
			    writer.EndWriteCollectionItem(index);
			    ++index;
			}
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x74A2F0
			return Read(reader);
		}

		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			// Recovered from GameAssembly.dll RVA 0x752AE0
			if (reader.StartReadCollection()) throw new System.NullReferenceException("The Collection we are trying to load is stored as null, which is not allowed when using ReadInto methods.");
			System.Collections.Generic.Queue<T> collection = (System.Collections.Generic.Queue<T>)obj;
			int count = 0;
			foreach (T value in collection)
			{
			    ++count;
			    if (!reader.StartReadCollectionItem()) break;
			    reader.ReadInto<T>(value, elementType);
			    if (reader.EndReadCollectionItem()) break;
			    if (count == collection.Count) throw new System.IndexOutOfRangeException("The collection we are loading is longer than the collection provided as a parameter.");
			}
			if (count != collection.Count) throw new System.IndexOutOfRangeException("The collection we are loading is shorter than the collection provided as a parameter.");
			reader.EndReadCollection();
		}

		public override object Read(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x54FA50
			System.Collections.IList values = (System.Collections.IList)ES3Internal.ES3Reflection.CreateInstance(typeof(System.Collections.Generic.List<>).MakeGenericType(elementType.type));
			bool isNull = reader.StartReadCollection();
			if (isNull) return null;
			if (!isNull)
			{
			    while (reader.StartReadCollectionItem())
			    {
			        values.Add(reader.Read<object>(elementType));
			        if (reader.EndReadCollectionItem()) break;
			    }
			    reader.EndReadCollection();
			}
			return ES3Internal.ES3Reflection.CreateInstance(type, new object[] { values });
		}

		public override void ReadInto(ES3Reader reader, object obj)
		{
			// Recovered from GameAssembly.dll RVA 0x54FE90
			if (reader.StartReadCollection()) throw new System.NullReferenceException("The Collection we are trying to load is stored as null, which is not allowed when using ReadInto methods.");
			System.Collections.ICollection collection = (System.Collections.ICollection)obj;
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
