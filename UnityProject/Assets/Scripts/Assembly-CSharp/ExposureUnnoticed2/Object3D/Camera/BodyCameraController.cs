using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Scripts.InGame;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Camera
{
	public class BodyCameraController : MonoBehaviour
	{
		[SerializeField]
		private Vector3 smallOffset;

		[SerializeField]
		private Vector3 bigOffset;

		[SerializeField]
		private Vector3 futomomoOffset;

		[SerializeField]
		private float a;

		[SerializeField]
		private float b;

		[SerializeField]
		private float c;

		[SerializeField]
		private float d;

		[SerializeField]
		private float e;

		private Vector3 initPosition;

		private void Awake()
		{
            // Original RVA 0x45BF60: no eager adjustment before player initialization.
            initPosition = transform.localPosition;
            EventManager.AddListener<ChangeBodyEvent>(OnChangeBody, false);
            EventManager.AddListener<ChangeCostumeCosplayEvent>(OnChangeCostumeCosplayEvent, false);
		}

		private void OnDestroy()
		{
            // Original RVA 0x45C140, matching registration order.
            EventManager.RemoveListener<ChangeBodyEvent>(OnChangeBody);
            EventManager.RemoveListener<ChangeCostumeCosplayEvent>(OnChangeCostumeCosplayEvent);
		}

		private void Update()
		{
		}

		private void OnChangeBody(ChangeBodyEvent evt)
		{
            // Original 0x45C260 is a tail jump to A at 0x45C2F0.
            A();
		}

		private void OnChangeCostumeCosplayEvent(ChangeCostumeCosplayEvent evt)
		{
            // Original RVA 0x45C270 defers its generated A() callback by one frame.
            Scheduler.DelayFlame(() => A(), 1);
		}

		private void A()
		{
            // Original RVA 0x45C2F0, including original coefficients and clamped height interpolation.
            float height = (float)GameState.PlayerCustomData.CustomizeHeight / 160f;
            float neck = Mathf.Clamp01((float)unchecked(GameState.PlayerCustomData.CustomizeNeck - 50) / 50f);
            float thighs = Mathf.Clamp01((float)unchecked(GameState.PlayerCustomData.CustomizeThighsLength - 50) / 50f);
            float lowerLeg = Mathf.Clamp01((float)unchecked(GameState.PlayerCustomData.CustomizeLowerLegLength - 50) / 50f);
            float upperBody = Mathf.Clamp01((float)unchecked(GameState.PlayerCustomData.CustomizeUpperBodyLength - 50) / 50f);
            float amount = thighs * b + neck * a + lowerLeg * c + upperBody * d
                + PlayerController.Instance.Pca.PlayerToeHeelController.HeelOffset * e;
            Vector3 heightOffset = height < 1f
                ? Vector3.Lerp(Vector3.zero, smallOffset, 1f - height)
                : Vector3.Lerp(Vector3.zero, bigOffset, height - 1f);
            transform.localPosition = futomomoOffset * amount + heightOffset + initPosition;
		}
	}
}
