using System;
using ES3Internal;
using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3TupleType : ES3Type
	{
		public ES3Type[] es3Types;

		public Type[] types;

		protected ES3Reflection.ES3ReflectedMethod readMethod;

		protected ES3Reflection.ES3ReflectedMethod readIntoMethod;

		// Recovered original constructor RVA 0x550FC0.
		public ES3TupleType(Type type)
			: base(type)
		{
			types = ES3Internal.ES3Reflection.GetElementTypes(type);
			es3Types = new ES3Type[types.Length];
			for (int i = 0; i < types.Length; ++i)
			{
			    es3Types[i] = ES3Internal.ES3TypeMgr.GetOrCreateES3Type(types[i], false);
			    if (es3Types[i] == null) isUnsupported = true;
			}
			isTuple = true;
		}

		public override void Write(object obj, ES3Writer writer)
		{
		}

		public void Write(object obj, ES3Writer writer, ES3.ReferenceMode memberReferenceMode)
		{
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x75F1E0
			object[] values = new object[types.Length];
			if (reader.StartReadCollection()) return null;
			for (int i = 0; i < types.Length; ++i)
			{
			    reader.StartReadCollectionItem();
			    values[i] = reader.Read<object>(es3Types[i]);
			    reader.EndReadCollectionItem();
			}
			reader.EndReadCollection();
			return type.GetConstructor(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, null, System.Reflection.CallingConventions.Any, types, null).Invoke(System.Reflection.BindingFlags.CreateInstance, null, values, null);
		}
	}
}
