using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Other
{
	public class FootHeelAdjuster : MonoBehaviour
	{
		[SerializeField]
		private int heelIndex = -1;

		[SerializeField]
		private bool isInverse;

		private SkinnedMeshRenderer mesh;

		private void Awake()
		{
			global::ExposureUnnoticed2.Scripts.Base.EventManager.AddListener<global::ExposureUnnoticed2.Scripts.Base.ChangeShoesEvent>(OnChangeShoes, false);
			mesh = GetComponent<SkinnedMeshRenderer>();
		}

		private void OnDestroy()
		{
			global::ExposureUnnoticed2.Scripts.Base.EventManager.RemoveListener<global::ExposureUnnoticed2.Scripts.Base.ChangeShoesEvent>(OnChangeShoes);
		}

		private void OnEnable()
		{
			int weight;
			if (global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CurrentShoesRecord == null
			    || global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CurrentShoesRecord.IsNotHeelToe) weight = 100;
			else weight = 0;
			if (isInverse) weight = unchecked(100 - weight);
			if (global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CurrentShoesRecord != null && !isInverse)
			    weight = unchecked((int)(global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CurrentShoesRecord.HeelToeShapeScale * (float)weight));
			if (heelIndex != -1) mesh.SetBlendShapeWeight(heelIndex, (float)weight);
		}

		private void OnChangeShoes(ChangeShoesEvent evt)
		{
			int weight;
			if (global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CurrentShoesRecord == null
			    || global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CurrentShoesRecord.IsNotHeelToe) weight = 100;
			else weight = 0;
			if (isInverse) weight = unchecked(100 - weight);
			if (global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CurrentShoesRecord != null && !isInverse)
			    weight = unchecked((int)(global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CurrentShoesRecord.HeelToeShapeScale * (float)weight));
			if (heelIndex != -1) mesh.SetBlendShapeWeight(heelIndex, (float)weight);
		}
	}
}
