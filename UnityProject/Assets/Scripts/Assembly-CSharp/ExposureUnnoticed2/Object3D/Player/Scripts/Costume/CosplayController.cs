using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Costume
{
	public class CosplayController : MonoBehaviour
	{
		[SerializeField]
		private GameObject[] parts;

		public void SetActiveParts(int id, bool isActive)
		{
			// Recovered from GameAssembly.dll RVA 0x4086D0
			parts[id].SetActive(isActive);
		}

		public bool IsActiveParts(int id)
		{
			return false;
		}
	}
}
