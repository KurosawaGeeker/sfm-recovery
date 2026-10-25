using Common.CommonUI.CommonGauge;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InGame.MissionPanel
{
	public class MissionItemPanelView : MonoBehaviour
	{
		[SerializeField]
		private new TextMeshProUGUI name;

		[SerializeField]
		private TextMeshProUGUI rewardText;

		[SerializeField]
		private TextMeshProUGUI rewardPerSecText;

		[SerializeField]
		private TextMeshProUGUI completeText;

		[SerializeField]
		private GaugeFillView gauge;

		[SerializeField]
		private GaugeView continueGauge;

		private Transform dummyView;

		private IMissionItemPanelModel mission;

		private Animator animator;

		private float remainStartPerSecTime = 1f;

		private bool TransformedPerSec;

		private float lastDisplayRpPerSec = -1f;

		private static readonly int TransOut = Animator.StringToHash("TransOut");

		private static readonly int Complete = Animator.StringToHash("Complete");

		private static readonly int ContinueComplete = Animator.StringToHash("ContinueComplete");

		private static readonly int IsPerSec = Animator.StringToHash("IsPerSec");

		private static readonly int IsPerSecDirect = Animator.StringToHash("IsPerSecDirect");

		public void Initialize(IMissionItemPanelModel missionBase, Transform dummyView)
		{
            this.dummyView = dummyView;
            mission = missionBase;
            animator = GetComponent<Animator>();
            global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayFlame(() =>
            {
                var target = transform;
                var source = transform;
                Vector3 old = source.localPosition;
                global::System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
                target.localPosition = new Vector3(1160f, old.y, old.z);
                var nextTarget = transform;
                var nextSource = transform;
                Vector3 position = nextSource.position;
                Vector3 dummyPosition = dummyView.position;
                nextTarget.position = new Vector3(position.x, dummyPosition.y, position.z);
            }, 1);
            continueGauge.UseAlphaInitialize();
            name.text = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(missionBase.MissionName);
            completeText.text = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(
                missionBase.MissionType == global::ExposureUnnoticed2.Master.Mission.MissionType.Time ? "ui_mission_complete" : "ui_mission_achieve");
            GetComponent<CanvasGroup>().alpha = 0f;
            int reward = mission.RewardRp;
            if (missionBase is global::ExposureUnnoticed2.Scripts.DroneMission.DroneMissionBase)
            {
                var stage = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType;
                var stageRecord = global::ExposureUnnoticed2.Master.Stage.MStage.Get(stage);
                reward = unchecked((int)((float)reward * stageRecord.DroneMissionRpBonus));
            }
            var label = rewardText;
            string rewardValue;
            var rewardType = missionBase.MissionType;
            switch (rewardType)
            {
                case global::ExposureUnnoticed2.Master.Mission.MissionType.Time:
                case global::ExposureUnnoticed2.Master.Mission.MissionType.All:
                    rewardValue = reward.ToString() + global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_mission_rp_per_second");
                    break;
                case global::ExposureUnnoticed2.Master.Mission.MissionType.Condition:
                case global::ExposureUnnoticed2.Master.Mission.MissionType.Continue:
                case global::ExposureUnnoticed2.Master.Mission.MissionType.Drone:
                    rewardValue = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(reward.ToString() + "RP");
                    break;
                case global::ExposureUnnoticed2.Master.Mission.MissionType.Random:
                    rewardValue = "";
                    break;
                default:
                    throw new global::System.Runtime.CompilerServices.SwitchExpressionException(rewardType);
            }
            if (label is null) throw new global::System.NullReferenceException();
            label.text = rewardValue;
            if (mission.Satisfy >= 1f && mission.IsPerSec) TransformPerSec(true);
		}

		private void LateUpdate()
		{
            var target = transform;
            var source = transform;
            Vector3 current = source.position;
            Vector3 next = dummyView.position;
            target.position = Vector3.Lerp(current, next, 0.1f);
            var progressGauge = gauge;
            float progress = mission.Satisfy;
            if (progressGauge is null) throw new global::System.NullReferenceException();
            progressGauge.Set(progress);
            if (mission.Satisfy >= 1f && !TransformedPerSec)
            {
                var capturedAnimator = animator;
                capturedAnimator.SetTrigger(Complete);
                remainStartPerSecTime -= Time.unscaledDeltaTime;
                if (mission.IsPerSec && remainStartPerSecTime <= 0f) TransformPerSec();
            }
            if (mission.Satisfy >= 1f && mission.IsPerSec && lastDisplayRpPerSec != mission.RpPerSec)
            {
                float value = mission.RpPerSec;
                lastDisplayRpPerSec = value;
                var label = rewardPerSecText;
                string text = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_rpPerSec", string.Format("{0:N1}", value));
                label.text = text;
            }
		}

		private void TransformPerSec(bool isDirect = false)
		{
            if (TransformedPerSec) return;
            var captured = animator;
            TransformedPerSec = true;
            captured.SetBool(isDirect ? IsPerSecDirect : IsPerSec, true);
            var rect = dummyView.GetComponent<RectTransform>();
            Vector2 width = rect.sizeDelta;
            Vector2 height = rect.sizeDelta;
            global::System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
            rect.sizeDelta = new Vector2(width.x, height.y - 15f);
		}

		public void DestroyItem()
		{
            animator.SetTrigger(TransOut);
		}

		public void OnTransOutAnimEnd()
		{
            Object.Destroy(dummyView.gameObject);
            Object.Destroy(gameObject);
		}
	}
}
