using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[] { "boneIndex0", "boneIndex1", "boneIndex2", "boneIndex3", "weight0", "weight1", "weight2", "weight3" })]
	public class ES3Type_BoneWeight : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x564150.
		public ES3Type_BoneWeight()
			: base(typeof(UnityEngine.BoneWeight))
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
