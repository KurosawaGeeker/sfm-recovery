namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class StrangenessValueManager
	{
		private const int MaxReduceCount = 120;

		private const float ReducePerSecond = 0.2f;

		private float val;

		private int strangenessReduceCount;

		public float Value
		{
			get
			{
			return val;
		}
			set
			{
			strangenessReduceCount = 120;
			if (value < 0f) val = 0f;
			else if (1f < value) val = 1f;
			else val = value;
		}
		}

		public float CurrentReduceParSecond { get; set; } = 0.2f;

		public void OnUpdate(bool isSkipReduceCount)
		{
			if (strangenessReduceCount < 0)
			{
			    float current = val;
			    float reduce = CurrentReduceParSecond;
			    float delta = UnityEngine.Time.deltaTime;
			    float next = current - delta * reduce;
			    if (next < 0f) next = 0f;
			    else if (1f < next) next = 1f;
			    val = next;
			}
			if (!isSkipReduceCount) strangenessReduceCount = unchecked(strangenessReduceCount - 1);
		}
	}
}
