using System.Collections.Generic;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcFadeOutState
	{
		private const float FadeTime = 1f;

		private NpcComponentAccessor nca;

		private float enterTime;

		private float initTime;

		private SmoothFloat fade = new SmoothFloat(1f);

		private SmoothFloat fadeIn = new SmoothFloat(1f);

		private List<Material> targetMaterialList = new List<Material>();

		private static readonly int ClippingLevel = Shader.PropertyToID("_Tweak_transparency");

		public void Initialize(NpcComponentAccessor nca)
		{
			initTime = UnityEngine.Time.time;
			this.nca = nca;
			nca.AvatarComponent.AddTargetRenderer(targetMaterialList);
			fadeIn.Current = -1f;
			fadeIn.Target = 1f;
			foreach (Material material in targetMaterialList)
			    material.SetFloat(ClippingLevel, fadeIn.Current);
		}

		public void OnEnter()
		{
			// Original RVA 0x43B580: scaled time, fade reset, agent disable, endpoint sounds.
			enterTime = Time.time;
			fade.Current = 0f;
			fade.Target = -1f;
			nca.NavAgent.enabled = false;
			if (nca.StateModel.RouteData.GoalPoint.Type == global::ExposureUnnoticed2.Object3D.RoutePoint.RoutePointType.Door)
			    nca.NpcSoundController.PlaySe(global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType.CloseDoor);
			if (nca.StateModel.RouteData.GoalPoint.Type == global::ExposureUnnoticed2.Object3D.RoutePoint.RoutePointType.GlassDoor)
			    nca.NpcSoundController.PlaySe(global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType.ShopDoor);
		}

		public void OnUpdate()
		{
			// Original RVA 0x43B710: strict > 1 second, deferred removal before IsDestroy.
			if (Time.time - enterTime > FadeTime)
			{
			    NpcManager.Instance.RegisterDestroy(nca.NpcController);
			    nca.StateModel.IsDestroy = true;
			    return;
			}
			fade.UpdateToTarget();
			bool first = true;
			foreach (Material material in targetMaterialList)
			{
			    float value = fade.Current;
			    if (first) value = Mathf.Clamp(value + 0.5f, -1f, 1f);
			    material.SetFloat(ClippingLevel, value);
			    first = false;
			}
		}

		public void UpdateFadeIn()
		{
			if (1f <= fadeIn.Current)
			{
			    nca.StateModel.IsSkipUpdateStrangeness = false;
			    return;
			}
			fadeIn.UpdateToTarget();
			foreach (Material material in targetMaterialList)
			    material.SetFloat(ClippingLevel, fadeIn.Current);
		}
	}
}
