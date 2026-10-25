using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3Type_enum : ES3Type
	{
		public static ES3Type Instance;

		private Type underlyingType;

		// Recovered original constructor RVA 0x5559F0.
        public ES3Type_enum(Type type)
            : base(type)
        {
            isPrimitive = true;
            isEnum = true;
            Instance = this;
            underlyingType = Enum.GetUnderlyingType(type);
        }

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x555BD0
			if (underlyingType == typeof(int))
			{
			    if (writer is null) throw new NullReferenceException();
			    writer.WritePrimitive((int)obj);
			    return;
			}
			if (underlyingType == typeof(bool))
			{
			    if (writer is null) throw new NullReferenceException();
			    writer.WritePrimitive((bool)obj);
			    return;
			}
			if (underlyingType == typeof(byte))
			{
			    if (writer is null) throw new NullReferenceException();
			    writer.WritePrimitive((byte)obj);
			    return;
			}
			if (underlyingType == typeof(char))
			{
			    if (writer is null) throw new NullReferenceException();
			    writer.WritePrimitive((char)obj);
			    return;
			}
			if (underlyingType == typeof(decimal))
			{
			    if (writer is null) throw new NullReferenceException();
			    writer.WritePrimitive((decimal)obj);
			    return;
			}
			if (underlyingType == typeof(double))
			{
			    if (writer is null) throw new NullReferenceException();
			    writer.WritePrimitive((double)obj);
			    return;
			}
			if (underlyingType == typeof(float))
			{
			    if (writer is null) throw new NullReferenceException();
			    writer.WritePrimitive((float)obj);
			    return;
			}
			if (underlyingType == typeof(long))
			{
			    if (writer is null) throw new NullReferenceException();
			    writer.WritePrimitive((long)obj);
			    return;
			}
			if (underlyingType == typeof(sbyte))
			{
			    if (writer is null) throw new NullReferenceException();
			    writer.WritePrimitive((sbyte)obj);
			    return;
			}
			if (underlyingType == typeof(short))
			{
			    if (writer is null) throw new NullReferenceException();
			    writer.WritePrimitive((short)obj);
			    return;
			}
			if (underlyingType == typeof(uint))
			{
			    if (writer is null) throw new NullReferenceException();
			    writer.WritePrimitive((uint)obj);
			    return;
			}
			if (underlyingType == typeof(ulong))
			{
			    if (writer is null) throw new NullReferenceException();
			    writer.WritePrimitive((ulong)obj);
			    return;
			}
			if (underlyingType == typeof(ushort))
			{
			    if (writer is null) throw new NullReferenceException();
			    writer.WritePrimitive((ushort)obj);
			    return;
			}
			throw new InvalidCastException("The underlying type " + (underlyingType == null ? null : underlyingType.ToString()) + " of Enum " + (type == null ? null : type.ToString()) + " is not supported");
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x925C30
			if (underlyingType == typeof(int)) return Enum.ToObject(type, reader.Read_int());
			if (underlyingType == typeof(bool)) return Enum.ToObject(type, (object)reader.Read_bool());
			if (underlyingType == typeof(byte)) return Enum.ToObject(type, reader.Read_byte());
			if (underlyingType == typeof(char)) return Enum.ToObject(type, (ushort)reader.Read_char());
			if (underlyingType == typeof(decimal)) return Enum.ToObject(type, (object)reader.Read_decimal());
			if (underlyingType == typeof(double)) return Enum.ToObject(type, (object)reader.Read_double());
			if (underlyingType == typeof(float)) return Enum.ToObject(type, (object)reader.Read_float());
			if (underlyingType == typeof(long)) return Enum.ToObject(type, reader.Read_long());
			if (underlyingType == typeof(sbyte)) return Enum.ToObject(type, reader.Read_sbyte());
			if (underlyingType == typeof(short)) return Enum.ToObject(type, reader.Read_short());
			if (underlyingType == typeof(uint)) return Enum.ToObject(type, reader.Read_uint());
			if (underlyingType == typeof(ulong)) return Enum.ToObject(type, reader.Read_ulong());
			if (underlyingType == typeof(ushort)) return Enum.ToObject(type, reader.Read_ushort());
			throw new InvalidCastException("The underlying type " + (underlyingType == null ? null : underlyingType.ToString()) + " of Enum " + (type == null ? null : type.ToString()) + " is not supported");
		}
	}
}
