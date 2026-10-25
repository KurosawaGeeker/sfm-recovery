using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Costume
{
	public class CostumeShrinkApplyer : MonoBehaviour
	{
		[SerializeField]
		private int indexToeFoot = -1;

		[SerializeField]
		private int indexAnkleLowerFootKnee = -1;

		private SkinnedMeshRenderer mesh;

		private void Awake()
		{
			mesh = GetComponent<SkinnedMeshRenderer>();
			global::ExposureUnnoticed2.Scripts.Base.EventManager.AddListener<global::ExposureUnnoticed2.Scripts.Base.UpdateBodyShrinkEvent>(OnChange, false);
		}

		private void OnDestroy()
		{
			global::ExposureUnnoticed2.Scripts.Base.EventManager.RemoveListener<global::ExposureUnnoticed2.Scripts.Base.UpdateBodyShrinkEvent>(OnChange);
		}

		private void OnEnable()
		{
			var cache = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerBodyCustomizeAdjuster.shrinkCacheDict;
			if (indexToeFoot != -1)
			    mesh.SetBlendShapeWeight(indexToeFoot, cache[global::ExposureUnnoticed2.Master.Cosplay.ShrinkType.Toe] || cache[global::ExposureUnnoticed2.Master.Cosplay.ShrinkType.Foot] ? 100f : 0f);
			if (indexAnkleLowerFootKnee != -1)
			    mesh.SetBlendShapeWeight(indexAnkleLowerFootKnee, cache[global::ExposureUnnoticed2.Master.Cosplay.ShrinkType.Ankle] || cache[global::ExposureUnnoticed2.Master.Cosplay.ShrinkType.LowerLeg] || cache[global::ExposureUnnoticed2.Master.Cosplay.ShrinkType.Knee] ? 100f : 0f);
		}

		private void OnChange(UpdateBodyShrinkEvent evt)
		{
			var cache = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerBodyCustomizeAdjuster.shrinkCacheDict;
			if (indexToeFoot != -1)
			    mesh.SetBlendShapeWeight(indexToeFoot, cache[global::ExposureUnnoticed2.Master.Cosplay.ShrinkType.Toe] || cache[global::ExposureUnnoticed2.Master.Cosplay.ShrinkType.Foot] ? 100f : 0f);
			if (indexAnkleLowerFootKnee != -1)
			    mesh.SetBlendShapeWeight(indexAnkleLowerFootKnee, cache[global::ExposureUnnoticed2.Master.Cosplay.ShrinkType.Ankle] || cache[global::ExposureUnnoticed2.Master.Cosplay.ShrinkType.LowerLeg] || cache[global::ExposureUnnoticed2.Master.Cosplay.ShrinkType.Knee] ? 100f : 0f);
		}
	}
}
