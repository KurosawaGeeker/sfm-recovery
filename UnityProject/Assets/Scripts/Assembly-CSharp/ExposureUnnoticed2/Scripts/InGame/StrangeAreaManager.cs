using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class StrangeAreaManager : MonoBehaviour
	{
		[SerializeField]
		private StrangeAreaController[] areaControllers;

		public static StrangeAreaManager Instance { get; private set; }

		private void Awake()
		{
			if (Instance != null) return; Instance = this;
		}

		private void OnDestroy()
		{
			if (Instance == this) Instance = null;
		}

		public float GetStrange()
		{
			StrangeAreaController[] controllers = areaControllers;
			float maximum = 0f;
			for (int i = 0; i < controllers.Length; i++)
			{
			    float next = controllers[i].GetCurrentStrange();
			    if (maximum < next || float.IsNaN(next)) maximum = next;
			}
			return maximum;
		}
	}
}
