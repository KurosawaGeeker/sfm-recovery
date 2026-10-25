namespace ExposureUnnoticed2.Scripts.Base
{
	public class SmoothFloat
	{
		private float maxDelta;

		private float downMaxDelta;

		private bool isLerp;

		private float baseValue;

		private float second;

		private float downSecond;

		private bool isUnscaled;

		public float Current { get; set; }

		public float Target { get; set; }

		public SmoothFloat(float second, float baseValue = 1f, float defaultValue = 0f, bool isLerp = false, bool isUnscaled = false, float downSecond = -1f)
		{
			// Recovered from GameAssembly.dll RVA 0x4B5410
			this.isLerp = isLerp;
			this.baseValue = baseValue;
			this.second = second;
			this.downSecond = System.Math.Abs(downSecond - -1f) < 0.001f ? second : downSecond;
			this.isUnscaled = isUnscaled;
			maxDelta = baseValue / second;
			downMaxDelta = baseValue / this.downSecond;
			Current = defaultValue;
		}

		public void UpdateTo(float target)
		{
			// Recovered from GameAssembly.dll RVA 0x4B54F0
			float current = Current;
			float rate = maxDelta;
			if (isLerp)
			{
			    if (rate < 0f) rate = 0f;
			    else if (rate > 1f) rate = 1f;
			    Current = current + (target - current) * rate;
			    return;
			}
			if (current > target) rate = downMaxDelta;
			float maxStep = (isUnscaled ? UnityEngine.Time.unscaledDeltaTime : UnityEngine.Time.deltaTime) * rate;
			float difference = target - current;
			if (!(maxStep >= System.Math.Abs(difference)))
			    target = current + (difference >= 0f ? 1f : -1f) * maxStep;
			Current = target;
		}

		public void SetBaseValue(float baseValue)
		{
			// Recovered from GameAssembly.dll RVA 0x4B5660
			this.baseValue = baseValue;
			maxDelta = baseValue / second;
			downMaxDelta = baseValue / downSecond;
		}

		public void SetSecond(float second)
		{
			// Recovered from GameAssembly.dll RVA 0x4B5680
			if (second == 0f) second = 0.0001f;
			this.second = second;
			maxDelta = baseValue / second;
			if (System.Math.Abs(downSecond - -1f) < 0.001f) downSecond = second;
			downMaxDelta = baseValue / downSecond;
		}

		public void UpdateToTarget()
		{
			// Recovered from GameAssembly.dll RVA 0x4B5740
			UpdateTo(Target);
		}
	}
}
