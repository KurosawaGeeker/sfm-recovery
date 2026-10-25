using System.IO;

namespace ES3Internal
{
	internal class ES3JSONWriter : ES3Writer
	{
		internal StreamWriter baseWriter;

		private bool isFirstProperty = true;

		// Original public constructor RVA0x5A83A0 forwards both flags=true.
		public ES3JSONWriter(Stream stream, ES3Settings settings)
			: this(stream, settings, true, true)
		{
		}

		internal ES3JSONWriter(Stream stream, ES3Settings settings, bool writeHeaderAndFooter, bool mergeKeys)
			: base(settings, writeHeaderAndFooter, mergeKeys)
		{
			// Original StreamWriter(Stream): default UTF8, not settings.encoding.
			baseWriter = new StreamWriter(stream);
			StartWriteFile();
		}

		internal override void WritePrimitive(int value)
		{
			// Recovered from GameAssembly.dll RVA 0x5A84C0
			baseWriter.Write(value);
		}

		internal override void WritePrimitive(float value)
		{
			// Recovered from GameAssembly.dll RVA 0x5A84F0
			baseWriter.Write(value.ToString("R", System.Globalization.CultureInfo.InvariantCulture));
		}

		internal override void WritePrimitive(bool value)
		{
			// Recovered from GameAssembly.dll RVA 0x5A8640
			baseWriter.Write(value ? "true" : "false");
		}

		internal override void WritePrimitive(decimal value)
		{
			// Recovered from GameAssembly.dll RVA 0x5A86C0
			baseWriter.Write(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
		}

		internal override void WritePrimitive(double value)
		{
			// Recovered from GameAssembly.dll RVA 0x5A87D0
			baseWriter.Write(value.ToString("R", System.Globalization.CultureInfo.InvariantCulture));
		}

		internal override void WritePrimitive(long value)
		{
			// Recovered from GameAssembly.dll RVA 0x5A8920
			baseWriter.Write(value);
		}

		internal override void WritePrimitive(ulong value)
		{
			// Recovered from GameAssembly.dll RVA 0x5A8950
			baseWriter.Write(value);
		}

		internal override void WritePrimitive(uint value)
		{
			// Recovered from GameAssembly.dll RVA 0x5A8980
			baseWriter.Write(value);
		}

		internal override void WritePrimitive(byte value)
		{
			// Recovered from GameAssembly.dll RVA 0x5A89B0
			baseWriter.Write((int)value);
		}

		internal override void WritePrimitive(sbyte value)
		{
			// Recovered from GameAssembly.dll RVA 0x5A8A30
			baseWriter.Write((int)value);
		}

		internal override void WritePrimitive(short value)
		{
			// Recovered from GameAssembly.dll RVA 0x5A8AB0
			baseWriter.Write((int)value);
		}

		internal override void WritePrimitive(ushort value)
		{
			// Recovered from GameAssembly.dll RVA 0x5A8B30
			baseWriter.Write((int)value);
		}

		internal override void WritePrimitive(char value)
		{
			// Recovered from GameAssembly.dll RVA 0x5A8BB0
			WritePrimitive(value.ToString());
		}

		internal override void WritePrimitive(byte[] value)
		{
			// Recovered from GameAssembly.dll RVA 0x5A8C60
			WritePrimitive(System.Convert.ToBase64String(value));
		}

		internal override void WritePrimitive(string value)
		{
			// Recovered from GameAssembly.dll RVA 0x5A8DA0
			baseWriter.Write("\"");
			if (value is null) throw new System.NullReferenceException();
			foreach (char c in value)
			{
			    switch (c)
			    {
			        case '\b': baseWriter.Write("\\b"); break;
			        case '\t': baseWriter.Write("\\t"); break;
			        case '\n': baseWriter.Write("\\n"); break;
			        case '\f': baseWriter.Write("\\f"); break;
			        case '\r': baseWriter.Write("\\r"); break;
			        default:
			            if (c == '"' || c == '/' || c == '\\' || c == '\u201C' || c == '\u201D') baseWriter.Write('\\');
			            baseWriter.Write(c);
			            break;
			    }
			}
			baseWriter.Write("\"");
		}

		internal override void WriteNull()
		{
			// Recovered from GameAssembly.dll RVA 0x5A90B0
			baseWriter.Write("null");
		}

		private static bool CharacterRequiresEscaping(char c)
		{
			// Recovered from GameAssembly.dll RVA 0x5A9110
			return c == '"' || c == '\\' || c == '\u201C' || c == '\u201D';
		}

		private void WriteCommaIfRequired()
		{
			// Recovered from GameAssembly.dll RVA 0x5A9140
			if (isFirstProperty) isFirstProperty = false;
			else baseWriter.Write(',');
			WriteNewlineAndTabs();
		}

		internal override void WriteRawProperty(string name, byte[] value)
		{
			// Recovered from GameAssembly.dll RVA 0x5A91A0
			StartWriteProperty(name);
			if (settings is null || value is null || settings.encoding is null) throw new System.NullReferenceException();
			string text = settings.encoding.GetString(value, 0, value.Length);
			baseWriter.Write(text);
			EndWriteProperty(name);
		}

		internal override void StartWriteFile()
		{
			// Recovered from GameAssembly.dll RVA 0x5A9270
			if (writeHeaderAndFooter) baseWriter.Write('{');
			++serializationDepth;
		}

		internal override void EndWriteFile()
		{
			// Recovered from GameAssembly.dll RVA 0x5A92B0
			--serializationDepth;
			WriteNewlineAndTabs();
			if (writeHeaderAndFooter) baseWriter.Write('}');
		}

		internal override void StartWriteProperty(string name)
		{
			// Recovered from GameAssembly.dll RVA 0x5A9300
			if (name is null) throw new System.ArgumentNullException("Key or field name cannot be NULL when saving data.");
			ES3Debug.Log("<b>" + name + "</b> (writing property)", null, serializationDepth);
			if (isFirstProperty) isFirstProperty = false;
			else baseWriter.Write(',');
			WriteNewlineAndTabs();
			Write(name, ES3.ReferenceMode.ByRef);
			if (settings.prettyPrint) baseWriter.Write(' ');
			baseWriter.Write(':');
			if (settings.prettyPrint) baseWriter.Write(' ');
		}

		internal override void EndWriteProperty(string name)
		{
		}

		internal override void StartWriteObject(string name)
		{
			// Recovered from GameAssembly.dll RVA 0x5A94C0
			++serializationDepth;
			isFirstProperty = true;
			baseWriter.Write('{');
		}

		internal override void EndWriteObject(string name)
		{
			// Recovered from GameAssembly.dll RVA 0x5A9500
			--serializationDepth;
			isFirstProperty = false;
			WriteNewlineAndTabs();
			baseWriter.Write('}');
		}

		internal override void StartWriteCollection()
		{
			// Recovered from GameAssembly.dll RVA 0x5A9550
			++serializationDepth;
			baseWriter.Write('[');
			WriteNewlineAndTabs();
		}

		internal override void EndWriteCollection()
		{
			// Recovered from GameAssembly.dll RVA 0x5A95A0
			--serializationDepth;
			WriteNewlineAndTabs();
			baseWriter.Write(']');
		}

		internal override void StartWriteCollectionItem(int index)
		{
			// Recovered from GameAssembly.dll RVA 0x5A95E0
			if (index != 0) baseWriter.Write(',');
		}

		internal override void EndWriteCollectionItem(int index)
		{
		}

		internal override void StartWriteDictionary()
		{
			// Recovered from GameAssembly.dll RVA 0x5A9620
			StartWriteObject(null);
		}

		internal override void EndWriteDictionary()
		{
			// Recovered from GameAssembly.dll RVA 0x5A9640
			EndWriteObject(null);
		}

		internal override void StartWriteDictionaryKey(int index)
		{
			// Recovered from GameAssembly.dll RVA 0x5A95E0
			if (index != 0) baseWriter.Write(',');
		}

		internal override void EndWriteDictionaryKey(int index)
		{
			// Recovered from GameAssembly.dll RVA 0x5A9660
			baseWriter.Write(':');
		}

		internal override void StartWriteDictionaryValue(int index)
		{
		}

		internal override void EndWriteDictionaryValue(int index)
		{
		}

		public override void Dispose()
		{
			// Recovered from GameAssembly.dll RVA 0x5A9690
			baseWriter.Dispose();
		}

		public void WriteNewlineAndTabs()
		{
			// Recovered from GameAssembly.dll RVA 0x5A9710
			if (settings.prettyPrint)
			{
			    baseWriter.Write(System.Environment.NewLine);
			    for (int i = 0; i < serializationDepth; ++i) baseWriter.Write('\t');
			}
		}
	}
}
