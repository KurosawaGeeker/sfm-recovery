using ExposureUnnoticed2.Object3D.NPC.Script;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.NpcUi
{
	public class NpcUiView : MonoBehaviour
	{
		[SerializeField]
		private GameObject UiObjects;

		[SerializeField]
		private Image circleGauge;

		[SerializeField]
		private Image subCircleGauge;

		[SerializeField]
		private GameObject questionObject;

		private NpcController target;

		private Animator animator;

		private RectTransform rect;

		private NpcComponentAccessor nca;

		private bool IsBeforeZeroStrange;

		private static readonly int Strangeness = Animator.StringToHash("Strangeness"); // Original .cctor RVA 0x4DEA90

		private void Initialize(NpcComponentAccessor target)
		{
			animator = GetComponent<Animator>();
			this.nca = target;
			this.target = target.NpcController;
			rect = GetComponent<RectTransform>();
		}

		private void LateUpdate()
		{
            // Original GameAssembly.dll RVA 0x4DDEE0; only original NPC feedback state.
            if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsActivePassiveSkill(global::ExposureUnnoticed2.Master.Skill.SkillType.HideStrangeUi)
                || global::ExposureUnnoticed2.Scripts.InGame.SexManager.Instance.ActiveTraceSexPlayerNpc == target)
            {
                UiObjects.SetActive(false);
                return;
            }
            if (target.StrangenessValue <= 0f && IsBeforeZeroStrange) return;
            IsBeforeZeroStrange = target.StrangenessValue <= 0f;
            questionObject.SetActive(target.StrangenessValue >= global::ExposureUnnoticed2.Master.Common.MCommon.Instance.StrangeLookAt);
            UpdateLockOnEnemyHpView();
		}

		private void UpdateLockOnEnemyHpView()
		{
            // Original GameAssembly.dll RVA 0x4DE090; presentation reads no gameplay writes.
            if (target == null || !target.IsAlive || global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.GameOverController.IsOnCameraStaging)
            {
                Destroy(gameObject);
                return;
            }
            bool inFogRange = !global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.VisibilityFogController.IsBadVisibility || target.PlayerDistance <= 3f;
            bool sameElevatorSide = !global::ExposureUnnoticed2.Object3D.ScenePlops.Elevator.ElevatorController.Instance
                || global::ExposureUnnoticed2.Object3D.ScenePlops.Elevator.ElevatorController.Instance.IsDoorOpenOverHalf()
                || global::ExposureUnnoticed2.Object3D.ScenePlops.Elevator.ElevatorController.Instance.IsInsidePlayer()
                   == global::ExposureUnnoticed2.Object3D.ScenePlops.Elevator.ElevatorController.Instance.IsInArea(nca.Transform.position);
            bool visible = (target.Nca.StateModel.CurrentFixedType == global::ExposureUnnoticed2.Object3D.NPC.Script.NpcStateModel.FixedType.Conbini && target.IsInCameraArea) || target.IsVisible;
            UiObjects.SetActive(inFogRange && visible && sameElevatorSide);
            Vector2 screen = RectTransformUtility.WorldToScreenPoint(Camera.main, target.EyePosition.position);
            rect.position = new Vector3(screen.x + 14f, screen.y + 20f, 0f);
            animator.SetFloat(Strangeness, target.StrangenessValue);
            circleGauge.fillAmount = target.StrangenessValue;
            float fraction = Mathf.InverseLerp(0f, 1f, target.StrangenessValue);
            circleGauge.color = nca.StateModel.IsRiseStrangeFromOnlyDiscomfort
                ? Color.gray
                : Color.Lerp(new Color(1f, 235f / 255f, 4f / 255f, 1f), Color.red, fraction);
		}

		public static void Create(NpcComponentAccessor target)
		{
			GameObject prefab = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.NpcUiView;
			Transform parent = global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.BackLayer;
			GameObject created = UnityEngine.Object.Instantiate(prefab, parent);
			if (created is null) throw new System.NullReferenceException();
			NpcUiView view = created.GetComponent<NpcUiView>();
			if (view is null) throw new System.NullReferenceException();
			view.Initialize(target);
		}
	}
}
