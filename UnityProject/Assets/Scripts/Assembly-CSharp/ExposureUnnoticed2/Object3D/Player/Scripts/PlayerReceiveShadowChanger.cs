using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerReceiveShadowChanger
	{
		private PlayerClassAccessor pca;

		private bool isActiveCache;

		private List<Renderer> targetRenderer;

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x657650
			this.pca = pca;
			foreach (Renderer renderer in pca.PlayerAvatarObjectReferencer.Handcuffs.GetComponentsInChildren<Renderer>(true))
			    targetRenderer.Add(renderer);
			foreach (Renderer renderer in pca.PlayerAvatarObjectReferencer.BottleWater.GetComponentsInChildren<Renderer>(true))
			    targetRenderer.Add(renderer);
			targetRenderer.Add(pca.PlayerAvatarObjectReferencer.bodyMeshRenderer.GetComponent<Renderer>());
			targetRenderer.Add(pca.PlayerAvatarObjectReferencer.Face.GetComponent<Renderer>());
			targetRenderer.Add(pca.PlayerAvatarObjectReferencer.Coat.GetComponent<Renderer>());
			targetRenderer.Add(pca.PlayerAvatarObjectReferencer.Shoes.GetComponent<Renderer>());
			targetRenderer.Add(pca.PlayerAvatarObjectReferencer.FutanariMeshSao);
			targetRenderer.Add(pca.PlayerAvatarObjectReferencer.FutanariMeshTama);
			foreach (Renderer renderer in pca.PlayerAvatarObjectReferencer.PantsParent.GetComponentsInChildren<Renderer>(true))
			    targetRenderer.Add(renderer);
			foreach (Renderer renderer in pca.PlayerAvatarObjectReferencer.BraParent.GetComponentsInChildren<Renderer>(true))
			    targetRenderer.Add(renderer);
			foreach (GameObject obj in pca.PlayerAvatarObjectReferencer.Accessorys)
			{
			    Renderer renderer = obj.GetComponent<Renderer>();
			    if (renderer != null) targetRenderer.Add(renderer);
			    foreach (Renderer child in obj.GetComponentsInChildren<Renderer>(true))
			        targetRenderer.Add(child);
			}
			foreach (GameObject obj in pca.PlayerAvatarObjectReferencer.Hairs)
			    foreach (Renderer renderer in obj.GetComponentsInChildren<Renderer>(true))
			        targetRenderer.Add(renderer);
			foreach (var obj in pca.PlayerAvatarObjectReferencer.CosplayControllers)
			    foreach (Renderer renderer in obj.GetComponentsInChildren<Renderer>(true))
			        targetRenderer.Add(renderer);
			foreach (GameObject obj in pca.PlayerAvatarObjectReferencer.OtherCosplatPartForPRSC)
			{
			    Renderer renderer = obj.GetComponent<Renderer>();
			    if (renderer != null) targetRenderer.Add(renderer);
			    foreach (Renderer child in obj.GetComponentsInChildren<Renderer>(true))
			        targetRenderer.Add(child);
			}
		}

		public void SetReceiveShadow(bool isActive)
		{
			// Recovered from GameAssembly.dll RVA 0x657FD0
			if (isActiveCache == isActive) return;
			isActiveCache = isActive;
			foreach (Renderer renderer in targetRenderer)
			    renderer.receiveShadows = isActive;
		}
	// Recovered original member RVA 0x658170: .ctor
		public PlayerReceiveShadowChanger()
        {
            isActiveCache = true;
            targetRenderer = new List<Renderer>();
        }
	}
}
