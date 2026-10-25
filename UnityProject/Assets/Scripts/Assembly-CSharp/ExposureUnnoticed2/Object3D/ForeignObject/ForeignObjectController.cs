using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ForeignObject
{
	public class ForeignObjectController : MonoBehaviour
	{
		[field: SerializeField]
		public float MaxDiscomfort { get; private set; }

		[field: SerializeField]
		public ForeignObjectFaceType ForeignObjectFaceType { get; private set; }
	}
}
