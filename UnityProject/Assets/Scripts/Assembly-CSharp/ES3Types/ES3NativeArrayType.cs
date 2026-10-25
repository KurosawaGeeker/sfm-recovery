using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3NativeArrayType : ES3CollectionType
	{
		// Recovered original constructor RVA 0x549DD0.
		public ES3NativeArrayType(Type type)
			: base(type)
		{
		}

		// Recovered original constructor RVA 0x54B2F0.
		public ES3NativeArrayType(Type type, ES3Type elementType)
			: base(type, elementType)
		{
		}

		public override void Write(object obj, ES3Writer writer, ES3.ReferenceMode memberReferenceMode)
		{
		}

		public override object Read(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x54EFB0
			Array values = ReadAsArray(reader);
			return ES3Internal.ES3Reflection.CreateInstance(type, new object[] { values, (Unity.Collections.Allocator)4 });
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x74A2F0
			return Read(reader);
		}

		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			// Recovered from GameAssembly.dll RVA 0x74A320
			ReadInto(reader, obj);
		}

		public override void ReadInto(ES3Reader reader, object obj)
		{
			// Recovered from GameAssembly.dll RVA 0x54F110
			System.Array values = ReadAsArray(reader);
			// Original predicate0x54F6A0 calls System.Object.GetType on ParameterInfo,
			// not ParameterInfo.ParameterType. Retain the original nonmatching predicate.
			System.Reflection.MethodInfo method = System.Linq.Enumerable.First(ES3Internal.ES3Reflection.GetMethods(type, "CopyFrom"), candidate => candidate.GetParameters()[0].GetType().IsArray);
			method.Invoke(obj, System.Reflection.BindingFlags.Default, null, new object[] { values }, null);
		}

		private Array ReadAsArray(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x54F3A0
			var values = new System.Collections.Generic.List<object>();
			if (!ReadICollection<object>(reader, values, elementType)) return null;
			Array array = ES3Internal.ES3Reflection.ArrayCreateInstance(elementType.type, values.Count);
			int index = 0;
			foreach (object value in values) array.SetValue(value, index++);
			return array;
		}
	}
}
