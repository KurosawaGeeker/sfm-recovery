using ExposureUnnoticed2.Object3D.Player.Scripts.Other;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerObjectReferencer : MonoBehaviour
	{
		[SerializeField]
		public AudioSource BreathVoiceSource;

		[SerializeField]
		public DownhillChecker DownhillChecker;

		[SerializeField]
		public DownhillChecker OnGroundedChecker;

		[SerializeField]
		public TerrainCheckCollider TerrainCheckCollider;

		[SerializeField]
		public AudioSource[] AudioSources;
	}
}
