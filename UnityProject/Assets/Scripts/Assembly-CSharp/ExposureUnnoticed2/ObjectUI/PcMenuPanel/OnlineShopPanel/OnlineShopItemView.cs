using System;
using ExposureUnnoticed2.Master.Cosplay;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.PcMenuPanel.OnlineShopPanel
{
	public class OnlineShopItemView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI itemName;

		[SerializeField]
		private TextMeshProUGUI rpText;

		[SerializeField]
		private RectTransform rpTextRect;

		[SerializeField]
		private GameObject purchasedMask;

		[SerializeField]
		private GameObject achievementMask;

		[SerializeField]
		private GameObject rankMask;

		[SerializeField]
		private TextMeshProUGUI rankMaskText;

		[SerializeField]
		private ButtonView buttonView;

		[SerializeField]
		private GameObject descriptionPanel;

		[SerializeField]
		private TextMeshProUGUI description;

		[SerializeField]
		private GameObject infoIconObject;

		[SerializeField]
		private GameObject optionButtonObject;

		[SerializeField]
		private GameObject HidePartsIcon;

		[SerializeField]
		private CanvasGroup emitCanvasGroup;

		private Action<IOnlineShopItemRecord, bool> onClick;

		private Action onClickPurchase;

		private Action<RCosplayParts> onClickOption;

		private IOnlineShopItemRecord record;

		private bool isBring;

		private ButtonGroupManager.IButtonView selfButtonView;

		private bool isViewDescription;

		private bool isExistDescription;

		private bool isSomeMask;

		public void Initialize(IOnlineShopItemRecord record, bool isBring, Action<IOnlineShopItemRecord, bool> onClick, Action onClickPurchase, Action<RCosplayParts> onClickOption = null)
		{
            // Whole original native4D6AB0, including all four linked-unwind ranges.
            this.onClick = onClick;
            this.onClickPurchase = onClickPurchase;
            this.record = record;
            this.isBring = isBring;
            selfButtonView = GetComponent<ButtonGroupManager.IButtonView>();
            itemName.text = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(record.NameKey);
            rpText.text = record.Price.ToString() + "RP";
            purchasedMask.SetActive(isBring);
            string descriptionKey = record.DescriptionKey;
            isExistDescription = descriptionKey != null && descriptionKey.Length > 0;
            descriptionPanel.SetActive(false);
            if (isExistDescription)
            {
                description.text = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(record.DescriptionKey);
                infoIconObject.SetActive(true);
            }
            else infoIconObject.SetActive(false);
            rankMask.SetActive(false);
            achievementMask.SetActive(false);
            buttonView.SetMask(false, null);
            if (!isBring && record is RCosplayParts parts)
            {
                if (!global::ExposureUnnoticed2.Master.Achievement.MAchievement.IsRelease(parts.ParentCosplayType))
                {
                    achievementMask.SetActive(true);
                    buttonView.SetMask(true, null);
                }
                else if (global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CurrentRank.Rank < parts.ReleaseRank)
                {
                    rankMask.SetActive(true);
                    int releaseRank = parts.ReleaseRank;
                    var targetRankText = rankMaskText;
                    string rankName = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetRankNameAutoHide(releaseRank);
                    targetRankText.text = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_release_at_rank", rankName);
                }
            }
            isSomeMask = purchasedMask.activeInHierarchy || rankMask.activeInHierarchy || achievementMask.activeInHierarchy;
            optionButtonObject.SetActive(false);
            HidePartsIcon.SetActive(false);
            if (record is RCosplayParts optionParts)
            {
                optionButtonObject.SetActive(optionParts.IsShowCustomizeButton);
                this.onClickOption = onClickOption;
                HidePartsIcon.SetActive((int)optionParts.HiddenPartsType != 0);
            }
		}

		public void ForceSetEmit(int alpha)
		{
            // Whole original4D74D0. The int-to-single conversion is not clamped.
            emitCanvasGroup.alpha = alpha;
		}

		private void Update()
		{
            // Whole original4D7540. Preserve both interface getter calls and the second field reread.
            if (isExistDescription && isViewDescription != selfButtonView.IsEmitting)
            {
                isViewDescription = selfButtonView.IsEmitting;
                descriptionPanel.SetActive(isViewDescription);
            }
		}

		public void OnClick()
		{
            // Whole original unwindless leaf4D75F0..4D761E, including the external delegate tail call.
            onClick?.Invoke(record, isBring);
		}

		public void OnClickPurchase()
		{
            // Whole original4D7620. The second callback field is read after the optional selection callback.
            if (isSomeMask) return;
            if (OnlineShopPanelView.Instance.CurrentRecord != record) onClick?.Invoke(record, isBring);
            onClickPurchase?.Invoke();
		}

		public void OnClickOption()
		{
            // Whole original4D76E0. A non-parts or null record is passed to the callback as null.
            onClickOption?.Invoke(record as RCosplayParts);
		}
	}
}
