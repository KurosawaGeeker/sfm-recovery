using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Scripts.Base;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InGame.HeatBeatPanel
{
	public class HeartRatePanelView : MonoBehaviour
	{
		private const float UpdateBeatInterval = 1.5f;

		[SerializeField]
		private CanvasGroup vignetteCanvas;

		private SmoothFloat audioVolumeScale;

		private TextMeshProUGUI heartRateText;

		private float lastUpdateTime;

		private Animator animator;

		private AudioSource audioSource;

		private static readonly int BeatRate = Animator.StringToHash("BeatRate");

		private static readonly int BeatRateNijo = Animator.StringToHash("BeatRateNijo");

		private static readonly int BeatRateSanjo = Animator.StringToHash("BeatRateSanjo");

		private float[] randoms;

		private int randomIndex;

		private PlayerHeartBeat playerHeartBeat;

		private float oldAnimatorSpeed;

		private static readonly int NoVignette = Animator.StringToHash("NoVignette");

		public void Initialize(PlayerHeartBeat playerHeartBeat)
		{
			// Recovered from GameAssembly.dll RVA 0x50CCC0
			this.playerHeartBeat = playerHeartBeat;
			lastUpdateTime = -999f;
			heartRateText = GetComponentInChildren<TextMeshProUGUI>();
			animator = GetComponent<Animator>();
			audioSource = GetComponent<AudioSource>();
			audioVolumeScale.Current = 1f;
			audioVolumeScale.Target = 1f;
			EventManager.AddListener<OptionChangeEvent>(OnChangeOption, false);
			// Original generated no-argument callback shares RVA 0x50D940 with OnChangeOption.
			Scheduler.DelayFlame(() => OnChangeOption(null), 1);
		}

		private void OnDestroy()
		{
			// Recovered from GameAssembly.dll RVA 0x50CFB0
			EventManager.RemoveListener<OptionChangeEvent>(OnChangeOption);
		}

		public void OnLateUpdate()
		{
            if (!global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.IsGameOver)
            {
                if (oldAnimatorSpeed >= 0f)
                {
                    animator.speed = oldAnimatorSpeed;
                    oldAnimatorSpeed = -1f;
                }
                float rate = (playerHeartBeat.HeartRate - 95f) / 57f;
                animator.SetFloat(BeatRate, rate);
                animator.SetFloat(BeatRateNijo, rate * rate);
                animator.SetFloat(BeatRateSanjo, rate * rate * rate);
                animator.speed = rate + rate + 1f;
                audioVolumeScale.Target = global::ExposureUnnoticed2.ObjectUI.SleepBlackPanel.SleepBlackPanelView.IsOpen ? 0f : 1f;
                audioVolumeScale.UpdateTo(audioVolumeScale.Target);
                float volume = rate * audioVolumeScale.Current;
                volume *= (float)global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.SoundHeartBeat100;
                audioSource.volume = volume * 0.01f;
                if (Time.time - lastUpdateTime < 1.5f) return;
                lastUpdateTime = Time.time;
                int index = randomIndex;
                var oldRandoms = randoms;
                randomIndex = unchecked(index + 1);
                if (oldRandoms is null) throw new global::System.NullReferenceException();
                float noise = UnityEngine.Random.value;
                oldRandoms[index % oldRandoms.Length] = noise - 0.5f;
                var currentRandoms = randoms;
                float sum = 0f;
                for (int i = 0; i < currentRandoms.Length; i++) sum += currentRandoms[i];
                if (!heartRateText.gameObject.activeSelf) return;
                int value = unchecked((int)(sum * 1.5f + playerHeartBeat.HeartRate));
                heartRateText.text = value.ToString();
                Color middle = new Color(1f, 0.92156863f, 0.015686275f, 1f);
                if (rate > 0.5f) heartRateText.color = Color.Lerp(middle, Color.red, (rate - 0.5f) + (rate - 0.5f));
                else heartRateText.color = Color.Lerp(Color.white, middle, rate + rate);
            }
            else
            {
                if (!(oldAnimatorSpeed <= -1f)) return;
                oldAnimatorSpeed = animator.speed;
                Scheduler.DelayUnscaledTime(() => animator.speed = 0f, 0.25f);
                animator.SetBool(NoVignette, true);
            }
		}

		private void UpdateRandom()
		{
		}

		private float GetRandomSum()
		{
			return 0f;
		}

		public void PlayHeatBeat(int id)
		{
		}

		private void OnChangeOption(OptionChangeEvent evt)
		{
			// Recovered from GameAssembly.dll RVA 0x50D940
			vignetteCanvas.alpha = global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.HeartBeatEffectAlpha100 * 0.01f;
		}
	// Recovered original member RVA 0x50DA00: .ctor
		public HeartRatePanelView()
        {
            audioVolumeScale = new SmoothFloat(0.2f, 1f, 0f, false, false, -1f);
            randoms = new float[3];
        }
	// Recovered original member RVA 0x50DB80: .cctor
		// Recovered original .cctor RVA 0x50DB80: ordered field initializers preserve BeforeFieldInit.
	}
}
