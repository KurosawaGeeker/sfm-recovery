using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InGame.RingMenu
{
	public class RingItemView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI actionName;

		private Animator animator;

		private static readonly int IsActive = Animator.StringToHash("IsActive");

		public RingMenuItemModel Model { get; private set; }

		public bool IsRelease { get; private set; }

		public void Initialize(RingMenuItemModel model)
		{
Model = model;
            IsRelease = true;
            if (model != null && model.ReleaseCondition != null) IsRelease = model.ReleaseCondition();
            var capturedName = actionName;
            string text = model == null || !IsRelease ? "-" : global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(model.NameKey);
            if (capturedName is null) throw new System.NullReferenceException();
            capturedName.text = text;
        }

		private void Start()
		{
animator = GetComponent<Animator>();
        }

		private void Update()
		{
		}

		public void SetActive(bool isActive)
		{
animator.SetBool(IsActive, isActive);
        }
	}
}
