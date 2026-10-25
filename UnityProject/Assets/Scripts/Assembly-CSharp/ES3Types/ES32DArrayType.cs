using System;

namespace ES3Types
{
	public class ES32DArrayType : ES3CollectionType
	{
		// Recovered original constructor RVA 0x549DD0.
		public ES32DArrayType(Type type)
			: base(type)
		{
		}

		public override void Write(object obj, ES3Writer writer, ES3.ReferenceMode unityObjectType)
		{
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x74A2F0
			return Read(reader);
		}

		public override object Read(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x54A030
			if (reader.StartReadCollection()) return null;
			var values = new System.Collections.Generic.List<object>();
			int rows = 0;
			while (reader.StartReadCollectionItem())
			{
			    ReadICollection<object>(reader, values, elementType);
			    ++rows;
			    if (reader.EndReadCollectionItem()) break;
			}
			int columns = values.Count / rows;
			Array array = Array.CreateInstance(elementType.type, new int[] { rows, columns });
			int offset = 0;
			for (int row = 0; row < rows; ++row)
			{
			    for (int column = 0; column < columns; ++column)
			        array.SetValue(values[offset + column], row, column);
			    offset += columns;
			}
			return array;
		}

		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			// Recovered from GameAssembly.dll RVA 0x74A320
			ReadInto(reader, obj);
		}

		public override void ReadInto(ES3Reader reader, object obj)
		{
			// Recovered from GameAssembly.dll RVA 0x54A310
			System.Array array = (System.Array)obj;
			if (reader.StartReadCollection()) throw new System.NullReferenceException("The Collection we are trying to load is stored as null, which is not allowed when using ReadInto methods.");
			bool end0 = false;
			for (int index0 = 0; index0 < array.GetLength(0); ++index0)
			{
			bool end1 = false;
			if (!reader.StartReadCollectionItem()) throw new System.IndexOutOfRangeException("The collection we are loading is smaller than the collection provided as a parameter.");
			reader.StartReadCollection();
			for (int index1 = 0; index1 < array.GetLength(1); ++index1)
			{
			if (!reader.StartReadCollectionItem()) throw new System.IndexOutOfRangeException("The collection we are loading is smaller than the collection provided as a parameter.");
			reader.ReadInto<object>(array.GetValue(index0, index1), elementType);
			end1 = reader.EndReadCollectionItem();
			}
			if (!end1) throw new System.IndexOutOfRangeException("The collection we are loading is larger than the collection provided as a parameter.");
			reader.EndReadCollection();
			end0 = reader.EndReadCollectionItem();
			}
			if (!end0) throw new System.IndexOutOfRangeException("The collection we are loading is larger than the collection provided as a parameter.");
			reader.EndReadCollection();
		}
	}
}
