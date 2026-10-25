using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;

[IncludeInSettings(true)]
public class ES3Spreadsheet
{
	protected struct Index
	{
		public int col;

		public int row;

		public Index(int col, int row)
		{
			this.col = 0;
			this.row = 0;
		}
	}

	private int cols;

	private int rows;

	private Dictionary<Index, string> cells;

	private const string QUOTE = "\"";

	private const char QUOTE_CHAR = '"';

	private const char COMMA_CHAR = ',';

	private const char NEWLINE_CHAR = '\n';

	private const string ESCAPED_QUOTE = "\"\"";

	private static char[] CHARS_TO_ESCAPE;

	public int ColumnCount => 0;

	public int RowCount => 0;

	public int GetColumnLength(int col)
	{
		return 0;
	}

	public int GetRowLength(int row)
	{
		return 0;
	}

	public void SetCell(int col, int row, object value)
	{
	}

	private void SetCellString(int col, int row, string value)
	{
	}

	public T GetCell<T>(int col, int row)
	{
		return default;
	}

	public object GetCell(Type type, int col, int row)
	{
		return null;
	}

	public void Load(string filePath)
	{
	}

	public void Load(string filePath, ES3Settings settings)
	{
	}

	public void Load(ES3Settings settings)
	{
	}

	public void LoadRaw(string str)
	{
	}

	public void LoadRaw(string str, ES3Settings settings)
	{
	}

	private void Load(Stream stream, ES3Settings settings)
	{
	}

	public void Save(string filePath)
	{
	}

	public void Save(string filePath, ES3Settings settings)
	{
	}

	public void Save(ES3Settings settings)
	{
	}

	public void Save(string filePath, bool append)
	{
	}

	public void Save(string filePath, ES3Settings settings, bool append)
	{
	}

	public void Save(ES3Settings settings, bool append)
	{
	}

	private static string Escape(string str, bool isAlreadyWrappedInQuotes = false)
	{
		return null;
	}

	private static string Unescape(string str)
	{
		return null;
	}

	private string[,] ToArray()
	{
		return null;
	}
}
