using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3ListType : ES3CollectionType
	{
		// Recovered original constructor RVA 0x549DD0.
		public ES3ListType(Type type)
			: base(type)
		{
		}

		// Recovered original constructor RVA 0x54B2F0.
		public ES3ListType(Type type, ES3Type elementType)
			: base(type, elementType)
		{
		}

		public override void Write(object obj, ES3Writer writer, ES3.ReferenceMode memberReferenceMode)
		{
			// Recovered from GameAssembly.dll RVA 0x54E380
			if (obj is null)
			{
			    if (writer is null) throw new NullReferenceException();
			    writer.WriteNull();
			    return;
			}
			System.Collections.IList list = (System.Collections.IList)obj;
			if (elementType is null) throw new ArgumentNullException("ES3Type argument cannot be null.");
			int index = 0;
			foreach (object value in list)
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
			// Recovered from GameAssembly.dll RVA 0x7514A0
			ReadICollectionInto(reader, (System.Collections.ICollection)obj, elementType);
		}

		public override object Read(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x54E690
			System.Collections.IList list = (System.Collections.IList)ES3Internal.ES3Reflection.CreateInstance(type);
			if (reader.StartReadCollection()) return null;
			while (reader.StartReadCollectionItem())
			{
			    list.Add(reader.Read<object>(elementType));
			    if (reader.EndReadCollectionItem()) break;
			}
			reader.EndReadCollection();
			return list;
		}

		public override void ReadInto(ES3Reader reader, object obj)
		{
			// Recovered from GameAssembly.dll RVA 0x54E8F0
			System.Collections.IList list = (System.Collections.IList)obj;
			if (reader.StartReadCollection()) throw new System.NullReferenceException("The Collection we are trying to load is stored as null, which is not allowed when using ReadInto methods.");
			int count = 0;
			foreach (object value in list)
			{
			    ++count;
			    if (!reader.StartReadCollectionItem()) break;
			    reader.ReadInto<object>(value, elementType);
			    if (reader.EndReadCollectionItem()) break;
			    if (count == list.Count) throw new System.IndexOutOfRangeException("The collection we are loading is longer than the collection provided as a parameter.");
			}
			if (count != list.Count) throw new System.IndexOutOfRangeException("The collection we are loading is shorter than the collection provided as a parameter.");
			reader.EndReadCollection();
		}
	}
}
