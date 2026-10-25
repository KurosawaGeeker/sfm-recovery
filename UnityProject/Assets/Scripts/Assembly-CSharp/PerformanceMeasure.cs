using UnityEngine;

public class PerformanceMeasure : MonoBehaviour
{
	private static PerformanceMeasure Instance;

	private const float MeasureInterval = 1f;

	private const int AveNum = 10;

	private float nextTime;

	private int frameCount;

	private int measureCount;

	private float fps;

	private float aveFps;

	private readonly float[] fpsHistory;

	[SerializeField]
	private float x;

	[SerializeField]
	private float y;

	[SerializeField]
	private float w;

	[SerializeField]
	private float h;

	[SerializeField]
	private float i;

	private void Update()
	{
	}

	private void OnGUI()
	{
	}
}
