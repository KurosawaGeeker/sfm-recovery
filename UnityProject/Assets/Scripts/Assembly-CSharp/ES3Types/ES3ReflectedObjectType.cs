using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	internal class ES3ReflectedObjectType : ES3ObjectType
	{
		// Recovered original constructor RVA 0x5594D0.
		public ES3ReflectedObjectType(Type type)
			: base(type)
		{
			isReflectedType = true;
			GetMembers(true);
		}

		protected override void WriteObject(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x5594C0
			WriteProperties(obj, writer);
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x7598D0
			object obj = ES3Internal.ES3Reflection.CreateInstance(type);
			ReadProperties(reader, obj);
			return obj;
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			// Recovered from GameAssembly.dll RVA 0x7598B0
			ReadProperties(reader, obj);
		}
	}
}
