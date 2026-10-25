using System;
using System.ComponentModel;

[Serializable]
[EditorBrowsable(EditorBrowsableState.Never)]
public class ES3SerializableSettings : ES3Settings
{
	// Recovered original constructor RVA 0x3CD890
	public ES3SerializableSettings() : base(false)
	{

	}

	// Recovered original constructor RVA 0x3CD8F0
	public ES3SerializableSettings(bool applyDefaults) : base(applyDefaults)
	{

	}

	// Recovered original constructor RVA 0x3CD950
	public ES3SerializableSettings(string path) : base(false)
	{
this.path = path;
	}

	// Recovered original constructor RVA 0x3CDA00
	public ES3SerializableSettings(string path, ES3.Location location) : base(false)
	{
this.location = location;
	}
}
