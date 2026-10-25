using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	internal class ES3ReflectedComponentType : ES3ComponentType
	{
		// Recovered original constructor RVA 0x559490.
		public ES3ReflectedComponentType(Type type)
			: base(type)
		{
			isReflectedType = true;
			GetMembers(true);
		}

		protected override void WriteComponent(object obj, ES3Writer writer)
		{
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
		}
	}
}
