using ExposureUnnoticed2.Object3D.NPC.Script;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InGame.NpcDirectIconPanel
{
	public class NpcDirectionPanelView : MonoBehaviour
	{
		private static NpcDirectionPanelView instance;

		[SerializeField]
		private GameObject arrowPrefab;

		public void Initialize()
		{
			// Recovered from GameAssembly.dll RVA 0x507980
			instance = this;
		}

		public static void Register(NpcController npc)
		{
			NpcDirectionPanelView panel = instance;
			UnityEngine.Object.Instantiate(panel.arrowPrefab, panel.transform).GetComponent<NpcDirectionArrowView>().Initialize(npc);
		}
	}
}
