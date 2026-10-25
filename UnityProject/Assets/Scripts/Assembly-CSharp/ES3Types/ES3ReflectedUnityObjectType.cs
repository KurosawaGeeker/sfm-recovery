using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	internal class ES3ReflectedUnityObjectType : ES3UnityObjectType
	{
		// Recovered original constructor RVA 0x559490.
		public ES3ReflectedUnityObjectType(Type type)
			: base(type)
		{
			isReflectedType = true;
			GetMembers(true);
		}

		protected override void WriteUnityObject(object obj, ES3Writer writer)
		{
		}

		protected override object ReadUnityObject<T>(ES3Reader reader)
		{
			return null;
		}

		protected override void ReadUnityObject<T>(ES3Reader reader, object obj)
		{
		}
	}
}
