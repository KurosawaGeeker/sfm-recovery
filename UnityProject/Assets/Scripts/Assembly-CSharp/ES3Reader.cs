using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using ES3Types;

public abstract class ES3Reader : IDisposable
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class ES3ReaderPropertyEnumerator
	{
		[CompilerGenerated]
		private sealed class _003CGetEnumerator_003Ed__2 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public ES3ReaderPropertyEnumerator _003C_003E4__this;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
			// Recovered from GameAssembly.dll RVA 0x3CB9B0
			return _003C_003E2__current;
		}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
			// Recovered from GameAssembly.dll RVA 0x3CB9B0
			return _003C_003E2__current;
		}
			}

			[DebuggerHidden]
			public _003CGetEnumerator_003Ed__2(int _003C_003E1__state)
			{
			// Recovered from GameAssembly.dll RVA 0x3CB800
			this._003C_003E1__state = _003C_003E1__state;
		}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
			// Recovered from GameAssembly.dll RVA 0x3CB810
			if (_003C_003E1__state != 0 && _003C_003E1__state != 1 && _003C_003E1__state != 2) return false;
			_003C_003E1__state = -1;
			ES3Reader reader = _003C_003E4__this.reader;
			if (reader.overridePropertiesName != null)
			{
			    _003C_003E2__current = reader.overridePropertiesName;
			    reader.overridePropertiesName = null;
			    _003C_003E1__state = 1;
			    return true;
			}
			string name = reader.ReadPropertyName();
			if (name == null) return false;
			_003C_003E2__current = name;
			_003C_003E1__state = 2;
			return true;
		}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
			// Recovered from GameAssembly.dll RVA 0x3CB9C0
			throw new NotSupportedException();
		}
		}

		public ES3Reader reader;

		public ES3ReaderPropertyEnumerator(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x3CAB80
			this.reader = reader;
		}

		[IteratorStateMachine(typeof(_003CGetEnumerator_003Ed__2))]
		public IEnumerator GetEnumerator()
		{
			// Recovered from GameAssembly.dll RVA 0x3CB760
			return new _003CGetEnumerator_003Ed__2(0) { _003C_003E4__this = this };
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public class ES3ReaderRawEnumerator
	{
		[CompilerGenerated]
		private sealed class _003CGetEnumerator_003Ed__2 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public ES3ReaderRawEnumerator _003C_003E4__this;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
			// Recovered from GameAssembly.dll RVA 0x3CB9B0
			return _003C_003E2__current;
		}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
			// Recovered from GameAssembly.dll RVA 0x3CB9B0
			return _003C_003E2__current;
		}
			}

			[DebuggerHidden]
			public _003CGetEnumerator_003Ed__2(int _003C_003E1__state)
			{
			// Recovered from GameAssembly.dll RVA 0x3CB800
			this._003C_003E1__state = _003C_003E1__state;
		}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
			// Recovered from GameAssembly.dll RVA 0x3CBAA0
			if (_003C_003E1__state != 0 && _003C_003E1__state != 1) return false;
			_003C_003E1__state = -1;
			while (true)
			{
			    ES3Reader reader = _003C_003E4__this.reader;
			    string key = reader.ReadPropertyName();
			    if (key == null) return false;
			    // MethodInfo0x2D527C0 fixes object, despite the shared pseudocode label bool.
			    System.Type type = _003C_003E4__this.reader.ReadTypeFromHeader<object>();
			    byte[] bytes = _003C_003E4__this.reader.ReadElement(false);
			    _003C_003E4__this.reader.ReadKeySuffix();
			    if (type == null) continue;
			    _003C_003E2__current = new System.Collections.Generic.KeyValuePair<string, ES3Internal.ES3Data>(key, new ES3Internal.ES3Data(type, bytes));
			    _003C_003E1__state = 1;
			    return true;
			}
		}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
			// Recovered from GameAssembly.dll RVA 0x3CBCA0
			throw new System.NotSupportedException();
		}
		}

		public ES3Reader reader;

		public ES3ReaderRawEnumerator(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x3CAB80
			this.reader = reader;
		}

		[IteratorStateMachine(typeof(_003CGetEnumerator_003Ed__2))]
		public IEnumerator GetEnumerator()
		{
			// Recovered from GameAssembly.dll RVA 0x3CBA00
			return new _003CGetEnumerator_003Ed__2(0) { _003C_003E4__this = this };
		}
	}

	public ES3Settings settings;

	protected int serializationDepth;

	internal string overridePropertiesName;

	public virtual ES3ReaderPropertyEnumerator Properties => new ES3ReaderPropertyEnumerator(this);

	internal virtual ES3ReaderRawEnumerator RawEnumerator => new ES3ReaderRawEnumerator(this);

	internal abstract int Read_int();

	internal abstract float Read_float();

	internal abstract bool Read_bool();

	internal abstract char Read_char();

	internal abstract decimal Read_decimal();

	internal abstract double Read_double();

	internal abstract long Read_long();

	internal abstract ulong Read_ulong();

	internal abstract byte Read_byte();

	internal abstract sbyte Read_sbyte();

	internal abstract short Read_short();

	internal abstract ushort Read_ushort();

	internal abstract uint Read_uint();

	internal abstract string Read_string();

	internal abstract byte[] Read_byteArray();

	internal abstract long Read_ref();

	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract string ReadPropertyName();

	protected abstract Type ReadKeyPrefix(bool ignore = false);

	protected abstract void ReadKeySuffix();

	internal abstract byte[] ReadElement(bool skip = false);

	public abstract void Dispose();

	internal virtual bool Goto(string key)
	{
			// Recovered from GameAssembly.dll RVA 0x3CAA70
			if (key == null) throw new ArgumentNullException("Key cannot be NULL when loading data.");
			while (true)
			{
			    string candidate = ReadPropertyName();
			    if (candidate == key) return true;
			    if (candidate == null) return false;
			    Skip();
			}
		}

	internal virtual bool StartReadObject()
	{
		return false;
	}

	internal virtual void EndReadObject()
	{
	}

	internal abstract bool StartReadDictionary();

	internal abstract void EndReadDictionary();

	internal abstract bool StartReadDictionaryKey();

	internal abstract void EndReadDictionaryKey();

	internal abstract void StartReadDictionaryValue();

	internal abstract bool EndReadDictionaryValue();

	internal abstract bool StartReadCollection();

	internal abstract void EndReadCollection();

	internal abstract bool StartReadCollectionItem();

	internal abstract bool EndReadCollectionItem();

	internal ES3Reader(ES3Settings settings, bool readHeaderAndFooter = true)
	{
			// Recovered from GameAssembly.dll RVA 0x3CAB80
			this.settings = settings;
		}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void Skip()
	{
			// Recovered from GameAssembly.dll RVA 0x3CAD00
			ReadElement(true);
		}

	public virtual T Read<T>()
	{
			// Recovered from GameAssembly.dll RVA 0x7537F0
			return Read<T>(ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(T)));
		}

	public virtual void ReadInto<T>(object obj)
	{
			// Recovered from GameAssembly.dll RVA 0x757620
			ReadInto<T>(obj, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(T)));
		}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public T ReadProperty<T>()
	{
			// Recovered from GameAssembly.dll RVA 0x758E10
			return ReadProperty<T>(ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(T)));
		}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public T ReadProperty<T>(ES3Type type)
	{
			// Recovered from GameAssembly.dll RVA 0x7591F0
			ReadPropertyName();
			return Read<T>(type);
		}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public long ReadRefProperty()
	{
			// Recovered from GameAssembly.dll RVA 0x3CAD20
			ReadPropertyName();
			return Read_ref();
		}

	internal Type ReadType()
	{
			// Recovered from GameAssembly.dll RVA 0x3CAD60
			return ES3Internal.ES3Reflection.GetType(Read<string>());
		}

	public object SetPrivateProperty(string name, object value, object objectContainingProperty)
	{
		return null;
	}

	public object SetPrivateField(string name, object value, object objectContainingField)
	{
		return null;
	}

	public virtual T Read<T>(string key)
	{
			// Recovered from GameAssembly.dll RVA 0x755940
			if (!Goto(key)) throw new KeyNotFoundException("Key \"" + key + "\" was not found in file \"" + settings.FullPath + "\". Use Load<T>(key, defaultValue) if you want to return a default value if the key does not exist.");
			return Read<T>(ES3Internal.ES3TypeMgr.GetOrCreateES3Type(ReadTypeFromHeader<T>()));
		}

	public virtual T Read<T>(string key, T defaultValue)
	{
			// Recovered from GameAssembly.dll RVA 0x757180
			if (!Goto(key)) return defaultValue;
			return Read<T>(ES3Internal.ES3TypeMgr.GetOrCreateES3Type(ReadTypeFromHeader<T>()));
		}

	public virtual void ReadInto<T>(string key, T obj) where T : class
	{
			// Recovered from GameAssembly.dll RVA 0x757920
			if (!Goto(key)) throw new KeyNotFoundException("Key \"" + key + "\" was not found in file \"" + settings.FullPath + "\"");
			ReadInto<T>(obj, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(ReadTypeFromHeader<T>()));
		}

	protected virtual void ReadObject<T>(object obj, ES3Type type)
	{
			// Recovered from GameAssembly.dll RVA 0x758B30
			if (StartReadObject()) return;
			type.ReadInto<T>(this, obj);
			EndReadObject();
		}

	protected virtual T ReadObject<T>(ES3Type type)
	{
			// Recovered from GameAssembly.dll RVA 0x758260
			if (StartReadObject()) return default;
			object value = type.Read<T>(this);
			EndReadObject();
			return (T)value;
		}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual T Read<T>(ES3Type type)
	{
			// Recovered from GameAssembly.dll RVA 0x755660
			if (type == null || type.isUnsupported) throw new NotSupportedException("Type of " + (type == null ? null : type.ToString()) + " is not currently supported, and could not be loaded using reflection.");
			if (type.isPrimitive) return (T)type.Read<T>(this);
			if (type.isCollection) return (T)((ES3CollectionType)type).Read(this);
			if (type.isDictionary) return (T)((ES3DictionaryType)type).Read(this);
			return ReadObject<T>(type);
		}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void ReadInto<T>(object obj, ES3Type type)
	{
			// Recovered from GameAssembly.dll RVA 0x757730
			if (type == null || type.isUnsupported) throw new NotSupportedException("Type of " + obj.GetType().ToString() + " is not currently supported, and could not be loaded using reflection.");
			if (type.isCollection) ((ES3CollectionType)type).ReadInto(this, obj);
			else if (type.isDictionary) ((ES3DictionaryType)type).ReadInto(this, obj);
			else ReadObject<T>(obj, type);
		}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal Type ReadTypeFromHeader<T>()
	{
			// Recovered from GameAssembly.dll RVA 0x759550
			if (typeof(T) == typeof(object)) return ReadKeyPrefix(false);
			if (!settings.typeChecking)
			{
			    ReadKeyPrefix(true);
			    return typeof(T);
			}
			Type actual = ReadKeyPrefix(false);
			if (actual != typeof(T)) throw new InvalidOperationException("Trying to load data of type " + typeof(T).ToString() + ", but data contained in file is type of " + (actual == null ? null : actual.ToString()) + ".");
			return actual;
		}

	public static ES3Reader Create()
	{
			// Recovered from GameAssembly.dll RVA 0x3CB210
			return Create(new ES3Settings(true));
		}

	public static ES3Reader Create(string filePath)
	{
			// Recovered from GameAssembly.dll RVA 0x3CB270
			return Create(new ES3Settings(filePath));
		}

	public static ES3Reader Create(string filePath, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3CB330
			return Create(new ES3Settings(filePath, settings));
		}

	public static ES3Reader Create(ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3CB400
			Stream stream = ES3Internal.ES3Stream.CreateStream(settings, ES3Internal.ES3FileMode.Read);
			return stream != null && settings.format == ES3.Format.JSON ? new ES3Internal.ES3JSONReader(stream, settings, true) : null;
		}

	public static ES3Reader Create(byte[] bytes)
	{
			// Recovered from GameAssembly.dll RVA 0x3CB4B0
			return Create(bytes, new ES3Settings(true));
		}

	public static ES3Reader Create(byte[] bytes, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3CB520
			Stream stream = ES3Internal.ES3Stream.CreateStream(new MemoryStream(bytes), settings, ES3Internal.ES3FileMode.Read);
			return stream != null && settings.format == ES3.Format.JSON ? new ES3Internal.ES3JSONReader(stream, settings, true) : null;
		}

	internal static ES3Reader Create(Stream stream, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3CB600
			stream = ES3Internal.ES3Stream.CreateStream(stream, settings, ES3Internal.ES3FileMode.Read);
			return settings.format == ES3.Format.JSON ? new ES3Internal.ES3JSONReader(stream, settings, true) : null;
		}

	internal static ES3Reader Create(Stream stream, ES3Settings settings, bool readHeaderAndFooter)
	{
			// Recovered from GameAssembly.dll RVA 0x3CB6B0
			return settings.format == ES3.Format.JSON ? new ES3Internal.ES3JSONReader(stream, settings, readHeaderAndFooter) : null;
		}
}
