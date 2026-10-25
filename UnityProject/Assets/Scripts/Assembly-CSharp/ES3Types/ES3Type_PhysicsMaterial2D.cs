using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "bounciness", "friction" })]
	public class ES3Type_PhysicsMaterial2D : ES3ObjectType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x583900.
		public ES3Type_PhysicsMaterial2D()
			: base(typeof(UnityEngine.PhysicsMaterial2D))
		{
			Instance = this;
		}

		protected override void WriteObject(object obj, ES3Writer writer)
		{
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			return null;
		}
	}
}
