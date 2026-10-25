using ExposureUnnoticed2.Scripts.Base;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InGame.RingMenu
{
	public class ShortcutItemView : MonoBehaviour
	{
		[SerializeField]
		private new TextMeshProUGUI name;

		[SerializeField]
		private TextMeshProUGUI buttonText;

		private Animator animator;

		private static readonly int IsEmit = Animator.StringToHash("IsEmit");

		private int chaseIndex;

		private int autoRunIndex;

		private int timeStopIndex;

		private RingMenuParentView.RingMenuType type;

		public void Initialize(RingMenuParentView.RingMenuType type, RingMenuItemModel[] models)
		{
            // Complete original 506260..5067AC, including its linked second unwind segment.
            this.type = type;
            int index = type switch
            {
                RingMenuParentView.RingMenuType.Common => global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexCommon,
                RingMenuParentView.RingMenuType.Skill => global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill,
                RingMenuParentView.RingMenuType.Skill3 => global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill3,
                RingMenuParentView.RingMenuType.Skill4 => global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill4,
                RingMenuParentView.RingMenuType.Skill5 => global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill5,
                RingMenuParentView.RingMenuType.Skill6 => global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill6,
                RingMenuParentView.RingMenuType.Skill7 => global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill7,
                RingMenuParentView.RingMenuType.Skill8 => global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill8,
                _ => throw new global::System.Runtime.CompilerServices.SwitchExpressionException(type)
            };
            RingMenuItemModel model = global::System.Linq.Enumerable.FirstOrDefault(models, f => f.Index == index);
            name.text = index != -1 && model != null ? Localizer.Trans(model.NameKey) : "-";
            animator = GetComponent<Animator>();
            for (int i = 0; i < models.Length; i++)
            {
                RingMenuItemModel current = models[i];
                if (current.RingItemType == RingMenuParentView.RingItemType.AutoChase) chaseIndex = current.Index;
                if (current.RingItemType == RingMenuParentView.RingItemType.AutoRun) autoRunIndex = current.Index;
                if (current.RingItemType == RingMenuParentView.RingItemType.TimeStop) timeStopIndex = current.Index;
            }
            Scheduler.DelayFlame(() => Refresh(null), 1);
            EventManager.AddListener<OptionChangeEvent>(Refresh);
        
}

		private void OnDestroy()
		{
            EventManager.RemoveListener<OptionChangeEvent>(Refresh);
        }

		public void SetName(RingMenuItemModel model)
		{
            var capturedName = name;
            string text = model is null ? "-" : Localizer.Trans(model.NameKey);
            if (capturedName is null) throw new System.NullReferenceException();
            capturedName.text = text;
        }

		public void OnLateUpdate()
		{
if (InputManager.JustChangeIsGamePad)
            {
                var capturedText = buttonText;
                bool enabled = !InputManager.IsGamePad;
                if (capturedText is null) throw new System.NullReferenceException();
                capturedText.enabled = enabled;
            }
            int index;
            switch (type)
            {
                case RingMenuParentView.RingMenuType.Common: index = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexCommon; break;
                case RingMenuParentView.RingMenuType.Skill: index = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill; break;
                case RingMenuParentView.RingMenuType.Skill3: index = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill3; break;
                case RingMenuParentView.RingMenuType.Skill4: index = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill4; break;
                case RingMenuParentView.RingMenuType.Skill5: index = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill5; break;
                case RingMenuParentView.RingMenuType.Skill6: index = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill6; break;
                case RingMenuParentView.RingMenuType.Skill7: index = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill7; break;
                case RingMenuParentView.RingMenuType.Skill8: index = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.RingShortcutModelIndexSkill8; break;
                default: throw new System.ArgumentOutOfRangeException();
            }
            bool emit = (global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.IsAutoChase && index == chaseIndex)
                || (global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.IsAutoRun && index == autoRunIndex)
                || (global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.IsTimeStop && index == timeStopIndex);
            animator.SetBool(IsEmit, emit);
        }

		private void Refresh(OptionChangeEvent evt)
		{
            // Original Refresh 506CB0..506E04, including exact ArgumentOutOfRangeException default.
            InputManager.InputType input = type switch
            {
                RingMenuParentView.RingMenuType.Common => (InputManager.InputType)14,
                RingMenuParentView.RingMenuType.Skill => (InputManager.InputType)23,
                RingMenuParentView.RingMenuType.Skill3 => (InputManager.InputType)27,
                RingMenuParentView.RingMenuType.Skill4 => (InputManager.InputType)28,
                RingMenuParentView.RingMenuType.Skill5 => (InputManager.InputType)1018,
                RingMenuParentView.RingMenuType.Skill6 => (InputManager.InputType)1019,
                RingMenuParentView.RingMenuType.Skill7 => (InputManager.InputType)1020,
                RingMenuParentView.RingMenuType.Skill8 => (InputManager.InputType)1021,
                _ => throw new global::System.ArgumentOutOfRangeException()
            };
            buttonText.text = InputManager.GetString(input) + ":";
        
}
	}
}
