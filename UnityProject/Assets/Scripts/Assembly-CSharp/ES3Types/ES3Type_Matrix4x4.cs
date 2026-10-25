using UnityEngine.Scripting;

namespace ES3Types
{
	[ES3Properties(new string[] { "col0", "col1", "col2", "col3" })]
	[Preserve]
	public class ES3Type_Matrix4x4 : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x570fc0.
		public ES3Type_Matrix4x4()
			: base(typeof(UnityEngine.Matrix4x4))
		{
			Instance = this;
		}

		public override void Write(object obj, ES3Writer writer)
		{
			// Recovered from GameAssembly.dll RVA 0x571100
			UnityEngine.Matrix4x4 value = (UnityEngine.Matrix4x4)obj;
			writer.WriteProperty("col0", new UnityEngine.Vector4(value.m00, value.m10, value.m20, value.m30), ES3Type_Vector4.Instance);
			writer.WriteProperty("col1", new UnityEngine.Vector4(value.m01, value.m11, value.m21, value.m31), ES3Type_Vector4.Instance);
			writer.WriteProperty("col2", new UnityEngine.Vector4(value.m02, value.m12, value.m22, value.m32), ES3Type_Vector4.Instance);
			writer.WriteProperty("col3", new UnityEngine.Vector4(value.m03, value.m13, value.m23, value.m33), ES3Type_Vector4.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			// Recovered from GameAssembly.dll RVA 0x81D9B0
			return new UnityEngine.Matrix4x4(reader.ReadProperty<UnityEngine.Vector4>(ES3Type_Vector4.Instance), reader.ReadProperty<UnityEngine.Vector4>(ES3Type_Vector4.Instance), reader.ReadProperty<UnityEngine.Vector4>(ES3Type_Vector4.Instance), reader.ReadProperty<UnityEngine.Vector4>(ES3Type_Vector4.Instance));
		}
	}
}
