using ExposureUnnoticed2.Master.AdultGoods;
using ExposureUnnoticed2.Scripts.Sound;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Costume
{
	public class PistonMachineAnimationEventReceiver : MonoBehaviour
	{
		[SerializeField]
		private MAdultGoodsType adultGoodsType;

		private AudioSource audioSource;

		private float addEcstasy;

		private void Awake()
		{
		}

		private void OnPlaySe(InGameSoundManager.SeType type)
		{
		}
	}
}
