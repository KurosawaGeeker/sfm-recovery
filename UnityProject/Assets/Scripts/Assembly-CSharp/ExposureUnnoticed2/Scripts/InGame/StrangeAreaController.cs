using ExposureUnnoticed2.Object3D.ScenePlops.Toilet;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class StrangeAreaController : MonoBehaviour
	{
		[SerializeField]
		public float strangenessValue;

		[SerializeField]
		private InSidePlayerChecker playerChecker;

		public float GetCurrentStrange()
		{
			return playerChecker.IsInArea ? strangenessValue : 0f;
		}
	}
}
