using System;
using System.IO;

namespace ES3Internal
{
	public class ES3JSONReader : ES3Reader
	{
		private const char endOfStreamChar = '\uffff';

		public StreamReader baseReader;

		internal ES3JSONReader(Stream stream, ES3Settings settings, bool readHeaderAndFooter = true)
            : base(settings, readHeaderAndFooter)
        {
            // Original ignores settings.encoding/bufferSize: UTF8 + BOM detection + 1024 buffer.
            baseReader = new StreamReader(stream, System.Text.Encoding.UTF8);
            if (readHeaderAndFooter)
            {
                char first = ReadCharIgnoreWhitespace(true);
                if (first != '{') throw new FormatException("File is not valid JSON. Expected '{' at beginning of file, but found '" + first + "'.");
            }
        }

		public override string ReadPropertyName()
		{
			// Recovered from GameAssembly.dll RVA 0x5A02F0
			char first = PeekCharIgnoreWhitespace();
			if (IsTerminator(first)) return null;
			if (first == ',') ReadCharIgnoreWhitespace(true);
			else if (!IsQuotationMark(first)) throw new FormatException("Expected ',' separating properties or '\"' before property name, found '" + first + "'.");
			string name = Read_string();
			if (name == null) throw new FormatException("Stream isn't positioned before a property.");
			ES3Debug.Log("<b>" + name + "</b> (reading property)", null, serializationDepth);
			ReadCharIgnoreWhitespace(':');
			return name;
		}

		protected override Type ReadKeyPrefix(bool ignoreType = false)
		{
			// Recovered from GameAssembly.dll RVA 0x5A04D0
			StartReadObject();
			Type type = null;
			string name = ReadPropertyName();
			if (name == "__type")
			{
			    string typeName = Read_string();
			    if (!ignoreType) type = ES3Reflection.GetType(typeName);
			    name = ReadPropertyName();
			}
			if (name != "value") throw new FormatException("This data is not Easy Save Key Value data. Expected property name \"value\", found \"" + name + "\".");
			return type;
		}

		protected override void ReadKeySuffix()
		{
			// Recovered from GameAssembly.dll RVA 0x5A06B0
			EndReadObject();
		}

		internal override bool StartReadObject()
		{
			// Recovered from GameAssembly.dll RVA 0x5A06D0
			++serializationDepth;
			return ReadNullOrCharIgnoreWhitespace('{');
		}

		internal override void EndReadObject()
		{
			// Recovered from GameAssembly.dll RVA 0x5A06E0
			ReadCharIgnoreWhitespace('}');
			--serializationDepth;
		}

		internal override bool StartReadDictionary()
		{
			// Recovered from GameAssembly.dll RVA 0x5A0700
			return StartReadObject();
		}

		internal override void EndReadDictionary()
		{
		}

		internal override bool StartReadDictionaryKey()
		{
			// Recovered from GameAssembly.dll RVA 0x5A0720
			if (PeekCharIgnoreWhitespace() != '}') return true;
			ReadCharIgnoreWhitespace(true);
			return false;
		}

		internal override void EndReadDictionaryKey()
		{
			// Recovered from GameAssembly.dll RVA 0x5A0760
			ReadCharIgnoreWhitespace(':');
		}

		internal override void StartReadDictionaryValue()
		{
		}

		internal override bool EndReadDictionaryValue()
		{
			// Recovered from GameAssembly.dll RVA 0x5A0770
			char next = ReadCharIgnoreWhitespace(true);
			if (next == '}') return true;
			if (next == ',') return false;
			throw new FormatException("Expected ',' seperating Dictionary items or '}' terminating Dictionary, found '" + next + "'.");
		}

		internal override bool StartReadCollection()
		{
			// Recovered from GameAssembly.dll RVA 0x5A0840
			return ReadNullOrCharIgnoreWhitespace('[');
		}

		internal override void EndReadCollection()
		{
		}

		internal override bool StartReadCollectionItem()
		{
			// Recovered from GameAssembly.dll RVA 0x5A0850
			if (PeekCharIgnoreWhitespace() != ']') return true;
			ReadCharIgnoreWhitespace(true);
			return false;
		}

		internal override bool EndReadCollectionItem()
		{
			// Recovered from GameAssembly.dll RVA 0x5A0890
			char next = ReadCharIgnoreWhitespace(true);
			if (next == ']') return true;
			if (next == ',') return false;
			throw new FormatException("Expected ',' seperating collection items or ']' terminating collection, found '" + next + "'.");
		}

		private void ReadString(StreamWriter writer, bool skip = false)
		{
			// Recovered from GameAssembly.dll RVA 0x5A0960
			while (true)
			{
			    char value = ReadOrSkipChar(writer, skip);
			    if (value == '\\') ReadOrSkipChar(writer, skip);
			    else
			    {
			        if (value == endOfStreamChar) throw new FormatException("String without closing quotation mark detected.");
			        if (IsQuotationMark(value)) return;
			    }
			}
		}

		internal override byte[] ReadElement(bool skip = false)
		{
			// Recovered from GameAssembly.dll RVA 0x5A0A90
			using (StreamWriter writer = skip ? null : new StreamWriter(new MemoryStream(settings.bufferSize)))
			{
			    // The original does not skip leading whitespace before selecting this branch.
			    char first = (char)baseReader.Peek();
			    if (IsOpeningBrace(first))
			    {
			        int depth = 0;
			        while (true)
			        {
			            char value = ReadOrSkipChar(writer, skip);
			            if (value == endOfStreamChar) throw new FormatException("Missing closing brace detected, as end of stream was reached before finding it.");
			            if (IsQuotationMark(value)) { ReadString(writer, skip); continue; }
			            if (IsOpeningBrace(value)) ++depth;
			            else if (IsTerminator(value) && --depth <= 0) break;
			        }
			    }
			    else if (first == '"')
			    {
			        ReadOrSkipChar(writer, skip);
			        ReadString(writer, skip);
			    }
			    else
			    {
			        while (!IsEndOfValue((char)baseReader.Peek())) ReadOrSkipChar(writer, skip);
			    }
			    if (skip) return null;
			    writer.Flush();
			    return ((MemoryStream)writer.BaseStream).ToArray();
			}
		}

		private char ReadOrSkipChar(StreamWriter writer, bool skip)
		{
			// Recovered from GameAssembly.dll RVA 0x5A1120
			char value = (char)baseReader.Read();
			if (!skip) writer.Write(value);
			return value;
		}

		private char ReadCharIgnoreWhitespace(bool ignoreTrailingWhitespace = true)
		{
			// Recovered from GameAssembly.dll RVA 0x5A11A0
			char value;
			do { value = (char)baseReader.Read(); } while (IsWhiteSpace(value));
			if (ignoreTrailingWhitespace)
			    while (IsWhiteSpace((char)baseReader.Peek())) baseReader.Read();
			return value;
		}

		private bool ReadNullOrCharIgnoreWhitespace(char expectedChar)
		{
			// Recovered from GameAssembly.dll RVA 0x5A1280
			char actual = ReadCharIgnoreWhitespace(true);
			if (actual == 'n')
			{
			    char[] tail = new char[3];
			    // Exact original: single block read; trailing whitespace was already skipped.
			    baseReader.Read(tail, 0, 3);
			    if (tail[0] == 'u' && tail[1] == 'l' && tail[2] == 'l') return true;
			}
			if (actual == expectedChar) return false;
			if (actual == endOfStreamChar) throw new FormatException("End of stream reached when expecting '" + expectedChar + "'.");
			throw new FormatException("Expected '" + expectedChar + "' or \"null\", found '" + actual + "'.");
		}

		private char ReadCharIgnoreWhitespace(char expectedChar)
		{
			// Recovered from GameAssembly.dll RVA 0x5A1560
			char actual = ReadCharIgnoreWhitespace(true);
			if (actual == expectedChar) return actual;
			if (actual == endOfStreamChar) throw new FormatException("End of stream reached when expecting '" + expectedChar + "'.");
			throw new FormatException("Expected '" + expectedChar + "', found '" + actual + "'.");
		}

		private bool ReadQuotationMarkOrNullIgnoreWhitespace()
		{
			// Recovered from GameAssembly.dll RVA 0x5A1760
			char first = ReadCharIgnoreWhitespace(false);
			if (first == 'n')
			{
			    char[] tail = new char[3];
			    baseReader.Read(tail, 0, 3);
			    return tail[0] == 'u' && tail[1] == 'l' && tail[2] == 'l';
			}
			if (IsQuotationMark(first)) return false;
			if (first == endOfStreamChar) throw new FormatException("End of stream reached when expecting quotation mark.");
			throw new FormatException("Expected quotation mark, found '" + first + "'.");
		}

		private char PeekCharIgnoreWhitespace(char expectedChar)
		{
			// Recovered from GameAssembly.dll RVA 0x5A1990
			char actual = PeekCharIgnoreWhitespace();
			if (actual == expectedChar) return actual;
			if (actual == endOfStreamChar) throw new FormatException("End of stream reached while peeking, when expecting '" + expectedChar + "'.");
			throw new FormatException("Expected '" + expectedChar + "', found '" + actual + "'.");
		}

		private char PeekCharIgnoreWhitespace()
		{
			// Recovered from GameAssembly.dll RVA 0x5A1B90
			SkipWhiteSpace();
			return (char)baseReader.Peek();
		}

		private void SkipWhiteSpace()
		{
			// Recovered from GameAssembly.dll RVA 0x5A1C00
			while (IsWhiteSpace((char)baseReader.Peek())) baseReader.Read();
		}

		private void SkipOpeningBraceOfFile()
		{
			// Recovered from GameAssembly.dll RVA 0x5A1C70
			char first = ReadCharIgnoreWhitespace(true);
			if (first != '{') throw new FormatException("File is not valid JSON. Expected '{' at beginning of file, but found '" + first + "'.");
		}

		private static bool IsWhiteSpace(char c)
		{
			// Recovered from GameAssembly.dll RVA 0x5A1D30
			return c == ' ' || c == '\t' || c == '\n' || c == '\r';
		}

		private static bool IsOpeningBrace(char c)
		{
			// Recovered from GameAssembly.dll RVA 0x5A1D50
			return c == '{' || c == '[';
		}

		private static bool IsEndOfValue(char c)
		{
			// Recovered from GameAssembly.dll RVA 0x5A1D70
			return IsWhiteSpace(c) || c == '}' || c == ']' || c == ',' || c == ':' || c == endOfStreamChar;
		}

		private static bool IsTerminator(char c)
		{
			// Recovered from GameAssembly.dll RVA 0x5A1DB0
			return c == '}' || c == ']';
		}

		private static bool IsQuotationMark(char c)
		{
			// Recovered from GameAssembly.dll RVA 0x5A1DD0
			return c == (char)0x22 || c == (char)0x201c || c == (char)0x201d;
		}

		private static bool IsEndOfStream(char c)
		{
			// Recovered from GameAssembly.dll RVA 0x5A1DF0
			return c == endOfStreamChar;
		}

		private string GetValueString()
		{
			// Recovered from GameAssembly.dll RVA 0x5A1E00
			var result = new System.Text.StringBuilder();
			while (true)
			{
			    char next = (char)baseReader.Peek();
			    // Native skips ASCII whitespace throughout tokens, not just their prefix.
			    if (IsWhiteSpace(next)) { baseReader.Read(); continue; }
			    if (IsEndOfValue(next)) return result.Length == 0 ? null : result.ToString();
			    result.Append((char)baseReader.Read());
			}
		}

		internal override string Read_string()
		{
			// Recovered from GameAssembly.dll RVA 0x5A1FB0
			if (ReadQuotationMarkOrNullIgnoreWhitespace()) return null;
			var result = new System.Text.StringBuilder();
			while (true)
			{
			    char next = (char)baseReader.Read();
			    if (IsQuotationMark(next)) return result.ToString();
			    if (next == '\\')
			    {
			        next = (char)baseReader.Read();
			        if (next == endOfStreamChar) throw new FormatException("Reached end of stream while trying to read string literal.");
			        switch (next)
			        {
			            case 'b': next = '\b'; break;
			            case 'f': next = '\f'; break;
			            case 'n': next = '\n'; break;
			            case 'r': next = '\r'; break;
			            case 't': next = '\t'; break;
			        }
			    }
			    // Preserve original unknown escape/smart quote behavior and unescaped EOF loop.
			    result.Append(next);
			}
		}

		internal override long Read_ref()
		{
			// Recovered from GameAssembly.dll RVA 0x5A21D0
			if (ES3ReferenceMgrBase.Current == null) throw new InvalidOperationException("An Easy Save 3 Manager is required to load references. To add one to your scene, exit playmode and go to Tools > Easy Save 3 > Add Manager to Scene");
			if (IsQuotationMark(PeekCharIgnoreWhitespace())) return long.Parse(Read_string());
			return Read_long();
		}

		internal override char Read_char()
		{
			// Recovered from GameAssembly.dll RVA 0x5A2380
			return char.Parse(Read_string());
		}

		internal override float Read_float()
		{
			// Recovered from GameAssembly.dll RVA 0x5A2480
			return float.Parse(GetValueString(), System.Globalization.CultureInfo.InvariantCulture);
		}

		internal override int Read_int()
		{
			// Recovered from GameAssembly.dll RVA 0x5A2530
			return int.Parse(GetValueString());
		}

		internal override bool Read_bool()
		{
			// Recovered from GameAssembly.dll RVA 0x5A2550
			return bool.Parse(GetValueString());
		}

		internal override decimal Read_decimal()
		{
			// Recovered from GameAssembly.dll RVA 0x5A25B0
			return decimal.Parse(GetValueString(), System.Globalization.CultureInfo.InvariantCulture);
		}

		internal override double Read_double()
		{
			// Recovered from GameAssembly.dll RVA 0x5A2760
			return double.Parse(GetValueString(), System.Globalization.CultureInfo.InvariantCulture);
		}

		internal override long Read_long()
		{
			// Recovered from GameAssembly.dll RVA 0x5A2810
			return long.Parse(GetValueString());
		}

		internal override ulong Read_ulong()
		{
			// Recovered from GameAssembly.dll RVA 0x5A2830
			return ulong.Parse(GetValueString());
		}

		internal override uint Read_uint()
		{
			// Recovered from GameAssembly.dll RVA 0x5A2900
			return uint.Parse(GetValueString());
		}

		internal override byte Read_byte()
		{
			// Recovered from GameAssembly.dll RVA 0x5A2530
			return unchecked((byte)int.Parse(GetValueString()));
		}

		internal override sbyte Read_sbyte()
		{
			// Recovered from GameAssembly.dll RVA 0x5A2530
			return unchecked((sbyte)int.Parse(GetValueString()));
		}

		internal override short Read_short()
		{
			// Recovered from GameAssembly.dll RVA 0x5A2530
			return unchecked((short)int.Parse(GetValueString()));
		}

		internal override ushort Read_ushort()
		{
			// Recovered from GameAssembly.dll RVA 0x5A2530
			return unchecked((ushort)int.Parse(GetValueString()));
		}

		internal override byte[] Read_byteArray()
		{
			// Recovered from GameAssembly.dll RVA 0x5A29D0
			return Convert.FromBase64String(Read_string());
		}

		public override void Dispose()
		{
			// Recovered from GameAssembly.dll RVA 0x5A2AD0
			baseReader.Dispose();
		}
	}
}
