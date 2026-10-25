using System;
using Common.CommonUI.CommonSettingItem;
using Common.Scripts.UI;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.OptionMenu.GraphicsOption
{
	public class GraphicsOptionPanelView : BasePanelView
	{
		[SerializeField]
		private SettingSelectItemView aspectRatio;

		[SerializeField]
		private SettingSelectItemView quality;

		[SerializeField]
		private SettingSelectItemView resolution;

		[SerializeField]
		private SettingSelectItemView screenMode;

		[SerializeField]
		private OptionItemBarView frameRate;

		[SerializeField]
		private OptionItemBarView brightness;

		[SerializeField]
		private OptionItemBarView contrast;

		[SerializeField]
		private OptionItemBarView colorTemperature;

		[SerializeField]
		private OptionItemBarView cameraAngle1st;

		[SerializeField]
		private OptionItemBarView cameraAngle3rd;

		[SerializeField]
		private OptionItemCheckView vSync;

		[SerializeField]
		private OptionItemCheckView antiAliasing;

		[SerializeField]
		private OptionItemCheckView ssao;

		[SerializeField]
		private OptionItemCheckView ssr;

		[SerializeField]
		private OptionItemCheckView bloom;

		[SerializeField]
		private OptionItemBarView bloomStrength;

		[SerializeField]
		private OptionItemBarView bloomBias;

		[SerializeField]
		private OptionItemBarView bloomBiasLight;

		[SerializeField]
		private OptionItemCheckView motionBlur;

		[SerializeField]
		private GameObject frameRateMask;

		private int initWindowModeId;

		private int initResolutionId;

		private int initAspectId;

		private bool isOpenChild;

		private string[] aspectArray;

		private string[] qualityArray;

		private string[] screenModeArray;

		private string[] resolutionArray;

		private string[] resolutionWideArray;

		private void Initialize(Action onClose)
		{
		}

		private void InitializeWindowItem()
		{
		}

		private void InitializeResolution()
		{
		}

		private void InitializeFov()
		{
		}

		public void Save()
		{
		}

		public void AllReset()
		{
		}

		private void Update()
		{
		}

		public static void Open(Transform parent, Action onClose)
		{
		}
	}
}
