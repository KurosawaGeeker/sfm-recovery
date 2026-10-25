using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3HashSetType : ES3CollectionType
	{
		// Recovered original constructor RVA 0x549DD0.
		public ES3HashSetType(Type type)
			: base(type)
		{
		}

		public override void Write(object obj, ES3Writer writer, ES3.ReferenceMode memberReferenceMode)
		{
			// Recovered from GameAssembly.dll RVA 0x54D9F0
			if (obj is null)
			{
			    if (writer is null) throw new NullReferenceException();
			    writer.WriteNull();
			    return;
			}
			System.Collections.IEnumerable collection = (System.Collections.IEnumerable)obj;
			if (elementType is null) throw new ArgumentNullException("ES3Type argument cannot be null.");
			// Original first enumerates Current to completion, then obtains a second enumerator.
			foreach (object ignored in collection) { }
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
			// Recovered from GameAssembly.dll RVA 0x750250
			object value = Read(reader);
			return value == null ? (object)default(T) : (T)value;
		}

		public override object Read(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x54DEE0
			System.Collections.IList values = (System.Collections.IList)ES3Internal.ES3Reflection.CreateInstance(typeof(System.Collections.Generic.List<>).MakeGenericType(type.GetGenericArguments()[0]));
			bool isNull = reader.StartReadCollection();
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

		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			// Recovered from GameAssembly.dll RVA 0x74A320
			ReadInto(reader, obj);
		}

		public override void ReadInto(ES3Reader reader, object obj)
		{
			// Recovered from GameAssembly.dll RVA 0x54E330
			throw new System.NotImplementedException("Cannot use LoadInto/ReadInto with HashSet because HashSets do not maintain the order of elements");
		}
	}
}
