using ExposureUnnoticed2.Object3D.IngameManager;
using ExposureUnnoticed2.Master.Skill;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.UIPlayer
{
	public class UiPlayerController : MonoBehaviour
	{
		[SerializeField]
		private GameObject contentParent;

		[SerializeField]
		private Transform parent;

		private static readonly int PoseId = Animator.StringToHash("PoseId");

		private static readonly int Trans = Animator.StringToHash("Trans");

		private Animator uiPlayerAnimator;

		private UiPlayerAbsorbAnimationEvent uiPlayer;

		public static UiPlayerController Instance { get; private set; }

		public void Initialize()
		{
            // Original RVA 0x5EB800.
            Instance = this;
            GameObject instance = Instantiate(AssetReferencer.Instance.UiPlayer, parent);
            uiPlayerAnimator = instance.GetComponent<Animator>();
            uiPlayer = instance.GetComponent<UiPlayerAbsorbAnimationEvent>();
            Hide();
		}

		public void Dispose()
		{
            // Original RVA 0x5EBA30 clears the static instance through its setter.
            Instance = null;
		}

		public void ShowPose(SkillType type)
		{
            // Original RVA 0x5EBA80.
            Show();
            uiPlayerAnimator.SetInteger(PoseId, (int)type);
            uiPlayerAnimator.SetTrigger(Trans);
            uiPlayer.FutanariObject.SetActive(type == (SkillType)10019);
		}

		private void Show()
		{
            // Original RVA 0x5EBC70.
            contentParent.SetActive(true);
		}

		public void Hide()
		{
            // Original RVA 0x5EBCD0.
            contentParent.SetActive(false);
		}
	}
}
