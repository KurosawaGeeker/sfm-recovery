using System;
using System.Collections.Generic;
using System.Reflection;

namespace ES3Internal
{
	public static class ES3Reflection
	{
		public struct ES3ReflectedMember
		{
			private FieldInfo fieldInfo;

			private PropertyInfo propertyInfo;

			public bool isProperty;

			public bool IsNull => fieldInfo == null && propertyInfo == null;

			public string Name => isProperty ? propertyInfo.Name : fieldInfo.Name;

			public Type MemberType => isProperty ? propertyInfo.PropertyType : fieldInfo.FieldType;

			public bool IsPublic => isProperty ? propertyInfo.GetGetMethod(true).IsPublic && propertyInfo.GetSetMethod(true).IsPublic : fieldInfo.IsPublic;

			public bool IsProtected => isProperty ? propertyInfo.GetGetMethod(true).IsFamily : fieldInfo.IsFamily;

			public bool IsStatic => isProperty ? propertyInfo.GetGetMethod(true).IsStatic : fieldInfo.IsStatic;

			public ES3ReflectedMember(object fieldPropertyInfo)
			{
			// Recovered from GameAssembly.dll RVA 0x59F670
			fieldInfo = null;
			propertyInfo = null;
			isProperty = false;
			if (fieldPropertyInfo == null) return;
			isProperty = typeof(PropertyInfo).IsAssignableFrom(fieldPropertyInfo.GetType());
			if (isProperty) propertyInfo = (PropertyInfo)fieldPropertyInfo;
			else fieldInfo = (FieldInfo)fieldPropertyInfo;
		}

			public void SetValue(object obj, object value)
			{
			// Recovered from GameAssembly.dll RVA 0x59FA90
			if (isProperty) propertyInfo.SetValue(obj, value);
			else fieldInfo.SetValue(obj, value);
		}

			public object GetValue(object obj)
			{
			// Recovered from GameAssembly.dll RVA 0x59FAE0
			return isProperty ? propertyInfo.GetValue(obj, null) : fieldInfo.GetValue(obj);
		}
		}

		public class ES3ReflectedMethod
		{
			private MethodInfo method;

			public ES3ReflectedMethod(Type type, string methodName, Type[] genericParameters, Type[] parameterTypes)
			{
			// Recovered from GameAssembly.dll RVA 0x59FB40
			method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static, null, CallingConventions.Any, parameterTypes, null).MakeGenericMethod(genericParameters);
		}

			public ES3ReflectedMethod(Type type, string methodName, Type[] genericParameters, Type[] parameterTypes, BindingFlags bindingAttr)
			{
			// Recovered from GameAssembly.dll RVA 0x59FC20
			method = type.GetMethod(methodName, bindingAttr, null, CallingConventions.Any, parameterTypes, null).MakeGenericMethod(genericParameters);
		}

			public object Invoke(object obj, object[] parameters = null)
			{
			// Recovered from GameAssembly.dll RVA 0x59FD00
			return method.Invoke(obj, BindingFlags.Default, null, parameters, null);
		}
		}

		public const string memberFieldPrefix = "m_";

		public const string componentTagFieldName = "tag";

		public const string componentNameFieldName = "name";

		public static readonly string[] excludedPropertyNames = new string[] { "runInEditMode", "useGUILayout", "hideFlags" };

		public static readonly Type serializableAttributeType = typeof(System.SerializableAttribute);

		public static readonly Type serializeFieldAttributeType = typeof(UnityEngine.SerializeField);

		public static readonly Type obsoleteAttributeType = typeof(System.ObsoleteAttribute);

		public static readonly Type nonSerializedAttributeType = typeof(System.NonSerializedAttribute);

		public static readonly Type es3SerializableAttributeType = typeof(ES3Serializable);

		public static readonly Type es3NonSerializableAttributeType = typeof(ES3NonSerializable);

		public static Type[] EmptyTypes = new Type[0];

		private static Assembly[] _assemblies = null;

		private static Assembly[] Assemblies
        {
            get
            {
                if (_assemblies != null) return _assemblies;
                string[] assemblyNames = new ES3Settings(true).assemblyNames;
                var selected = new List<Assembly>();
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                    if (System.Linq.Enumerable.Contains(assemblyNames, assembly.GetName().Name)) selected.Add(assembly);
                _assemblies = selected.ToArray();
                return _assemblies;
            }
        }

		public static Type[] GetElementTypes(Type type)
		{
			// Recovered from GameAssembly.dll RVA 0x59A160
			if (type.IsGenericType) return GetGenericArguments(type);
			if (type.IsArray) return new Type[] { GetElementType(type) };
			return null;
		}

		public static List<FieldInfo> GetSerializableFields(Type type, List<FieldInfo> serializableFields = null, bool safe = true, string[] memberNames = null, BindingFlags bindings = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
		{
			// Recovered from GameAssembly.dll RVA 0x59A2D0
			if (type == null) return new List<FieldInfo>();
			FieldInfo[] fields = type.GetFields(bindings);
			if (serializableFields == null) serializableFields = new List<FieldInfo>();
			foreach (FieldInfo field in fields)
			{
			    string name = field.Name;
			    if (memberNames != null && !System.Linq.Enumerable.Contains(memberNames, name)) continue;
			    Type fieldType = field.FieldType;
			    if (Attribute.IsDefined(field, es3SerializableAttributeType, true)) { serializableFields.Add(field); continue; }
			    if (Attribute.IsDefined(field, es3NonSerializableAttributeType, true)) continue;
			    if (safe && !field.IsPublic && !Attribute.IsDefined(field, serializeFieldAttributeType, true)) continue;
			    if (field.IsLiteral || field.IsInitOnly) continue;
			    if (fieldType == type && !typeof(UnityEngine.Object).IsAssignableFrom(fieldType)) continue;
			    if (Attribute.IsDefined(field, nonSerializedAttributeType, true)) continue;
			    if (Attribute.IsDefined(field, obsoleteAttributeType, true)) continue;
			    if (!TypeIsSerializable(fieldType)) continue;
			    if (safe && name.StartsWith("m_") && field.DeclaringType.Namespace != null && field.DeclaringType.Namespace.Contains("UnityEngine")) continue;
			    serializableFields.Add(field);
			}
			Type baseType = type.BaseType;
			if (baseType != null && baseType != typeof(object) && baseType != typeof(UnityEngine.Object))
			    GetSerializableFields(baseType, serializableFields, safe, memberNames);
			return serializableFields;
		}

		public static List<PropertyInfo> GetSerializableProperties(Type type, List<PropertyInfo> serializableProperties = null, bool safe = true, string[] memberNames = null, BindingFlags bindings = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
		{
			// Recovered from GameAssembly.dll RVA 0x59AA20
			bool isComponent = typeof(UnityEngine.Component).IsAssignableFrom(type);
			PropertyInfo[] properties = type.GetProperties(safe ? bindings : bindings | BindingFlags.NonPublic);
			if (serializableProperties == null) serializableProperties = new List<PropertyInfo>();
			foreach (PropertyInfo property in properties)
			{
			    if (Attribute.IsDefined(property, es3SerializableAttributeType, true)) { serializableProperties.Add(property); continue; }
			    if (Attribute.IsDefined(property, es3NonSerializableAttributeType, true)) continue;
			    string name = property.Name;
			    if (System.Linq.Enumerable.Contains(excludedPropertyNames, name)) continue;
			    if (memberNames != null && !System.Linq.Enumerable.Contains(memberNames, name)) continue;
			    if (safe && !Attribute.IsDefined(property, serializeFieldAttributeType, true) && !Attribute.IsDefined(property, es3SerializableAttributeType, true)) continue;
			    Type propertyType = property.PropertyType;
			    if (propertyType == type && !typeof(UnityEngine.Object).IsAssignableFrom(propertyType)) continue;
			    if (!property.CanRead || !property.CanWrite) continue;
			    if (property.GetIndexParameters().Length != 0 && !propertyType.IsArray) continue;
			    if (!TypeIsSerializable(propertyType)) continue;
			    if (isComponent && (name == "tag" || name == "name")) continue;
			    if (Attribute.IsDefined(property, obsoleteAttributeType, true)) continue;
			    if (Attribute.IsDefined(property, nonSerializedAttributeType, true)) continue;
			    serializableProperties.Add(property);
			}
			Type baseType = type.BaseType;
			if (baseType != null && baseType != typeof(object)) GetSerializableProperties(baseType, serializableProperties, safe, memberNames);
			return serializableProperties;
		}

		public static bool TypeIsSerializable(Type type)
		{
			// Recovered from GameAssembly.dll RVA 0x59B1C0
			if (type == null || type.IsDefined(es3NonSerializableAttributeType, true)) return false;
			if (IsPrimitive(type) || type.IsValueType || typeof(UnityEngine.Component).IsAssignableFrom(type) || typeof(UnityEngine.ScriptableObject).IsAssignableFrom(type)) return true;
			ES3Types.ES3Type serializer = ES3TypeMgr.GetOrCreateES3Type(type, false);
			if (serializer != null && !serializer.isUnsupported) return true;
			if (type.IsArray) return TypeIsSerializable(type.GetElementType());
			foreach (Type argument in type.GetGenericArguments()) if (!TypeIsSerializable(argument)) return false;
			// Original native returns false after all generic arguments as well.
			return false;
		}

		public static object CreateInstance(Type type)
		{
			// Recovered from GameAssembly.dll RVA 0x59B500
			if (typeof(UnityEngine.Component).IsAssignableFrom(type)) return ES3Types.ES3ComponentType.CreateComponent(type);
			if (typeof(UnityEngine.ScriptableObject).IsAssignableFrom(type)) return UnityEngine.ScriptableObject.CreateInstance(type);
			if (HasParameterlessConstructor(type)) return Activator.CreateInstance(type, false);
			return System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
		}

		public static object CreateInstance(Type type, params object[] args)
		{
			// Recovered from GameAssembly.dll RVA 0x59B750
			if (typeof(UnityEngine.Component).IsAssignableFrom(type)) return ES3Types.ES3ComponentType.CreateComponent(type);
			if (typeof(UnityEngine.ScriptableObject).IsAssignableFrom(type)) return UnityEngine.ScriptableObject.CreateInstance(type);
			return Activator.CreateInstance(type, BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance, null, args, null);
		}

		public static Array ArrayCreateInstance(Type type, int length)
		{
			// Recovered from GameAssembly.dll RVA 0x59B980
			return Array.CreateInstance(type, length);
		}

		public static Array ArrayCreateInstance(Type type, int[] dimensions)
		{
			// Recovered from GameAssembly.dll RVA 0x59BA00
			return Array.CreateInstance(type, dimensions);
		}

		public static Type MakeGenericType(Type type, Type genericParam)
		{
			return null;
		}

		public static ES3ReflectedMember[] GetSerializableMembers(Type type, bool safe = true, string[] memberNames = null)
		{
			// Recovered from GameAssembly.dll RVA 0x59BAD0
			if (type == null) return new ES3ReflectedMember[0];
			List<FieldInfo> fields = GetSerializableFields(type, new List<FieldInfo>(), safe, memberNames);
			List<PropertyInfo> properties = GetSerializableProperties(type, new List<PropertyInfo>(), safe, memberNames);
			var result = new ES3ReflectedMember[fields.Count + properties.Count];
			for (int i = 0; i < fields.Count; ++i) result[i] = new ES3ReflectedMember(fields[i]);
			for (int i = 0; i < properties.Count; ++i) result[fields.Count + i] = new ES3ReflectedMember(properties[i]);
			return result;
		}

		public static ES3ReflectedMember GetES3ReflectedProperty(Type type, string propertyName)
		{
			return default;
		}

		public static ES3ReflectedMember GetES3ReflectedMember(Type type, string fieldName)
		{
			return default;
		}

		public static IList<T> GetInstances<T>()
		{
			// Recovered from GameAssembly.dll RVA 0x75B990
			var result = new List<T>();
			foreach (Assembly assembly in Assemblies)
			    foreach (Type candidate in assembly.GetTypes())
			        if (typeof(T).IsAssignableFrom(candidate) && HasParameterlessConstructor(candidate) && !candidate.IsAbstract)
			            result.Add((T)Activator.CreateInstance(candidate));
			return result;
		}

		public static IList<Type> GetDerivedTypes(Type derivedType)
		{
			return null;
		}

		public static bool IsAssignableFrom(Type a, Type b)
		{
			// Recovered from GameAssembly.dll RVA 0x59C590
			return a.IsAssignableFrom(b);
		}

		public static Type GetGenericTypeDefinition(Type type)
		{
			// Recovered from GameAssembly.dll RVA 0x59C5C0
			return type.GetGenericTypeDefinition();
		}

		public static Type[] GetGenericArguments(Type type)
		{
			// Recovered from GameAssembly.dll RVA 0x59C5F0
			return type.GetGenericArguments();
		}

		public static int GetArrayRank(Type type)
		{
			// Recovered from GameAssembly.dll RVA 0x59C620
			return type.GetArrayRank();
		}

		public static string GetAssemblyQualifiedName(Type type)
		{
			// Recovered from GameAssembly.dll RVA 0x59C650
			return type.AssemblyQualifiedName;
		}

		public static ES3ReflectedMethod GetMethod(Type type, string methodName, Type[] genericParameters, Type[] parameterTypes)
		{
			return null;
		}

		public static bool TypeIsArray(Type type)
		{
			// Recovered from GameAssembly.dll RVA 0x59C7B0
			return type.IsArray;
		}

		public static Type GetElementType(Type type)
		{
			// Recovered from GameAssembly.dll RVA 0x59C7E0
			return type.GetElementType();
		}

		public static bool IsAbstract(Type type)
		{
			// Recovered from GameAssembly.dll RVA 0x59C810
			return type.IsAbstract;
		}

		public static bool IsInterface(Type type)
		{
			// Recovered from GameAssembly.dll RVA 0x59C840
			return type.IsInterface;
		}

		public static bool IsGenericType(Type type)
		{
			// Recovered from GameAssembly.dll RVA 0x59C860
			return type.IsGenericType;
		}

		public static bool IsValueType(Type type)
		{
			// Recovered from GameAssembly.dll RVA 0x59C890
			return type.IsValueType;
		}

		public static bool IsEnum(Type type)
		{
			// Recovered from GameAssembly.dll RVA 0x59C8C0
			return type.IsEnum;
		}

		public static bool HasParameterlessConstructor(Type type)
		{
			// Recovered from GameAssembly.dll RVA 0x59C8F0
			if (type.IsValueType) return true;
			foreach (ConstructorInfo constructor in type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			    if (constructor.GetParameters().Length == 0) return true;
			return false;
		}

		public static ConstructorInfo GetParameterlessConstructor(Type type)
		{
			// Recovered from GameAssembly.dll RVA 0x59CA80
			foreach (ConstructorInfo constructor in type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			    if (constructor.GetParameters().Length == 0) return constructor;
			return null;
		}

		public static string GetShortAssemblyQualifiedName(Type type)
		{
			return null;
		}

		public static PropertyInfo GetProperty(Type type, string propertyName)
		{
			return null;
		}

		public static FieldInfo GetField(Type type, string fieldName)
		{
			return null;
		}

		public static MethodInfo[] GetMethods(Type type, string methodName)
		{
			// Recovered from GameAssembly.dll RVA 0x59CF50
			return System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Where(type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static), method => method.Name == methodName));
		}

		public static bool IsPrimitive(Type type)
		{
			// Recovered from GameAssembly.dll RVA 0x59D120
			return type.IsPrimitive || type == typeof(string) || type == typeof(decimal);
		}

		public static bool AttributeIsDefined(MemberInfo info, Type attributeType)
		{
			return false;
		}

		public static bool AttributeIsDefined(Type type, Type attributeType)
		{
			return false;
		}

		public static bool ImplementsInterface(Type type, Type interfaceType)
		{
			return false;
		}

		public static Type BaseType(Type type)
		{
			// Recovered from GameAssembly.dll RVA 0x59D2F0
			return type.BaseType;
		}

		public static Type GetType(string typeString)
		{
			// Recovered from GameAssembly.dll RVA 0x59D320
			switch (typeString)
			{
			    case "UnityEngine.Object": return typeof(UnityEngine.Object);
			    case "string": return typeof(string);
			    case "decimal": return typeof(System.Decimal);
			    case "Component": return typeof(UnityEngine.Component);
			    case "Texture2D": return typeof(UnityEngine.Texture2D);
			    case "System.Object": return typeof(object);
			    case "ushort": return typeof(ushort);
			    case "byte": return typeof(byte);
			    case "Vector2": return typeof(UnityEngine.Vector2);
			    case "Vector3": return typeof(UnityEngine.Vector3);
			    case "Vector4": return typeof(UnityEngine.Vector4);
			    case "int": return typeof(int);
			    case "ulong": return typeof(ulong);
			    case "double": return typeof(double);
			    case "float": return typeof(float);
			    case "char": return typeof(char);
			    case "Transform": return typeof(UnityEngine.Transform);
			    case "short": return typeof(short);
			    case "long": return typeof(long);
			    case "GameObject": return typeof(UnityEngine.GameObject);
			    case "bool": return typeof(bool);
			    case "uint": return typeof(uint);
			    case "Material": return typeof(UnityEngine.Material);
			    case "MeshFilter": return typeof(UnityEngine.MeshFilter);
			    case "Color": return typeof(UnityEngine.Color);
			    case "sbyte": return typeof(sbyte);
			    default: return Type.GetType(typeString);
			}
		}

		public static string GetTypeString(Type type)
		{
			// Recovered from GameAssembly.dll RVA 0x59E070
			if (type == typeof(bool)) return "bool";
			if (type == typeof(byte)) return "byte";
			if (type == typeof(sbyte)) return "sbyte";
			if (type == typeof(char)) return "char";
			if (type == typeof(System.Decimal)) return "decimal";
			if (type == typeof(double)) return "double";
			if (type == typeof(float)) return "float";
			if (type == typeof(int)) return "int";
			if (type == typeof(long)) return "long";
			if (type == typeof(ulong)) return "ulong";
			if (type == typeof(uint)) return "uint";
			if (type == typeof(short)) return "short";
			if (type == typeof(ushort)) return "ushort";
			if (type == typeof(string)) return "string";
			if (type == typeof(UnityEngine.Vector2)) return "Vector2";
			if (type == typeof(UnityEngine.Vector3)) return "Vector3";
			if (type == typeof(UnityEngine.Vector4)) return "Vector4";
			if (type == typeof(UnityEngine.Color)) return "Color";
			if (type == typeof(UnityEngine.Transform)) return "Transform";
			if (type == typeof(UnityEngine.Component)) return "Component";
			if (type == typeof(UnityEngine.GameObject)) return "GameObject";
			if (type == typeof(UnityEngine.MeshFilter)) return "MeshFilter";
			if (type == typeof(UnityEngine.Material)) return "Material";
			if (type == typeof(UnityEngine.Texture2D)) return "Texture2D";
			if (type == typeof(UnityEngine.Object)) return "UnityEngine.Object";
			if (type == typeof(object)) return "System.Object";
			if (IsPrimitive(type)) return type.Name;
			return type.FullName + "," + type.Assembly.GetName().Name;
		}
	// Recovered original member RVA 0x59EC20: .cctor
		
	}
}
