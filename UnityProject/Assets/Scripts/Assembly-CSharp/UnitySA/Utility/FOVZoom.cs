using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnitySA.Utility
{
	[Serializable]
	public class FOVZoom
	{
		[CompilerGenerated]
		private sealed class _003CFOVKickDown_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public FOVZoom _003C_003E4__this;

			private float _003Ct_003E5__2;

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
			public _003CFOVKickDown_003Ed__10(int _003C_003E1__state)
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

		[CompilerGenerated]
		private sealed class _003CFOVKickUp_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public FOVZoom _003C_003E4__this;

			private float _003Ct_003E5__2;

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
			public _003CFOVKickUp_003Ed__9(int _003C_003E1__state)
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

		public Camera Camera;

		[HideInInspector]
		public float originalFov;

		public float FOVIncrease;

		public float TimeToIncrease;

		public float TimeToDecrease;

		public AnimationCurve IncreaseCurve;

		public void Setup(Camera camera)
		{
		}

		private void CheckStatus(Camera camera)
		{
		}

		public void ChangeCamera(Camera camera)
		{
		}

		[IteratorStateMachine(typeof(_003CFOVKickUp_003Ed__9))]
		public IEnumerator FOVKickUp()
		{
			return null;
		}

		[IteratorStateMachine(typeof(_003CFOVKickDown_003Ed__10))]
		public IEnumerator FOVKickDown()
		{
			return null;
		}
	}
}
