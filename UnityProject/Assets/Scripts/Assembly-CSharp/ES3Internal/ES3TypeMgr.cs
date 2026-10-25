using System;
using System.Collections.Generic;
using System.ComponentModel;
using ES3Types;
using UnityEngine.Scripting;

namespace ES3Internal
{
	[Preserve]
	public static class ES3TypeMgr
	{
		private static object _lock = new object();

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static Dictionary<Type, ES3Type> types = null;

		private static ES3Type lastAccessedType = null;

		public static ES3Type GetOrCreateES3Type(Type type, bool throwException = true)
		{
			// Recovered from GameAssembly.dll RVA 0x5A4620
			if (types == null) Init();
			if (type != typeof(object) && lastAccessedType != null && lastAccessedType.type == type) return lastAccessedType;
			if (!types.TryGetValue(type, out lastAccessedType)) lastAccessedType = CreateES3Type(type, throwException);
			return lastAccessedType;
		}

		public static ES3Type GetES3Type(Type type)
		{
			// Recovered from GameAssembly.dll RVA 0x5A4940
			if (types == null) Init();
			return types.TryGetValue(type, out lastAccessedType) ? lastAccessedType : null;
		}

		internal static void Add(Type type, ES3Type es3Type)
		{
			// Recovered from GameAssembly.dll RVA 0x5A4A50
			if (types == null) Init();
			ES3Type previous = GetES3Type(type);
			if (previous != null && es3Type.priority < previous.priority) return;
			lock (_lock) types[type] = es3Type;
		}

		internal static ES3Type CreateES3Type(Type type, bool throwException = true)
		{
			// Recovered from GameAssembly.dll RVA 0x5A4C90
			if (type == null) throw new NullReferenceException();
			if (type.IsEnum) return new ES3Type_enum(type);
			ES3Type result;
			if (type.IsArray)
			{
			    int rank = type.GetArrayRank();
			    if (rank == 1) result = new ES3ArrayType(type);
			    else if (rank == 2) result = new ES32DArrayType(type);
			    else if (rank == 3) result = new ES33DArrayType(type);
			    else
			    {
			        if (!throwException) return null;
			        throw new NotSupportedException("Only arrays with up to three dimensions are supported by Easy Save.");
			    }
			}
			else if (type.IsGenericType && type.GetInterface(typeof(System.Collections.IEnumerable).Name) != null)
			{
			    Type definition = type.GetGenericTypeDefinition();
			    if (typeof(List<>).IsAssignableFrom(definition)) result = new ES3ListType(type);
			    else if (typeof(Dictionary<,>).IsAssignableFrom(definition)) result = new ES3DictionaryType(type);
			    else if (definition == typeof(Queue<>)) result = new ES3QueueType(type);
			    else if (definition == typeof(Stack<>)) result = new ES3StackType(type);
			    else if (definition == typeof(HashSet<>)) result = new ES3HashSetType(type);
			    else if (definition == typeof(Unity.Collections.NativeArray<>)) result = new ES3NativeArrayType(type);
			    else
			    {
			        if (!throwException) return null;
			        throw new NotSupportedException("Generic type \"" + type.ToString() + "\" is not supported by Easy Save.");
			    }
			}
			else
			{
			    if (ES3Reflection.IsPrimitive(type))
			        throw new TypeLoadException(types != null && types.Count > 0 ? "ES3Type for primitive could not be found, but the type list has been initialised and is not empty. Please contact Easy Save developers on mail@moodkie.com" : "ES3Type for primitive could not be found, and the type list is empty. Please contact Easy Save developers at http://www.moodkie.com/contact");
			    if (typeof(UnityEngine.Component).IsAssignableFrom(type)) result = new ES3ReflectedComponentType(type);
			    else if (type.IsValueType) result = new ES3ReflectedValueType(type);
			    else if (typeof(UnityEngine.ScriptableObject).IsAssignableFrom(type)) result = new ES3ReflectedScriptableObjectType(type);
			    else if (typeof(UnityEngine.Object).IsAssignableFrom(type)) result = new ES3ReflectedUnityObjectType(type);
			    else if (type.Name.StartsWith("Tuple`")) result = new ES3TupleType(type);
			    else result = new ES3ReflectedObjectType(type);
			}
			if (result == null) throw new NullReferenceException();
			if (result.type != null && !result.isUnsupported)
			{
			    Add(type, result);
			    return result;
			}
			if (!throwException) return null;
			throw new NotSupportedException(string.Format("ES3Type.type is null when trying to create an ES3Type for {0}, possibly because the element type is not supported.", type));
		}

		internal static void Init()
		{
			// Recovered from GameAssembly.dll RVA 0x5A5950
			lock (_lock)
			{
			    types = new Dictionary<Type, ES3Type>();
			    ES3Reflection.GetInstances<ES3Type>();
			    if (types == null || types.Count == 0) throw new TypeLoadException("Type list could not be initialised. Please contact Easy Save developers on mail@moodkie.com.");
			}
		}
	// Recovered original member RVA 0x5A5C40: .cctor
		
	}
}
