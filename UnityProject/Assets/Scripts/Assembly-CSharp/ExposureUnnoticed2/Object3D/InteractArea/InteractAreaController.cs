using System;
using System.Collections.Generic;
using ExposureUnnoticed2.ObjectUI.InteractMenuPanel;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.InGame;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.InteractArea
{
	public class InteractAreaController : MonoBehaviour
	{
		[SerializeField]
		private InteractType type;

		[SerializeField]
		private GameObject ringEffect;

		private bool isInArea;

		private Action onInteract;

		public Dictionary<int, InteractMenuItemModel> MenuAction = new Dictionary<int, InteractMenuItemModel>();

		private string overrideInteractPanelText;

		private bool isInteracting;

		private Vector3 position;

		private bool isOldActive;

		private bool disposed;

		private int skip;

		[field: SerializeField]
		public bool CanInteractWithAction { get; private set; }

		public InteractType Type => type;

		public Vector3 VectorParam { get; set; }

		public int IntParam { get; set; }

		public bool BoolParam { get; set; }

		public DroppedItemManager.DropItemType DropItemType { get; set; }

		public void Initialize(Action onInteract, string overrideText = null)
		{
			// Recovered from GameAssembly.dll RVA 0x453090
			this.onInteract = onInteract;
			overrideInteractPanelText = overrideText;
			position = transform.position;
			EventManager.AddListener<InteractEvent>(OnInteract, false);
			EventManager.AddListener<OptionChangeEvent>(OnChangeOption, false);
			bool active = global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData is null || !global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.InteractEffectInvisible;
			ringEffect.SetActive(active);
		}

		private void OnDestroy()
		{
			// Recovered from GameAssembly.dll RVA 0x4533C0
			Dispose();
		}

		public void Dispose()
		{
			// Recovered from GameAssembly.dll RVA 0x4533D0
			if (disposed) return;
			disposed = true;
			EventManager.RemoveListener<InteractEvent>(OnInteract);
			EventManager.RemoveListener<OptionChangeEvent>(OnChangeOption);
		}

		private void OnTriggerEnter(Collider collider)
		{
			// Recovered from GameAssembly.dll RVA 0x453500
			if (isInArea) return;
			if (collider.gameObject != global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.GameObject) return;
			InteractType capturedType = type;
			isInArea = true;
			InteractManager.Register(capturedType, this);
		}

		private void OnTriggerExit(Collider collider)
		{
			// Recovered from GameAssembly.dll RVA 0x4537C0
			if (!isInArea) return;
			if (collider.gameObject != global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.GameObject) return;
			InteractType capturedType = type;
			isInArea = false;
			InteractManager.Unregister(capturedType, this);
		}

		private void OnDisable()
		{
			// Recovered from GameAssembly.dll RVA 0x4539B0
			if (!isInArea) return;
			InteractType capturedType = type;
			isInArea = false;
			InteractManager.Unregister(capturedType, this);
		}

		private void OnInteract(InteractEvent evt)
		{
			// Recovered from GameAssembly.dll RVA 0x453A20
			if (evt.Model.Controller != this) return;
			if (evt.Model.Type != InteractType.WasherMachine)
			{
			    // Original facade reference is validated even though no facade method is called.
			    if (global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance is null) throw new NullReferenceException();
			    if ((int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentHoldClothes != -1) return;
			}
			onInteract?.Invoke();
			isInteracting = true;
			ringEffect.SetActive(false);
		}

		private void OnChangeOption(OptionChangeEvent evt)
		{
			// Recovered from GameAssembly.dll RVA 0x453C50
			bool active = global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData is null || !global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.InteractEffectInvisible;
			ringEffect.SetActive(active);
		}

		private bool IsActiveRingEffect()
		{
			// Recovered from GameAssembly.dll RVA 0x453D10
			bool optionAllows = global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData is null || !global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.InteractEffectInvisible;
			bool near = true;
			if (global::ExposureUnnoticed2.Scripts.InGame.VisibilityFogController.Instance is not null && global::ExposureUnnoticed2.Scripts.InGame.VisibilityFogController.Instance.IsBadVisibility)
			{
			    var difference = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CacheForNpcPosition - position;
			    float squaredDistance = difference.y * difference.y + difference.x * difference.x + difference.z * difference.z;
			    near = !(squaredDistance > 16f);
			}
			bool stateAllows = optionAllows && !isInteracting;
			return near & stateAllows;
		}

		private void Update()
		{
			// Recovered from GameAssembly.dll RVA 0x453EC0
			if (!isInteracting) return;
			if (InteractMenuPanelView.Instance != null)
			{
			    skip = 2;
			    return;
			}
			if (((int)global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CurrentAction.Type == -1 || CanInteractWithAction) && skip < 1)
			{
			    isInteracting = false;
			    bool active = !global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.InteractEffectInvisible;
			    ringEffect.SetActive(active);
			}
			skip = unchecked(skip - 1);
		}

		public string InteractPanelText()
		{
			// Recovered from GameAssembly.dll RVA 0x454080
			if (!string.IsNullOrEmpty(overrideInteractPanelText)) return overrideInteractPanelText;
			return (int)type switch
			{
			    0 => null,
			    1 => "服を選ぶ",
			    2 => "ui_interact_curtain",
			    3 => "カゴ",
			    4 => "ui_interact_coat",
			    5 => "話しかける",
			    7 => "話しかける",
			    6 => "グッズを選ぶ",
			    8 => "髪型を変更する",
			    9 => "ui_interact_washer",
			    10 => "ui_interact_buymachine",
			    11 => "ui_interact_closet",
			    12 => "パンツを履く",
			    13 => "寝る",
			    14 => "ui_interact_pc",
			    17 => "ui_interact_sit",
			    18 => "ui_interact_push",
			    19 => "ui_interact_dildo",
			    20 => "ui_interact_door",
			    21 => "ui_interact_toilet",
			    22 => "ui_interact_pinpon",
			    23 => "ui_interact_put_goods",
			    24 => "ui_interact_take_goods",
			    25 => "ui_interact_sexNpc",
			    _ => throw new ArgumentOutOfRangeException()
			};
		}
	}
}
