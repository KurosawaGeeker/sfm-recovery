using ExposureUnnoticed2.Object3D.InteractArea;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcComponent : MonoBehaviour
	{
		[SerializeField]
		public AudioSource DokinAudioSource;

		[SerializeField]
		public GameObject CeilingPlayerBlocker;

		[SerializeField]
		public GameObject PrefabFemale;

		[SerializeField]
		public GameObject PrefabMale;

		[SerializeField]
		public GameObject PrefabPreviousFemale;

		[SerializeField]
		public GameObject PrefabPreviousMale;

		[SerializeField]
		public GameObject[] PrefabOjis;

		[SerializeField]
		public GameObject[] PrefabObas;

		[SerializeField]
		public Transform Smartphone;

		[SerializeField]
		public Transform SmartphoneLight;

		[SerializeField]
		public Transform Book;

		[SerializeField]
		public Transform AvatarParent;

		[SerializeField]
		public InteractAreaController InteractArea;

		[SerializeField]
		public int id;

		public GameObject AvaterObject { get; set; }

		public NpcController NpcController { get; private set; }

		public NpcController CreateController()
		{
			NpcController = new NpcController(this);
			return NpcController;
		}
	}
}
