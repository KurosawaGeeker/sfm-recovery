using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	internal class ES3ReflectedScriptableObjectType : ES3ScriptableObjectType
	{
		// Recovered original constructor RVA 0x559490.
		public ES3ReflectedScriptableObjectType(Type type)
			: base(type)
		{
			isReflectedType = true;
			GetMembers(true);
		}

		protected override void WriteScriptableObject(object obj, ES3Writer writer)
		{
		}

		protected override void ReadScriptableObject<T>(ES3Reader reader, object obj)
		{
		}
	}
}
