using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerFaceShadowOnlyController
	{
		private class Model
		{
			public GameObject GameObject;

			public GameObject Original;
		}

		private PlayerClassAccessor pca;

		private List<Model> list;

		private int playerLayer;

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x64D2B0
			this.pca = pca;
			foreach (GameObject original in pca.PlayerAvatarObjectReferencer.OnlyShadowCloneObjects)
			    list.Add(new Model { GameObject = null, Original = original });
		}

		public void OnUpdate()
		{
		}
	// Recovered original member RVA 0x64DBF0: .ctor
		public PlayerFaceShadowOnlyController()
        {
            list = new List<Model>();
            playerLayer = LayerMask.NameToLayer("Player");
        }
	}
}
