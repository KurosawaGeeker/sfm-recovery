using UnityEngine;

namespace FUNSET
{
	public class OutRangeCullerManager : MonoBehaviour
	{
		[Tooltip(null)]
		[SerializeField]
		[Header("Input Camera Name")]
		public string CameraName;

		[SerializeField]
		[Tooltip(null)]
		[Header("Or Set Target Camera")]
		public GameObject TargetCamera;

		[Tooltip(null)]
		[SerializeField]
		[Header("Set Culling Distance(if need)")]
		public float Distance;

		[Tooltip(null)]
		[Header("Set Culling Check Interval")]
		[SerializeField]
		public float CheckCount;

		[SerializeField]
		[Tooltip("Set the OutRangeCuller that you want to batch set. If nothing is set, the OutRangeCuller will be detected automatically from the scene.")]
		[Header("Set OutRangeCuller or Stay Blank")]
		private OutRangeCuller[] _outRangeCullers;

		public void Reset()
		{
		}

		public void Awake()
		{
		}

		public void SetTargetCameraName(string s)
		{
		}

		public void SetTargetCamera(GameObject a)
		{
		}

		public void GetOutRangeCullers()
		{
		}

		public void SetAllTargetCamera(GameObject a)
		{
		}

		public void SetAllDistance(float a)
		{
		}

		public void SetAllCheckInterval(float a)
		{
		}

		public void ClearOutRangeCullers()
		{
		}
	}
}
