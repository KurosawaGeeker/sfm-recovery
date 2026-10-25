using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using ES3Types;
using UnityEngine;

public abstract class ES3Writer : IDisposable
{
	public ES3Settings settings;

	protected HashSet<string> keysToDelete;

	internal bool writeHeaderAndFooter;

	internal bool overwriteKeys;

	protected int serializationDepth;

	internal abstract void WriteNull();

	internal virtual void StartWriteFile()
	{
	}

	internal virtual void EndWriteFile()
	{
	}

	internal virtual void StartWriteObject(string name)
	{
	}

	internal virtual void EndWriteObject(string name)
	{
	}

	internal virtual void StartWriteProperty(string name)
	{
	}

	internal virtual void EndWriteProperty(string name)
	{
	}

	internal virtual void StartWriteCollection()
	{
	}

	internal virtual void EndWriteCollection()
	{
	}

	internal abstract void StartWriteCollectionItem(int index);

	internal abstract void EndWriteCollectionItem(int index);

	internal abstract void StartWriteDictionary();

	internal abstract void EndWriteDictionary();

	internal abstract void StartWriteDictionaryKey(int index);

	internal abstract void EndWriteDictionaryKey(int index);

	internal abstract void StartWriteDictionaryValue(int index);

	internal abstract void EndWriteDictionaryValue(int index);

	public abstract void Dispose();

	internal abstract void WriteRawProperty(string name, byte[] bytes);

	internal abstract void WritePrimitive(int value);

	internal abstract void WritePrimitive(float value);

	internal abstract void WritePrimitive(bool value);

	internal abstract void WritePrimitive(decimal value);

	internal abstract void WritePrimitive(double value);

	internal abstract void WritePrimitive(long value);

	internal abstract void WritePrimitive(ulong value);

	internal abstract void WritePrimitive(uint value);

	internal abstract void WritePrimitive(byte value);

	internal abstract void WritePrimitive(sbyte value);

	internal abstract void WritePrimitive(short value);

	internal abstract void WritePrimitive(ushort value);

	internal abstract void WritePrimitive(char value);

	internal abstract void WritePrimitive(string value);

	internal abstract void WritePrimitive(byte[] value);

	protected ES3Writer(ES3Settings settings, bool writeHeaderAndFooter, bool overwriteKeys)
	{
			// Recovered from GameAssembly.dll RVA 0x3D5D00
			keysToDelete = new HashSet<string>();
			this.writeHeaderAndFooter = true;
			this.overwriteKeys = true;
			this.settings = settings;
			this.overwriteKeys = overwriteKeys;
			this.writeHeaderAndFooter = writeHeaderAndFooter;
		}

	internal virtual void Write(string key, Type type, byte[] value)
	{
			// Recovered from GameAssembly.dll RVA 0x3D5E50
			StartWriteProperty(key);
			StartWriteObject(key);
			WriteType(type);
			WriteRawProperty("value", value);
			EndWriteObject(key);
			EndWriteProperty(key);
			MarkKeyForDeletion(key);
		}

	public virtual void Write<T>(string key, object value)
	{
			// Recovered from GameAssembly.dll RVA 0x933980
			Write(typeof(T) == typeof(object) ? value.GetType() : typeof(T), key, value);
		}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void Write(Type type, string key, object value)
	{
			// Recovered from GameAssembly.dll RVA 0x3D5F60
			StartWriteProperty(key);
			StartWriteObject(key);
			WriteType(type);
			ES3Type valueType = ES3Internal.ES3TypeMgr.GetOrCreateES3Type(type, true);
			WriteProperty("value", value, valueType, settings.referenceMode);
			EndWriteObject(key);
			EndWriteProperty(key);
			MarkKeyForDeletion(key);
		}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void Write(object value, ES3.ReferenceMode memberReferenceMode = ES3.ReferenceMode.ByRef)
	{
			// Recovered from GameAssembly.dll RVA 0x3D60C0
			if (value is null)
			{
			    WriteNull();
			    return;
			}
			ES3Type type = ES3Internal.ES3TypeMgr.GetOrCreateES3Type(value.GetType(), true);
			Write(value, type, memberReferenceMode);
		}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void Write(object value, ES3Type type, ES3.ReferenceMode memberReferenceMode = ES3.ReferenceMode.ByRef)
	{
			// Recovered from GameAssembly.dll RVA 0x3D61A0
			if (value is null)
			{
			    WriteNull();
			    return;
			}
			if (typeof(UnityEngine.Object).IsAssignableFrom(value.GetType()))
			{
			    // Original native inlined Object check reads m_CachedPtr at offset0x10;
			    // it does not call IsNativeObjectAlive or op_Equality. Read that field
			    // through reflection because Unity exposes it as private managed data.
			    UnityEngine.Object unityValue = value as UnityEngine.Object;
			    if (unityValue is null || (IntPtr)typeof(UnityEngine.Object).GetField("m_CachedPtr", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(unityValue) == IntPtr.Zero)
			    {
			        WriteNull();
			        return;
			    }
			}
			if (type is null || type.type == typeof(object))
			{
			    Type actualType = value.GetType();
			    type = ES3Internal.ES3TypeMgr.GetOrCreateES3Type(actualType, true);
			    if (type is null)
			        throw new NotSupportedException("Types of " + actualType + " are not supported. Please see the Supported Types guide for more information: https://docs.moodkie.com/easy-save-3/es3-supported-types/");
			    if (!type.isCollection && !type.isDictionary)
			    {
			        StartWriteObject(null);
			        WriteType(actualType);
			        type.Write(value, this);
			        EndWriteObject(null);
			        return;
			    }
			}
			if (type is null) throw new ArgumentNullException("ES3Type argument cannot be null.");
			if (type.isUnsupported)
			{
			    if (!type.isCollection && !type.isDictionary)
			        throw new NotSupportedException("Types of " + type.type + " are not supported. Please see the Supported Types guide for more information: https://docs.moodkie.com/easy-save-3/es3-supported-types/");
			    throw new NotSupportedException(type.type + " is not supported because it's element type is not supported. Please see the Supported Types guide for more information: https://docs.moodkie.com/easy-save-3/es3-supported-types/");
			}
			if (type.isPrimitive || type.isEnum)
			{
			    type.Write(value, this);
			    return;
			}
			if (type.isCollection)
			{
			    StartWriteCollection();
			    ((ES3CollectionType)type).Write(value, this, memberReferenceMode);
			    EndWriteCollection();
			}
			else if (type.isDictionary)
			{
			    StartWriteDictionary();
			    ((ES3DictionaryType)type).Write(value, this, memberReferenceMode);
			    EndWriteDictionary();
			}
			else
			{
			    if (type.type == typeof(UnityEngine.GameObject))
			    {
			        // Native checks writer.settings before its serializer cast.
			        if (settings is null) throw new NullReferenceException();
			        ((ES3Type_GameObject)type).saveChildren = settings.saveChildren;
			    }
			    StartWriteObject(null);
			    if (type.isES3TypeUnityObject) ((ES3UnityObjectType)type).WriteObject(value, this, memberReferenceMode);
			    else type.Write(value, this);
			    EndWriteObject(null);
			}
		}

	internal virtual void WriteRef(UnityEngine.Object obj)
	{
	}

	public virtual void WriteProperty(string name, object value)
	{
			// Recovered from GameAssembly.dll RVA 0x3D6BD0
			WriteProperty(name, value, settings.memberReferenceMode);
		}

	public virtual void WriteProperty(string name, object value, ES3.ReferenceMode memberReferenceMode)
	{
			// Recovered from GameAssembly.dll RVA 0x3D6C10
			if (SerializationDepthLimitExceeded()) return;
			StartWriteProperty(name);
			Write(value, memberReferenceMode);
			EndWriteProperty(name);
		}

	public virtual void WriteProperty<T>(string name, object value)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void WriteProperty(string name, object value, ES3Type type)
	{
			// Recovered from GameAssembly.dll RVA 0x3D6CA0
			WriteProperty(name, value, type, settings.memberReferenceMode);
		}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void WriteProperty(string name, object value, ES3Type type, ES3.ReferenceMode memberReferenceMode)
	{
			// Recovered from GameAssembly.dll RVA 0x3D6CE0
			if (SerializationDepthLimitExceeded()) return;
			StartWriteProperty(name);
			Write(value, type, memberReferenceMode);
			EndWriteProperty(name);
		}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void WritePropertyByRef(string name, UnityEngine.Object value)
	{
	}

	public void WritePrivateProperty(string name, object objectContainingProperty)
	{
	}

	public void WritePrivateField(string name, object objectContainingField)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void WritePrivateProperty(string name, object objectContainingProperty, ES3Type type)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void WritePrivateField(string name, object objectContainingField, ES3Type type)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void WritePrivatePropertyByRef(string name, object objectContainingProperty)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void WritePrivateFieldByRef(string name, object objectContainingField)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void WriteType(Type type)
	{
			// Recovered from GameAssembly.dll RVA 0x3D7E70
			WriteProperty("__type", ES3Internal.ES3Reflection.GetTypeString(type));
		}

	public static ES3Writer Create(string filePath, ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3D7F00
			var effective = new ES3Settings(filePath, settings);
			Stream stream = ES3Internal.ES3Stream.CreateStream(effective, ES3Internal.ES3FileMode.Write);
			if (stream is null) return null;
			return Create(stream, effective, true, true);
		}

	public static ES3Writer Create(ES3Settings settings)
	{
			// Recovered from GameAssembly.dll RVA 0x3D8010
			Stream stream = ES3Internal.ES3Stream.CreateStream(settings, ES3Internal.ES3FileMode.Write);
			if (stream is null) return null;
			return Create(stream, settings, true, true);
		}

	internal static ES3Writer Create(ES3Settings settings, bool writeHeaderAndFooter, bool overwriteKeys, bool append)
	{
			// Recovered from GameAssembly.dll RVA 0x3D8060
			Stream stream = ES3Internal.ES3Stream.CreateStream(settings, append ? ES3Internal.ES3FileMode.Append : ES3Internal.ES3FileMode.Write);
			if (stream is null) return null;
			return Create(stream, settings, writeHeaderAndFooter, overwriteKeys);
		}

	internal static ES3Writer Create(Stream stream, ES3Settings settings, bool writeHeaderAndFooter, bool overwriteKeys)
	{
			// Recovered from GameAssembly.dll RVA 0x3D80D0
			if (stream is null) throw new NullReferenceException();
			if (stream.GetType() == typeof(MemoryStream))
			{
			    settings = (ES3Settings)settings.Clone();
			    settings.location = ES3.Location.InternalMS;
			}
			if (settings is null) throw new NullReferenceException();
			if (settings.format == ES3.Format.JSON) return new ES3Internal.ES3JSONWriter(stream, settings, writeHeaderAndFooter, overwriteKeys);
			return null;
		}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected bool SerializationDepthLimitExceeded()
	{
			// Recovered from GameAssembly.dll RVA 0x3D82A0
			if (serializationDepth <= settings.serializationDepthLimit) return false;
			ES3Internal.ES3Debug.LogWarning("Serialization depth limit of " + settings.serializationDepthLimit.ToString() + " has been exceeded, indicating that there may be a circular reference.\nIf this is not a circular reference, you can increase the depth by going to Window > Easy Save 3 > Settings > Advanced Settings > Serialization Depth Limit", null, 0);
			return true;
		}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void MarkKeyForDeletion(string key)
	{
			// Recovered from GameAssembly.dll RVA 0x3D8340
			keysToDelete.Add(key);
		}

	protected void Merge()
	{
			// Recovered from GameAssembly.dll RVA 0x3D83B0
			using (ES3Reader reader = ES3Reader.Create(settings))
			{
			    if (reader != null) Merge(reader);
			}
		}

	protected void Merge(ES3Reader reader)
	{
			// Recovered from GameAssembly.dll RVA 0x3D8460
			foreach (object entry in reader.RawEnumerator)
			{
			    // Original TypeInfo0x2D52E88 is boxed KeyValuePair<string,ES3Data>,
			    // whose key/type/bytes at object+0x10/+0x18/+0x20 resemble a Tuple.
			    var raw = (KeyValuePair<string, ES3Internal.ES3Data>)entry;
			    if (!keysToDelete.Contains(raw.Key) || raw.Value.type is null)
			    {
			        if (raw.Value.type is null) throw new NullReferenceException();
			        Write(raw.Key, raw.Value.type.type, raw.Value.bytes);
			    }
			}
		}

	public virtual void Save()
	{
			// Recovered from GameAssembly.dll RVA 0x3D8750
			Save(overwriteKeys);
		}

	public virtual void Save(bool overwriteKeys)
	{
			// Recovered from GameAssembly.dll RVA 0x3D8770
			if (overwriteKeys) Merge();
			EndWriteFile();
			Dispose();
			if (settings.location == ES3.Location.File || settings.location == ES3.Location.PlayerPrefs)
			    ES3Internal.ES3IO.CommitBackup(settings);
		}
}
