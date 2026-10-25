using UnityEngine;

namespace AkilliMum.Standard.Mirror
{
	public abstract class ManagerBase
	{
		private bool _isDirty = true;

		public bool IsDirty
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public abstract void Destroy();

		public void DestroyObject(Object obj)
		{
		}
	}
}
