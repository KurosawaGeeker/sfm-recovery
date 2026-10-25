using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3Type_ES3Prefab : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x549810.
		public ES3Type_ES3Prefab()
			: base(typeof(ES3Internal.ES3Prefab))
		{
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
		}

		public override object Read<T>(ES3Reader reader)
		{
			return null;
		}
	}
}
