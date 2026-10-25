using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public abstract class ES3ObjectType : ES3Type
	{
		// Recovered original constructor RVA 0x551C70.
		public ES3ObjectType(Type type)
			: base(type)
		{
		}

		protected abstract void WriteObject(object obj, ES3Writer writer);

		protected abstract object ReadObject<T>(ES3Reader reader);

		protected virtual void ReadObject<T>(ES3Reader reader, object obj)
		{
			// Recovered from GameAssembly.dll RVA 0x752A50
			throw new NotSupportedException("ReadInto is not supported for type " + (type == null ? null : type.ToString()));
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x551C80
			if (WriteUsingDerivedType(obj, writer)) return;
			Type baseType = obj.GetType().BaseType;
			if (baseType != typeof(object))
			{
			    ES3Type baseSerializer = ES3Internal.ES3TypeMgr.GetOrCreateES3Type(baseType, false);
			    if (baseSerializer != null && (baseSerializer.isDictionary || baseSerializer.isCollection))
			    {
			        if (writer is null) throw new NullReferenceException();
			        writer.WriteProperty("_Values", obj, baseSerializer);
			    }
			}
			WriteObject(obj, writer);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x7525E0
			string name = ReadPropertyName(reader);
			if (name == "__type") return ES3Internal.ES3TypeMgr.GetOrCreateES3Type(reader.ReadType()).Read<T>(reader);
			reader.overridePropertiesName = name;
			return ReadObject<T>(reader);
		}

		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			// Recovered from GameAssembly.dll RVA 0x7527D0
			while (true)
			{
			    string name = ReadPropertyName(reader);
			    if (name == "__type")
			    {
			        ES3Internal.ES3TypeMgr.GetOrCreateES3Type(reader.ReadType()).ReadInto<T>(reader, obj);
			        return;
			    }
			    if (name == null) return;
			    reader.overridePropertiesName = name;
			    ReadObject<T>(reader, obj);
			}
		}
	}
}
