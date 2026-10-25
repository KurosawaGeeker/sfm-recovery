using System;

namespace ES3Types
{
	public class ES33DArrayType : ES3CollectionType
	{
		// Recovered original constructor RVA 0x549DD0.
		public ES33DArrayType(Type type)
			: base(type)
		{
		}

		public override void Write(object obj, ES3Writer writer, ES3.ReferenceMode memberReferenceMode)
		{
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x74A2F0
			return Read(reader);
		}

		public override object Read(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x54AA00
			if (reader.StartReadCollection()) return null;
			var values = new System.Collections.Generic.List<object>();
			int layers = 0;
			int rows = 0;
			while (reader.StartReadCollectionItem())
			{
			    reader.StartReadCollection();
			    ++layers;
			    while (reader.StartReadCollectionItem())
			    {
			        ReadICollection<object>(reader, values, elementType);
			        ++rows;
			        if (reader.EndReadCollectionItem()) break;
			    }
			    reader.EndReadCollection();
			    if (reader.EndReadCollectionItem()) break;
			}
			reader.EndReadCollection();
			rows /= layers;
			int columns = values.Count / rows / layers;
			Array array = Array.CreateInstance(elementType.type, new int[] { layers, rows, columns });
			int rowOffset = 0;
			for (int layer = 0; layer < layers; ++layer)
			{
			    for (int row = 0; row < rows; ++row)
			        for (int column = 0; column < columns; ++column)
			            array.SetValue(values[(rowOffset + row) * columns + column], layer, row, column);
			    rowOffset += rows;
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
			// Recovered from GameAssembly.dll RVA 0x54ADB0
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
			bool end2 = false;
			if (!reader.StartReadCollectionItem()) throw new System.IndexOutOfRangeException("The collection we are loading is smaller than the collection provided as a parameter.");
			reader.StartReadCollection();
			for (int index2 = 0; index2 < array.GetLength(2); ++index2)
			{
			if (!reader.StartReadCollectionItem()) throw new System.IndexOutOfRangeException("The collection we are loading is smaller than the collection provided as a parameter.");
			reader.ReadInto<object>(array.GetValue(index0, index1, index2), elementType);
			end2 = reader.EndReadCollectionItem();
			}
			if (!end2) throw new System.IndexOutOfRangeException("The collection we are loading is larger than the collection provided as a parameter.");
			reader.EndReadCollection();
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
