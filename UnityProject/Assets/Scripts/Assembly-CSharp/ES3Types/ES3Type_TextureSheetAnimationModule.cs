using UnityEngine.Scripting;

namespace ES3Types
{
	[ES3Properties(new string[]
	{
		"enabled", "numTilesX", "numTilesY", "animation", "useRandomRow", "frameOverTime", "frameOverTimeMultiplier", "startFrame", "startFrameMultiplier", "cycleCount",
		"rowIndex", "uvChannelMask", "flipU", "flipV"
	})]
	[Preserve]
	public class ES3Type_TextureSheetAnimationModule : ES3Type
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x58cfa0.
		public ES3Type_TextureSheetAnimationModule()
			: base(typeof(UnityEngine.ParticleSystem.TextureSheetAnimationModule))
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

		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
		}
	}
}
