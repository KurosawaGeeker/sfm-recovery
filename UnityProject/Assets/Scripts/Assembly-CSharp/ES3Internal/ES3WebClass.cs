using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace ES3Internal
{
	public class ES3WebClass
	{
		[CompilerGenerated]
		private sealed class _003CSendWebRequest_003Ed__19 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public ES3WebClass _003C_003E4__this;

			public UnityWebRequest webRequest;

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
			public _003CSendWebRequest_003Ed__19(int _003C_003E1__state)
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

		protected string url;

		protected string apiKey;

		protected List<KeyValuePair<string, string>> formData;

		protected UnityWebRequest _webRequest;

		public bool isDone;

		public string error;

		public long errorCode;

		public float uploadProgress => 0f;

		public float downloadProgress => 0f;

		public bool isError => false;

		public static bool IsNetworkError(UnityWebRequest www)
		{
			return false;
		}

		protected ES3WebClass(string url, string apiKey)
		{
		}

		public void AddPOSTField(string fieldName, string value)
		{
		}

		protected string GetUser(string user, string password)
		{
			return null;
		}

		protected WWWForm CreateWWWForm()
		{
			return null;
		}

		protected bool HandleError(UnityWebRequest webRequest, bool errorIfDataIsDownloaded)
		{
			return false;
		}

		[IteratorStateMachine(typeof(_003CSendWebRequest_003Ed__19))]
		protected IEnumerator SendWebRequest(UnityWebRequest webRequest)
		{
			return null;
		}

		protected virtual void Reset()
		{
		}
	}
}
