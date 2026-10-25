using System.Collections.Generic;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class InmouController : MonoBehaviour
	{
		[SerializeField]
		private GameObject[] inmouObjects;

		[SerializeField]
		private GameObject pantsObject;

		[SerializeField]
		private Vector3 aaa;

		[SerializeField]
		private float bbb;

		private List<Material> materialList = new List<Material>();

		// Recovered original .cctor RVA 0x620630: ordered field initializers preserve BeforeFieldInit.
        private static readonly int TweakTransparency = Shader.PropertyToID("_Tweak_transparency");

        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

		private bool isHideFromCosplay;

		private Vector3 initLocalPosition;

		private Quaternion initLocalRotation;

		private void Awake()
		{
			// Recovered from GameAssembly.dll RVA 0x61F550
			initLocalPosition = transform.localPosition;
			initLocalRotation = transform.localRotation;
			EventManager.AddListener<ChangeBodyEvent>(OnChangeBody, false);
			for (int i = 0; i < inmouObjects.Length; ++i)
			{
			    var renderer = inmouObjects[i].GetComponent<Renderer>();
			    renderer.material = UnityEngine.Object.Instantiate(renderer.material);
			    materialList.Add(renderer.material);
			}
			Scheduler.DelayFlame(Apply, 1);
		}

		private void OnDestroy()
		{
			// Recovered from GameAssembly.dll RVA 0x61FA80
			EventManager.RemoveListener<ChangeBodyEvent>(OnChangeBody);
		}

		private void Apply()
		{
			// Recovered from GameAssembly.dll RVA 0x61FB40
			bool show = !isHideFromCosplay;
			inmouObjects[0].SetActive(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmou1 > 0 && show);
			inmouObjects[1].SetActive(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmou1 > 1 && show);
			inmouObjects[2].SetActive(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmou1 > 2 && show);
			inmouObjects[3].SetActive(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmou1 > 3 && show);
			inmouObjects[4].SetActive(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmou1 > 3 && show);
			float amount = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeWaist * 0.01f;
			transform.localPosition = Vector3.Lerp(initLocalPosition, aaa, amount);
			transform.localRotation = Quaternion.Lerp(initLocalRotation, Quaternion.Euler(bbb, 0f, 0f), amount);
		}

		private void OnChangeBody(ChangeBodyEvent evt)
		{
			// Recovered from GameAssembly.dll RVA 0x620190
			foreach (var material in materialList)
			{
			    float transparency = ((global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmou2 / 50f - 1f) / 50f) * 15f;
			    material.SetFloat(TweakTransparency, transparency);
			    float red = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmouColorR / 255f;
			    int colorProperty = BaseColor;
			    var otherComponents = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData;
			    material.SetColor(colorProperty, new Color(red, otherComponents.CustomizeInmouColorG / 255f, otherComponents.CustomizeInmouColorB / 255f, 1f));
			}
			Apply();
		}

		public void SetHide(bool isHide)
		{
			// Recovered from GameAssembly.dll RVA 0x620520
			isHideFromCosplay = isHide;
			Apply();
		}
	}
}
