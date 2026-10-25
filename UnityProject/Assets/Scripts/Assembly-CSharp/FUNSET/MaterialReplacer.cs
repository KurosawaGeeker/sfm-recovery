using System.Collections.Generic;
using UnityEngine;

namespace FUNSET
{
	public class MaterialReplacer : MonoBehaviour
	{
		[SerializeField]
		[Header("TargetObjectsList")]
		[Tooltip(null)]
		public List<GameObject> TargetObjectsList;

		[SerializeField]
		[Header("Target material")]
		public Material[] TargetMatList;

		public int TargetNum;

		[SerializeField]
		[Header("Replace Material")]
		public Material[] ReplaceMatList;

		public int ReplaceNum;

		[SerializeField]
		[Header("Auto exe at Start()")]
		[Tooltip(null)]
		private bool OnStart;

		public void Awake()
		{
		}

		public void Start()
		{
		}

		public void ExeMaterialReplace()
		{
		}

		public void ChangeTargetMat(int a)
		{
		}

		public void ChangeReplaceMat(int a)
		{
		}
	}
}
