using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.Util
{
	public class SaveDataScreenShotter
	{
		[CompilerGenerated]
		private sealed class _003CCaptureCoroutine_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public string path;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return null;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return null;
				}
			}

			[DebuggerHidden]
			public _003CCaptureCoroutine_003Ed__5(int _003C_003E1__state)
			{
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
			}
		}

		private static SaveDataScreenShotter _instance;

		private static SaveDataScreenShotter instance => null;

		public static void CaptureAndSave(MonoBehaviour coroutineParent, SaveRapper.SaveDataType type, int slot = -1)
		{
		}

		private void CaptureAndSaveInstance(MonoBehaviour coroutineParent, SaveRapper.SaveDataType type, int slot = -1)
		{
		}

		[IteratorStateMachine(typeof(_003CCaptureCoroutine_003Ed__5))]
		private IEnumerator CaptureCoroutine(string path)
		{
			return null;
		}
	}
}
