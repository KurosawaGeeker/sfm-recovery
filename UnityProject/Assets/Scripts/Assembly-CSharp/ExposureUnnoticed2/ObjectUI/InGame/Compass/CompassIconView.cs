using ExposureUnnoticed2.Scripts.InGame;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InGame.Compass
{
	public class CompassIconView : MonoBehaviour
	{
		[SerializeField]
		private GameObject clothesIcon;

		[SerializeField]
		private GameObject keyIcon;

		[SerializeField]
		private GameObject switchIcon;

		[SerializeField]
		private GameObject portalIcon;

		public void Initialize(DroppedItemManager.DropItemType type)
		{
            clothesIcon.SetActive(false);
            keyIcon.SetActive(false);
            switchIcon.SetActive(false);
            portalIcon.SetActive(false);
            if (DroppedItemManager.Instance.IsClothes(type)) clothesIcon.SetActive(true);
            else if (type == DroppedItemManager.DropItemType.HandcuffKey) keyIcon.SetActive(true);
            else if (type == DroppedItemManager.DropItemType.VibeRemocon) switchIcon.SetActive(true);
        }

		public void InitializePortal()
		{
            clothesIcon.SetActive(false);
            keyIcon.SetActive(false);
            switchIcon.SetActive(false);
            portalIcon.SetActive(true);
        }
	}
}
