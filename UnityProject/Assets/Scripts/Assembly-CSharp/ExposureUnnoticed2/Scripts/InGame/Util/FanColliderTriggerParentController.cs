using System;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame.Util
{
	public class FanColliderTriggerParentController : MonoBehaviour, IColliderTriggerController
	{
		[SerializeField]
		private ColliderTriggerController sphere;

		[SerializeField]
		private ColliderTriggerController box1;

		[SerializeField]
		private ColliderTriggerController box2;

		public bool IsInArea => false;

		public float PenetrateDist
        {
            get
            {
                float first = box1.PenetrateDist;
                float second = box2.PenetrateDist;
                return first > second ? first : second;
            }
        }

		public GameObject CenterObject => sphere.gameObject;

		public GameObject GameObject => null;

		public void Initialize(Func<Collider, bool> condition)
		{
		}
	}
}
