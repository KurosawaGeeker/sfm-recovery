using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.Sound;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.ScriptableObject
{
	[CreateAssetMenu(fileName = "MFootAnimationEventRecord", menuName = "ScriptableObjects/MFootAnimationEventRecord")]
	public class MFootAnimationEventRecord : UnityEngine.ScriptableObject
	{
		[SerializeField]
		public InGameSoundManager.SeType SeType;

		[SerializeField]
		public Constant.Direct Direct;

		[SerializeField]
		public bool IsMoveAction;
	}
}
