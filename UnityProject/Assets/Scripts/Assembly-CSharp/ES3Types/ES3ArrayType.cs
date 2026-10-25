using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3ArrayType : ES3CollectionType
	{
		// Recovered original constructor RVA 0x549DD0.
		public ES3ArrayType(Type type)
			: base(type)
		{
		}

		// Recovered original constructor RVA 0x54B2F0.
		public ES3ArrayType(Type type, ES3Type elementType)
			: base(type, elementType)
		{
		}

		public override void Write(object obj, ES3Writer writer, ES3.ReferenceMode memberReferenceMode)
		{
			// Recovered from GameAssembly.dll RVA 0x54B300
			Array array = (Array)obj;
			if (elementType is null) throw new ArgumentNullException("ES3Type argument cannot be null.");
			for (int i = 0; i < array.Length; ++i)
			{
			    if (writer is null) throw new NullReferenceException();
			    writer.StartWriteCollectionItem(i);
			    object value = array.GetValue(i);
			    writer.Write(value, elementType, memberReferenceMode);
			    writer.EndWriteCollectionItem(i);
			}
		}

		public override object Read(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x54B4A0
			var values = new System.Collections.Generic.List<object>();
			if (!ReadICollection<object>(reader, values, elementType)) return null;
			Array array = ES3Internal.ES3Reflection.ArrayCreateInstance(elementType.type, values.Count);
			int index = 0;
			foreach (object value in values) array.SetValue(value, index++);
			return array;
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x74A2F0
			return Read(reader);
		}

		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			// Recovered from GameAssembly.dll RVA 0x74A350
			ReadICollectionInto(reader, (System.Collections.ICollection)obj, elementType);
		}

		public override void ReadInto(ES3Reader reader, object obj)
		{
			// Recovered from GameAssembly.dll RVA 0x54B700
			System.Collections.IList list = (System.Collections.IList)obj;
			if (list.Count == 0) ES3Internal.ES3Debug.LogWarning("LoadInto/ReadInto expects a collection containing instances to load data in to, but the collection is empty.");
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
