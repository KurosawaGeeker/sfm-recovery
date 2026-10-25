using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Hair
{
	public class HairHatApplyController : MonoBehaviour
	{
		[SerializeField]
		private GameObject hideTarget;

		[SerializeField]
		private GameObject shapeKeyTarget;

		[SerializeField]
		private int shapeKeyIndex;

		private SkinnedMeshRenderer renderer;

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnEnable()
		{
		}

		private void OnChangeEvent(ChangeCostumeCosplayEvent evt)
		{
		}
	}
}
