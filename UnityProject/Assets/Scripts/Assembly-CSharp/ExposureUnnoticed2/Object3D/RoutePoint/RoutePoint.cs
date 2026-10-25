using UnityEngine;

namespace ExposureUnnoticed2.Object3D.RoutePoint
{
	public class RoutePoint : MonoBehaviour
	{
		[field: SerializeField]
		public RoutePointType Type { get; private set; }

		[field: SerializeField]
		public RouteInteractType RouteInteractType { get; private set; }

		[field: SerializeField]
		public BaseInteractTokenPublisher LinkTokenPublisher { get; private set; }

		[field: SerializeField]
		public int Floor { get; set; }

		[field: SerializeField]
		public Transform EscalatorTarget { get; private set; }

		[field: SerializeField]
		public float MaxPathRightScale { get; private set; }

		[field: SerializeField]
		public int Param { get; set; }

		public Vector3 Position => transform.position;

		public Vector3 PathRight { get; set; }
	}
}
