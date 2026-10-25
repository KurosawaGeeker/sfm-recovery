using System.Collections.Generic;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerBoneScaleAdjuster : MonoBehaviour
	{
		private Transform target;

		private List<Transform> childList;

		public void Initialize(Transform target)
		{
			// Recovered from GameAssembly.dll RVA 0x639070
			this.target = target;
			global::ExposureUnnoticed2.Scripts.Base.EventManager.AddListener<global::ExposureUnnoticed2.Scripts.Base.ChangeBodyEvent>(OnChangeBody, false);
			childList = new List<Transform>();
			foreach (Transform child in target) childList.Add(child);
			transform.parent = target;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			ChangeParent();
		}

		private void OnDestroy()
		{
			// Recovered from GameAssembly.dll RVA 0x639780
			global::ExposureUnnoticed2.Scripts.Base.EventManager.RemoveListener<global::ExposureUnnoticed2.Scripts.Base.ChangeBodyEvent>(OnChangeBody);
		}

		private void OnChangeBody(ChangeBodyEvent evt)
		{
			// Recovered from GameAssembly.dll RVA 0x639840
			global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayFlame(OnRecoveredBodyChange, 1);
		}

		public void ChangeParent()
		{
			// Recovered from GameAssembly.dll RVA 0x6398C0
			foreach (Transform child in childList) child.parent = transform;
		}

		public void UndoParent()
		{
			// Recovered from GameAssembly.dll RVA 0x639A60
			foreach (Transform child in childList) child.parent = target;
		}
	// Recovered original member RVA 0x639BC0: <OnChangeBody>b__4_0
		private void OnRecoveredBodyChange()
        {
            Transform own = transform;
            Vector3 one = Vector3.one;
            Vector3 scale = target.localScale;
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
            own.localScale = new Vector3(one.x / scale.x, one.y / scale.y, one.z / scale.z);
        }
	}
}
