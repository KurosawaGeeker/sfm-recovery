using ExposureUnnoticed2.Object3D.NPC.Script;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.InGame.NpcDirectIconPanel
{
	public class NpcDirectionArrowView : MonoBehaviour
	{
		public static bool IsDoneCommonProcessThisFrame;

		private static bool hideSkill;

		private static bool hideFog;

		private static bool onTimeStop;

		[SerializeField]
		private Image arrowImage;

		private NpcController target;

		private bool isActive;

		private float lastCheckStrangeTime;

		private bool isLastSkip;

		public void Initialize(NpcController npc)
		{
			target = npc;
			arrowImage.gameObject.SetActive(false);
		}

		private void Update()
		{
		}

		private void CommonProcessIfNeed()
		{
		}
	}
}
