using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Ricimi
{
	public class Transition : MonoBehaviour
	{
		[CompilerGenerated]
		private sealed class _003CRunFade_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public Color fadeColor;

			public Transition _003C_003E4__this;

			public float duration;

			public string level;

			private Image _003Cimage_003E5__2;

			private float _003Ctime_003E5__3;

			private float _003ChalfDuration_003E5__4;

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
			public _003CRunFade_003Ed__5(int _003C_003E1__state)
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

		private static GameObject m_canvas;

		private GameObject m_overlay;

		private void Awake()
		{
		}

		public static void LoadLevel(string level, float duration, Color color)
		{
		}

		private void StartFade(string level, float duration, Color fadeColor)
		{
		}

		[IteratorStateMachine(typeof(_003CRunFade_003Ed__5))]
		private IEnumerator RunFade(string level, float duration, Color fadeColor)
		{
			return null;
		}
	}
}
