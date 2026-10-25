using System;
using System.Collections.Generic;

namespace ES3Internal
{
	internal static class ES3Binary
	{
		internal const string ObjectTerminator = ".";

		internal static readonly Dictionary<ES3SpecialByte, Type> IdToType;

		internal static readonly Dictionary<Type, ES3SpecialByte> TypeToId;

		internal static ES3SpecialByte TypeToByte(Type type)
		{
			return ES3SpecialByte.Null;
		}

		internal static Type ByteToType(ES3SpecialByte b)
		{
			return null;
		}

		internal static Type ByteToType(byte b)
		{
			return null;
		}

		internal static bool IsPrimitive(ES3SpecialByte b)
		{
			return false;
		}
	}
}
