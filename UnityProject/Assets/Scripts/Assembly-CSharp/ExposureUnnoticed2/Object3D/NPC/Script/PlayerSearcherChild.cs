using System.Collections.Generic;
using ExposureUnnoticed2.Object3D.ForeignObject;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class PlayerSearcherChild : MonoBehaviour
	{
		private bool isInArea;

		public List<ForeignObjectController> ForeignObjectList = new List<ForeignObjectController>();

		public HashSet<GameObject> ForeignObjectSet = new HashSet<GameObject>();

		private NpcComponentAccessor nca;

		public bool IsInArea => isInArea;

		public void Initialize(NpcComponentAccessor nca)
		{
			this.nca = nca;
		}

		public void DeActive()
		{
			isInArea = false;
			GameObject obj = gameObject;
			if (obj is null) throw new System.NullReferenceException();
			obj.SetActive(false);
		}

		private void OnTriggerEnter(Collider collider)
		{
			if (collider is null) throw new System.NullReferenceException();
			GameObject firstObject = collider.gameObject;
			if (firstObject == ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.GameObject)
			{
			    isInArea = true;
			    return;
			}
			GameObject layerObject = collider.gameObject;
			if (layerObject is null) throw new System.NullReferenceException();
			if (layerObject.layer != ExposureUnnoticed2.Scripts.Base.LayerAccessor.ForeignObjectLayer) return;
			System.Collections.Generic.HashSet<GameObject> looked = nca.StateModel.AlreadyLookedForeignObjectSet;
			GameObject candidateObject = collider.gameObject;
			if (looked is null) throw new System.NullReferenceException();
			if (looked.Contains(candidateObject)) return;
			System.Collections.Generic.HashSet<GameObject> existing = ForeignObjectSet;
			GameObject existingObject = collider.gameObject;
			if (existing is null) throw new System.NullReferenceException();
			if (existing.Contains(existingObject)) return;
			System.Collections.Generic.HashSet<GameObject> destination = ForeignObjectSet;
			GameObject addObject = collider.gameObject;
			if (destination is null) throw new System.NullReferenceException();
			destination.Add(addObject);
			System.Collections.Generic.List<ForeignObjectController> list = ForeignObjectList;
			GameObject componentObject = collider.gameObject;
			if (componentObject is null) throw new System.NullReferenceException();
			ForeignObjectController component = componentObject.GetComponent<ForeignObjectController>();
			if (list is null) throw new System.NullReferenceException();
			list.Add(component);
		}

		private void OnTriggerExit(Collider collider)
		{
			if (collider is null) throw new System.NullReferenceException();
			GameObject firstObject = collider.gameObject;
			if (firstObject == ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.GameObject)
			{
			    isInArea = false;
			    return;
			}
			GameObject layerObject = collider.gameObject;
			if (layerObject is null) throw new System.NullReferenceException();
			if (layerObject.layer != ExposureUnnoticed2.Scripts.Base.LayerAccessor.ForeignObjectLayer) return;
			System.Collections.Generic.HashSet<GameObject> looked = nca.StateModel.AlreadyLookedForeignObjectSet;
			GameObject candidateObject = collider.gameObject;
			if (looked is null) throw new System.NullReferenceException();
			if (looked.Contains(candidateObject)) return;
			System.Collections.Generic.HashSet<GameObject> existing = ForeignObjectSet;
			GameObject existingObject = collider.gameObject;
			if (existing is null) throw new System.NullReferenceException();
			if (!existing.Contains(existingObject)) return;
			System.Collections.Generic.HashSet<GameObject> destination = ForeignObjectSet;
			GameObject removeObject = collider.gameObject;
			if (destination is null) throw new System.NullReferenceException();
			destination.Remove(removeObject);
			System.Collections.Generic.List<ForeignObjectController> list = ForeignObjectList;
			GameObject componentObject = collider.gameObject;
			if (componentObject is null) throw new System.NullReferenceException();
			ForeignObjectController component = componentObject.GetComponent<ForeignObjectController>();
			if (list is null) throw new System.NullReferenceException();
			list.Remove(component);
		}
	}
}
