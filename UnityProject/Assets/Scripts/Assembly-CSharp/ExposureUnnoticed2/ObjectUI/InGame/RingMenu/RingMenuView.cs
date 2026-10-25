using System;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InGame.RingMenu
{
	public class RingMenuView : MonoBehaviour
	{
		private enum State
		{
			Show = 0,
			Hide = 1
		}

		private enum Direct
		{
			Up = 0,
			Right = 1,
			Down = 2,
			Left = 3,
			None = 4
		}

		[SerializeField]
		private RectTransform ringDirectRect;

		[SerializeField]
		private RectTransform content;

		[SerializeField]
		private CanvasGroup ringCanvasGroup;

		[SerializeField]
		private int aaa;

		[SerializeField]
		private float bbb;

		private State currentState;

		private Animator animator;

		private RingItemView[] itemView;

		private RingItemView currentSelectView;

		private int ringShortCutIndex;

		private Vector2 mouseWeight;

		private float currentAngle;

		private bool isByLongPress;

		private InputManager.InputType targetKey;

		private static readonly int IsShow = Animator.StringToHash("IsShow");

		private Action<RingMenuItemModel> onInvokeCommand;

		private int ringIndex;

		private RingMenuParentView parentView;

		private SmoothFloat positionX = new SmoothFloat(0.1f, 1f, 0f, true, false, -1f);

		private SmoothFloat ringAlpha = new SmoothFloat(0.05f, 1f, 0f, false, false, -1f);

		public bool IsOpen => currentState == State.Show;

		public void Initialize(RingMenuParentView parentView, RingMenuItemModel[] itemModels, Action<RingMenuItemModel> onInvokeCommand, int ringIndex = 0)
		{
this.parentView = parentView;
            this.ringIndex = ringIndex;
            this.onInvokeCommand = onInvokeCommand;
            animator = GetComponent<Animator>();
            itemView = GetComponentsInChildren<RingItemView>(false);
            animator.SetBool(IsShow, true);
            positionX.Current = unchecked((ringIndex - parentView.CurrentRingIndex()) * aaa);
            ringAlpha.Current = ringIndex == parentView.CurrentRingIndex() ? 1f : 0f;
            mouseWeight = AngleToVector2(360 * (parentView.CurrentShortcutIndex() % 8) / 8f + 22.5f) * 1000f;
            currentAngle = Vector2ToAngle(mouseWeight);
            ringDirectRect.rotation = Quaternion.AngleAxis(currentAngle - 90f, Vector3.forward);
            for (int i = 0; i < itemView.Length && i < 8; i++)
            {
                var capturedItem = itemView[i];
                int modelIndex = unchecked(i + ringIndex * 8);
                RingMenuItemModel model = modelIndex < itemModels.Length ? itemModels[modelIndex] : null;
                if (capturedItem is null) throw new NullReferenceException();
                capturedItem.Initialize(model);
            }
        }

		public void LateUpdate()
		{
if (global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.IsOpenSomeUi) return;
            if (currentState == State.Show) ShowUpdate();
            positionX.SetSecond(bbb);
            positionX.UpdateTo(unchecked((ringIndex - parentView.CurrentRingIndex()) * aaa));
            var capturedContent = content;
            Vector3 previous = capturedContent.localPosition;
            capturedContent.localPosition = new Vector3(positionX.Current, previous.y, previous.z);
            ringAlpha.UpdateTo(ringIndex == parentView.CurrentRingIndex() ? 1f : 0f);
            ringCanvasGroup.alpha = ringAlpha.Current;
        }

		private void ChangeState(State nextState)
		{
currentState = nextState;
            if (nextState == State.Show)
            {
                animator.SetBool(IsShow, true);
            }
            else if (nextState == State.Hide)
            {
                animator.SetBool(IsShow, false);
                if (currentSelectView && currentSelectView.IsRelease && !parentView.IsClickToAction
                    && ringIndex == parentView.CurrentRingIndex())
                    onInvokeCommand(currentSelectView.Model);
            }
        }

		public void OnLongPressActionSlot(int index)
		{
ringShortCutIndex = index;
            isByLongPress = true;
            ChangeState(State.Show);
        }

		public void SetShow(bool isShow)
		{
            ChangeState(isShow ? State.Show : State.Hide);
        }

		private void ShowUpdate()
		{
AngleUpdate();
            if (parentView.CurrentRingIndex() == ringIndex)
            {
                if (InputManager.IsDownFrame((InputManager.InputType)15))
                {
                    if (currentSelectView && currentSelectView.Model != null && currentSelectView.IsRelease)
                    {
                        if (currentSelectView.Model.OnSelect == null)
                            global::ExposureUnnoticed2.Object3D.Player.Scripts.InputResolver.RingShortcutInput = currentSelectView.Model.RingItemType;
                        else
                            currentSelectView.Model.OnSelect();
                        parentView.IsClickToAction = true;
                    }
                }
                if (!(mouseWeight.x * mouseWeight.x + mouseWeight.y * mouseWeight.y > 0f)) return;
                _ = Vector2.up;
                _ = Vector2.down;
                _ = Vector2.left;
                _ = Vector2.right;
                float slot = (-currentAngle + 90f + 360f) % 360f / 45f;
                for (int i = 0; i < itemView.Length; i++)
                {
                    itemView[i].SetActive(i == (int)slot);
                    if (i == (int)slot) currentSelectView = itemView[i];
                }
            }
            else
            {
                var capturedItems = itemView;
                foreach (var item in capturedItems) item.SetActive(false);
            }
        }

		private void AngleUpdate()
		{
Vector2 input = InputManager.GetVector2((InputManager.InputType)1);
            if (InputManager.IsGamePad) input *= 1000f;
            if (input.x * input.x + input.y * input.y >= 0.01f)
            {
                Vector2 normalized = input.normalized;
                float factor = -(normalized.x * mouseWeight.x + normalized.y * mouseWeight.y) + 1f;
                if (factor < 0f) factor = 0f;
                else if (factor > 1f) factor = 1f;
                Vector2 previous = mouseWeight;
                float delta = Time.deltaTime;
                mouseWeight.x = input.x * 7f * delta * factor + previous.x;
                mouseWeight.y = input.y * 7f * delta * factor + previous.y;
                if (input.x * input.x + input.y * input.y > 1f) mouseWeight.Normalize();
                currentAngle = Vector2ToAngle(mouseWeight);
                ringDirectRect.rotation = Quaternion.AngleAxis(currentAngle - 90f, Vector3.forward);
            }
        }

		private void HideUpdate()
		{
		}

		public static float CalcAngle(Vector2 a, Vector2 b)
		{
float dot = a.y * b.y + a.x * b.x;
            float ma = (float)System.Math.Sqrt(a.y * a.y + a.x * a.x);
            float mb = (float)System.Math.Sqrt(b.y * b.y + b.x * b.x);
            return Mathf.Acos(dot / (ma * mb));
        }

		public static float Vector2ToAngle(Vector2 vector)
		{
return Mathf.Atan2(vector.y, vector.x) * 57.29578f;
        }

		public static Vector2 AngleToVector2(float angle)
		{
float radians = angle * 0.017453292f;
            float x = Mathf.Cos(radians);
            return new Vector2(x, Mathf.Sin(radians));
        }
	}
}
