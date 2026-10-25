using System;
using UnityEngine;
using UnityEngine.Audio;

namespace ExposureUnnoticed2.ObjectUI.OptionMenu
{
	public class OptionSoundMenuView : MonoBehaviour, IOptionMiddlePanel
	{
		[SerializeField]
		private OptionItemBarView master;

		[SerializeField]
		private OptionItemBarView se;

		[SerializeField]
		private OptionItemBarView ui;

		[SerializeField]
		private OptionItemBarView bgm;

		[SerializeField]
		private OptionItemBarView vibe;

		[SerializeField]
		private OptionItemBarView heartBeat;

		[SerializeField]
		private AudioMixer audioMixer;

		private Animator animator;

		private Action onClose;

		private Action<OptionItemType> onClickItem;

		public void Initialize(Action onClose, Action<OptionItemType> onClickItem)
		{
		}

		public static float ConvertVolume(float sliderValue)
		{
			return 0f;
		}

		public void AllReset()
		{
		}

		private void Update()
		{
		}

		public void Close()
		{
		}

		public void OnAnimCloseEnd()
		{
		}
	}
}
