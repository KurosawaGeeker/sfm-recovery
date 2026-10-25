using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	[ES3Properties(new string[]
	{
		"time", "hideFlags", "collision", "colorBySpeed", "colorOverLifetime", "emission", "externalForces", "forceOverLifetime", "inheritVelocity", "lights",
		"limitVelocityOverLifetime", "main", "noise", "rotatonBySpeed", "rotationOverLifetime", "shape", "sizeBySpeed", "sizeOverLifetime", "subEmitters", "textureSheetAnimation",
		"trails", "trigger", "useAutoRandomSeed", "velocityOverLifetime", "isPaused", "isPlaying", "isStopped"
	})]
	public class ES3Type_ParticleSystem : ES3ComponentType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x55e810.
		public ES3Type_ParticleSystem()
			: base(typeof(UnityEngine.ParticleSystem))
		{
			isValueType = false;
			isES3TypeUnityObject = true;
			Instance = this;
		}

		protected override void WriteComponent(object obj, ES3Writer writer)
		{
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
		}
	}
}
