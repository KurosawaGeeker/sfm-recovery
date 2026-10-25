using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "dynamicFriction", "staticFriction", "bounciness", "frictionCombine", "bounceCombine" })]
	public class ES3Type_PhysicMaterial : ES3ObjectType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x583290.
		public ES3Type_PhysicMaterial()
			: base(typeof(UnityEngine.PhysicMaterial))
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
