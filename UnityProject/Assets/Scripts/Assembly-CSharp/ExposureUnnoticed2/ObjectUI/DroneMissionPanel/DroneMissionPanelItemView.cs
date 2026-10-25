using System;
using ExposureUnnoticed2.Master.DroneMission;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.DroneMissionPanel
{
	public class DroneMissionPanelItemView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI text;

		[SerializeField]
		private TextMeshProUGUI clearCount;

		[SerializeField]
		private GameObject rankMaskObject;

		[SerializeField]
		private GameObject orderMaskObject;

		[SerializeField]
		private GameObject randomStartMaskObject;

		[SerializeField]
		private GameObject achievedBadge;

		[SerializeField]
		private CanvasGroup emitCanvasGroup;

		private RDroneMission record;

		private Action<RDroneMission> onClick;

		public void Initialize(RDroneMission record, Action<RDroneMission> onClick, bool isOrderLock)
		{
		}

		public void OnClick()
		{
		}

		public void ForceSetEmit(int alpha)
		{
		}
	}
}
