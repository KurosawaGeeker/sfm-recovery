using ExposureUnnoticed2.Scripts.Sound;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.FootStepSoundObject
{
	public class FootStepSoundObject : MonoBehaviour
	{
		[SerializeField]
		private InGameSoundManager.SeType footStepType;

		private void Awake()
		{
		    // Original RVA 0x5FC760; generated callback RVA 0x5FC7E0.
		    // This delay lets PlayerSoundManager initialize before surfaces register.
		    global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayFlame(() =>
		        global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerSoundManager.Instance.RegisterFootStepObject(gameObject, footStepType), 1);
		}
	}
}
