using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerBoneScaleAdjustManager
	{
		private PlayerClassAccessor pca;

		private List<PlayerBoneScaleAdjuster> adjusterList;

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x639D90
			this.pca = pca;
			global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayFlame(() =>
			{
			    CreateAdjuster(pca.PlayerAvatarObjectReferencer.Hip);
			    CreateAdjuster(pca.PlayerAvatarObjectReferencer.LegL);
			    CreateAdjuster(pca.PlayerAvatarObjectReferencer.LegR);
			    CreateAdjuster(pca.PlayerAvatarObjectReferencer.LowerLegL);
			    CreateAdjuster(pca.PlayerAvatarObjectReferencer.LowerLegR);
			    CreateAdjuster(pca.PlayerAvatarObjectReferencer.UpperArmL);
			    CreateAdjuster(pca.PlayerAvatarObjectReferencer.UpperArmR);
			    CreateAdjuster(pca.PlayerAvatarObjectReferencer.LowerArmL);
			    CreateAdjuster(pca.PlayerAvatarObjectReferencer.LowerArmR);
			    CreateAdjuster(pca.PlayerAvatarObjectReferencer.Spine);
			    CreateAdjuster(pca.PlayerAvatarObjectReferencer.Chest);
			    CreateAdjuster(pca.PlayerAvatarObjectReferencer.Neck);
			}, 1);
		}

		private void CreateAdjuster(Transform target)
		{
			// Recovered from GameAssembly.dll RVA 0x639F50
			GameObject obj = new GameObject("PlayerBoneScaleAdjuster");
			PlayerBoneScaleAdjuster adjuster = obj.AddComponent<PlayerBoneScaleAdjuster>();
			adjuster.Initialize(target);
			adjusterList.Add(adjuster);
		}

		public void ReAdjust()
		{
			// Recovered from GameAssembly.dll RVA 0x63A080
			foreach (PlayerBoneScaleAdjuster adjuster in adjusterList) adjuster.UndoParent();
			pca.GameObject.SetActive(false);
			global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayFlame(() => pca.GameObject.SetActive(true), 1);
			global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayFlame(() =>
			{
			    foreach (PlayerBoneScaleAdjuster adjuster in adjusterList) adjuster.ChangeParent();
			}, 3);
		}
	// Recovered original member RVA 0x63A420: .ctor
		public PlayerBoneScaleAdjustManager()
        {
            adjusterList = new List<PlayerBoneScaleAdjuster>();
        }
	}
}
