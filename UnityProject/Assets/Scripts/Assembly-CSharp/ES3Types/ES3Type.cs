using System;
using System.ComponentModel;
using ES3Internal;
using UnityEngine.Scripting;

namespace ES3Types
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Preserve]
	public abstract class ES3Type
	{
		public const string typeFieldName = "__type";

		public ES3Member[] members;

		public Type type;

		public bool isPrimitive;

		public bool isValueType;

		public bool isCollection;

		public bool isDictionary;

		public bool isTuple;

		public bool isEnum;

		public bool isES3TypeUnityObject;

		public bool isReflectedType;

		public bool isUnsupported;

		public int priority;

		// Recovered original constructor RVA 0x552040.
		protected ES3Type(Type type)
		{
			ES3Internal.ES3TypeMgr.Add(type, this);
			this.type = type;
			isValueType = type.IsValueType;
		}

		public abstract void Write(object obj, ES3Writer writer);

		public abstract object Read<T>(ES3Reader reader);

		public virtual void ReadInto<T>(ES3Reader reader, object obj)
		{
			// Recovered from GameAssembly.dll RVA 0x761230
			throw new NotImplementedException("Self-assigning Read is not implemented or supported on this type.");
		}

		protected bool WriteUsingDerivedType(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x552140
			Type actualType = obj.GetType();
			if (actualType == type) return false;
			if (writer is null) throw new NullReferenceException();
			writer.WriteType(actualType);
			ES3Type derived = ES3TypeMgr.GetOrCreateES3Type(actualType, true);
			if (derived is null) throw new NullReferenceException();
			derived.Write(obj, writer);
			return true;
		}

		protected void ReadUsingDerivedType<T>(ES3Reader reader, object obj)
		{
		}

		internal string ReadPropertyName(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x552240
			if (reader.overridePropertiesName == null) return reader.ReadPropertyName();
			string name = reader.overridePropertiesName;
			reader.overridePropertiesName = null;
			return name;
		}

		protected void WriteProperties(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x5522D0
			if (members is null)
			{
			    if (writer is null || writer.settings is null) throw new NullReferenceException();
			    GetMembers(writer.settings.safeReflection, null);
			}
			// Original rereads members/length each iteration, including after getters.
			for (int i = 0; i < members.Length; ++i)
			{
			    ES3Member member = members[i];
			    string name = member.name;
			    // Native offset0x38 is reflectedMember.isProperty, not useReflection.
			    object value = member.reflectedMember.GetValue(obj);
			    ES3Type memberType = ES3TypeMgr.GetOrCreateES3Type(member.type, true);
			    if (writer is null || writer.settings is null) throw new NullReferenceException();
			    writer.WriteProperty(name, value, memberType, writer.settings.memberReferenceMode);
			}
		}

		protected object ReadProperties(ES3Reader reader, object obj)
		{
			// Recovered from GameAssembly.dll RVA 0x552490
			foreach (string name in reader.Properties)
			{
			    ES3Member member = null;
			    for (int i = 0; i < members.Length; ++i)
			    {
			        if (members[i].name == name) { member = members[i]; break; }
			    }
			    if (name == "_Values")
			    {
			        ES3Type parent = ES3TypeMgr.GetOrCreateES3Type(obj.GetType().BaseType);
			        if (parent.isDictionary)
			        {
			            System.Collections.IDictionary target = (System.Collections.IDictionary)obj;
			            System.Collections.IDictionary source = (System.Collections.IDictionary)parent.Read<object>(reader);
			            foreach (System.Collections.DictionaryEntry entry in source) target.Add(entry.Key, entry.Value);
			        }
			        else if (parent.isCollection)
			        {
			            System.Collections.IEnumerable source = (System.Collections.IEnumerable)parent.Read<object>(reader);
			            Type parentClass = parent.GetType();
			            if (parentClass == typeof(ES3ListType))
			            {
			                foreach (object item in source) ((System.Collections.IList)obj).Add(item);
			            }
			            else if (parentClass == typeof(ES3QueueType) || parentClass == typeof(ES3StackType) || parentClass == typeof(ES3HashSetType))
			            {
			                string method = parentClass == typeof(ES3QueueType) ? "Enqueue" : parentClass == typeof(ES3StackType) ? "Push" : "Add";
			                System.Reflection.MethodInfo add = parent.type.GetMethod(method, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
			                foreach (object item in source) add.Invoke(obj, new object[] { item });
			            }
			        }
			        // Native _Values processing falls through to the ordinary lookup below.
			    }
			    if (member == null) reader.Skip();
			    else
			    {
			        ES3Type serializer = ES3TypeMgr.GetOrCreateES3Type(member.type);
			        object value;
			        if (typeof(ES3DictionaryType).IsAssignableFrom(serializer.GetType())) value = ((ES3DictionaryType)serializer).Read(reader);
			        else if (typeof(ES3CollectionType).IsAssignableFrom(serializer.GetType())) value = ((ES3CollectionType)serializer).Read(reader);
			        else value = reader.Read<object>(serializer);
			        member.reflectedMember.SetValue(obj, value);
			    }
			}
			return obj;
		}

		protected void GetMembers(bool safe)
		{
			// Recovered from GameAssembly.dll RVA 0x553650
			GetMembers(safe, null);
		}

		protected void GetMembers(bool safe, string[] memberNames)
		{
			// Recovered from GameAssembly.dll RVA 0x553660
			ES3Reflection.ES3ReflectedMember[] reflected = ES3Reflection.GetSerializableMembers(type, safe, memberNames);
			members = new ES3Member[reflected.Length];
			for (int i = 0; i < reflected.Length; ++i) members[i] = new ES3Member(reflected[i]);
		}
	}
}
