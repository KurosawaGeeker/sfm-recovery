using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.CosplayPartsOptionApplyer
{
	public class OptionApplyerNinjaOmen : MonoBehaviour
	{
		[SerializeField]
		private int partsId;

		[SerializeField]
		private Transform normalParent;

		[SerializeField]
		private Transform faceParent;

		[SerializeField]
		private Transform moveTarget;

		private Vector3 initLocalPosition;

		private Vector3 initLocalScale;

		private Quaternion initLocalRotate;

		private int uniqueId;

		private bool isNormal;

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnChangeEvent(ChangeCosplayPartsOptionEvent evt)
		{
		}

		private void ApplyFromGameState()
		{
		}

		private void OnChangeBody(ChangeBodyEvent evt)
		{
		}
	}
}
