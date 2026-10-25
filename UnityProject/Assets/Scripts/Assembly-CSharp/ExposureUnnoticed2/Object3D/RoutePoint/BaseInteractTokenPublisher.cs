using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.RoutePoint
{
	public class BaseInteractTokenPublisher : MonoBehaviour
	{
		private int remainTokenCount;

		private int maxNum;

		protected void Awake()
		{
			// Original RVA 0x60D5D0; complete original body, unchanged owner/signature.
			EventManager.AddListener<ChangeTimeEvent>(OnChangeTime, false);
		}

		private void OnDestroy()
		{
			// Original RVA 0x60D690; complete original body, unchanged owner/signature.
			EventManager.RemoveListener<ChangeTimeEvent>(OnChangeTime);
		}

		private void OnChangeTime(ChangeTimeEvent evt)
		{
			// Original RVA 0x60D750; complete original body, unchanged owner/signature.
			remainTokenCount = maxNum;
		}

		protected void SetMaxTokenNum(int maxTokenNum)
		{
			// Original RVA 0x60D760; complete original body, unchanged owner/signature.
			remainTokenCount = maxTokenNum;
			maxNum = maxTokenNum;
		}

		public bool TryGetToken()
		{
			if (remainTokenCount > 0)
			{
			    --remainTokenCount;
			    return true;
			}
			return false;
		}

		public void ReturnToken()
		{
            // Original RVA 0x608DA0; signed Int32 increment wraps before minimum.
            int incremented = unchecked(remainTokenCount + 1);
            int maximum = maxNum;
            remainTokenCount = incremented <= maximum ? incremented : maximum;
        }
	}
}
