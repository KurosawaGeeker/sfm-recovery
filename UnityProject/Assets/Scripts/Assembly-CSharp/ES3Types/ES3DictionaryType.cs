using System;
using ES3Internal;
using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3DictionaryType : ES3Type
	{
		public ES3Type keyType;

		public ES3Type valueType;

		protected ES3Reflection.ES3ReflectedMethod readMethod;

		protected ES3Reflection.ES3ReflectedMethod readIntoMethod;

		// Recovered original constructor RVA 0x54CD80.
		public ES3DictionaryType(Type type)
			: base(type)
		{
			Type[] elementTypes = ES3Internal.ES3Reflection.GetElementTypes(type);
			keyType = ES3Internal.ES3TypeMgr.GetOrCreateES3Type(elementTypes[0], false);
			valueType = ES3Internal.ES3TypeMgr.GetOrCreateES3Type(elementTypes[1], false);
			if (keyType == null || valueType == null) isUnsupported = true;
			isDictionary = true;
		}

		// Recovered original constructor RVA 0x54C1D0.
		public ES3DictionaryType(Type type, ES3Type keyType, ES3Type valueType)
			: base(type)
		{
			this.keyType = keyType;
			this.valueType = valueType;
			if (keyType == null || valueType == null) isUnsupported = true;
			isDictionary = true;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x54CF30
			if (writer is null || writer.settings is null) throw new NullReferenceException();
			Write(obj, writer, writer.settings.memberReferenceMode);
		}

		public void Write(object obj, ES3Writer writer, ES3.ReferenceMode memberReferenceMode)
		{
			// Recovered from GameAssembly.dll RVA 0x54CF60
			System.Collections.IDictionary dictionary = (System.Collections.IDictionary)obj;
			int index = 0;
			foreach (System.Collections.DictionaryEntry entry in dictionary)
			{
			    if (writer is null) throw new NullReferenceException();
			    writer.StartWriteDictionaryKey(index);
			    writer.Write(entry.Key, keyType, memberReferenceMode);
			    writer.EndWriteDictionaryKey(index);
			    writer.StartWriteDictionaryValue(index);
			    writer.Write(entry.Value, valueType, memberReferenceMode);
			    writer.EndWriteDictionaryValue(index);
			    ++index;
			}
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x74C810
			return Read(reader);
		}

		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			// Recovered from GameAssembly.dll RVA 0x74C820
			ReadInto(reader, obj);
		}

		public object Read(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x54D2C0
			if (reader.StartReadDictionary()) return null;
			System.Collections.IDictionary dictionary = (System.Collections.IDictionary)ES3Internal.ES3Reflection.CreateInstance(type);
			while (reader.StartReadDictionaryKey())
			{
			    object key = reader.Read<object>(keyType);
			    reader.EndReadDictionaryKey();
			    reader.StartReadDictionaryValue();
			    object value = reader.Read<object>(valueType);
			    dictionary.Add(key, value);
			    if (reader.EndReadDictionaryValue())
			    {
			        reader.EndReadDictionary();
			        break;
			    }
			}
			return dictionary;
		}

		public void ReadInto(ES3Reader reader, object obj)
		{
			// Recovered from GameAssembly.dll RVA 0x54D5C0
			if (reader.StartReadDictionary()) throw new System.NullReferenceException("The Dictionary we are trying to load is stored as null, which is not allowed when using ReadInto methods.");
			System.Collections.IDictionary dictionary = (System.Collections.IDictionary)obj;
			while (reader.StartReadDictionaryKey())
			{
			    object key = reader.Read<object>(keyType);
			    if (!dictionary.Contains(key)) throw new System.Collections.Generic.KeyNotFoundException("The key \"" + (key == null ? null : key.ToString()) + "\" in the Dictionary we are loading does not exist in the Dictionary we are loading into");
			    object value = dictionary[key];
			    reader.EndReadDictionaryKey();
			    reader.StartReadDictionaryValue();
			    reader.ReadInto<object>(value, valueType);
			    if (reader.EndReadDictionaryValue())
			    {
			        reader.EndReadDictionary();
			        return;
			    }
			}
		}
	}
}
