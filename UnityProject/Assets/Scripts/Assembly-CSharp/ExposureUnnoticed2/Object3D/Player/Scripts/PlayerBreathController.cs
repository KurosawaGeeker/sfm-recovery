using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerBreathController
	{
		private const float LongestInterval = 2f;

		private const float ShortestInterval = 0.6f;

		private const float Breath2IntervalScale = 0.6f;

		private ParticleSystemRenderer particleSystemRenderer;

		private PlayerClassAccessor pca;

		private SmoothFloat breathVolumeScale;

		private int breathId;

		private float nextPlayTime;

		private float normalizedRate;

		private Vector3 initLocalPosition;

		private Quaternion initLocalRotation;

		private Vector3 currentWorldPosition;

		private Quaternion currentWorldRotation;

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x63A920
			this.pca = pca;
			particleSystemRenderer = pca.PlayerAvatarObjectReferencer.BreathParticle.GetComponent<ParticleSystemRenderer>();
			ParticleSystemRenderer renderer = particleSystemRenderer;
			renderer.material = UnityEngine.Object.Instantiate(renderer.material);
			pca.PlayerAvatarObjectReferencer.BreathParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			initLocalPosition = pca.PlayerAvatarObjectReferencer.BreathParticle.transform.localPosition;
			initLocalRotation = pca.PlayerAvatarObjectReferencer.BreathParticle.transform.localRotation;
			pca.PlayerAvatarObjectReferencer.BreathParticle.transform.parent = pca.Transform;
		}

		public void OnUpdate()
		{
		}

		public void OnLateUpdate()
		{
		}

		private void PlayBreath()
		{
		}
	// Recovered original member RVA 0x63B8E0: .ctor
		public PlayerBreathController()
        {
            breathVolumeScale = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(0.2f, 1f, 0f, false, false, -1f);
        }
	}
}
