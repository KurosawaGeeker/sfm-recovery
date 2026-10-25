using Common.CommonUI.CommonSettingItem;
using ExposureUnnoticed2.Master.Cosplay;
using ExposureUnnoticed2.ObjectUI.OptionMenu;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InventoryPanel.PartsOptionSetupPanel
{
	public class PartsOptionSetupPanelView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI titleText;

		[SerializeField]
		private GameObject colorObject;

		[SerializeField]
		private CustomizeColorItemView color;

		[SerializeField]
		private CustomizeBarItemView r;

		[SerializeField]
		private CustomizeBarItemView g;

		[SerializeField]
		private CustomizeBarItemView b;

		[SerializeField]
		private CustomizeBarItemView shadeColor;

		[SerializeField]
		private GameObject materialObject;

		[SerializeField]
		private SettingSelectItemView materialType;

		[SerializeField]
		private GameObject boolOption1Object;

		[SerializeField]
		private CustomizeCheckItemView boolOption1;

		[SerializeField]
		private TextMeshProUGUI boolOption1Text;

		[SerializeField]
		private GameObject boolOption2Object;

		[SerializeField]
		private CustomizeCheckItemView boolOption2;

		[SerializeField]
		private TextMeshProUGUI boolOption2Text;

		[SerializeField]
		private CustomizeCheckItemView invisibleOption;

		private Animator animator;

		private bool isOpen;

		private bool isOpenChild;

		private static readonly int Close = Animator.StringToHash("Close");

		private CosplayPartsOptionData cacheOptionData;

		private RCosplayParts record;

		private string[] materialNames;

		public void Initialize()
		{
            // Complete original RVA 0x4F43B0.
            animator = GetComponent<Animator>();
            gameObject.SetActive(false);
        }

		private void Update()
		{
		}

		private void OnChange()
		{
		}

		public void Open(RCosplayParts record)
		{
		}

		public void OnClickOk()
		{
		}

		public void OnAnimCloseEnd()
		{
		}
		// Complete original constructor RVA 0x4F5720.
		public PartsOptionSetupPanelView()
		{
            materialNames = new string[] { "A", "B", "C", "D", "E", "F", "G" };
		}

	}
}
