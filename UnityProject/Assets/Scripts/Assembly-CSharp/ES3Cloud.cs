using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using ES3Internal;
using Unity.VisualScripting;
using UnityEngine.Networking;

[IncludeInSettings(true)]
public class ES3Cloud : ES3WebClass
{
	[CompilerGenerated]
	private sealed class _003CDeleteFile_003Ed__53 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ES3Cloud _003C_003E4__this;

		public ES3Settings settings;

		public string user;

		public string password;

		private UnityWebRequest _003CwebRequest_003E5__2;

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
		public _003CDeleteFile_003Ed__53(int _003C_003E1__state)
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

		private void _003C_003Em__Finally1()
		{
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003CDownloadFile_003Ed__44 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ES3Cloud _003C_003E4__this;

		public ES3File es3File;

		public string user;

		public string password;

		public long timestamp;

		private UnityWebRequest _003CwebRequest_003E5__2;

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
		public _003CDownloadFile_003Ed__44(int _003C_003E1__state)
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

		private void _003C_003Em__Finally1()
		{
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003CDownloadFile_003Ed__45 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ES3Cloud _003C_003E4__this;

		public ES3Settings settings;

		public string user;

		public string password;

		public long timestamp;

		private UnityWebRequest _003CwebRequest_003E5__2;

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
		public _003CDownloadFile_003Ed__45(int _003C_003E1__state)
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

		private void _003C_003Em__Finally1()
		{
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003CDownloadFilenames_003Ed__61 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ES3Cloud _003C_003E4__this;

		public string user;

		public string password;

		private UnityWebRequest _003CwebRequest_003E5__2;

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
		public _003CDownloadFilenames_003Ed__61(int _003C_003E1__state)
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

		private void _003C_003Em__Finally1()
		{
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003CDownloadTimestamp_003Ed__70 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ES3Cloud _003C_003E4__this;

		public ES3Settings settings;

		public string user;

		public string password;

		private UnityWebRequest _003CwebRequest_003E5__2;

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
		public _003CDownloadTimestamp_003Ed__70(int _003C_003E1__state)
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

		private void _003C_003Em__Finally1()
		{
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003CRenameFile_003Ed__60 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ES3Cloud _003C_003E4__this;

		public ES3Settings settings;

		public ES3Settings newSettings;

		public string user;

		public string password;

		private UnityWebRequest _003CwebRequest_003E5__2;

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
		public _003CRenameFile_003Ed__60(int _003C_003E1__state)
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

		private void _003C_003Em__Finally1()
		{
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003CSearchFilenames_003Ed__62 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ES3Cloud _003C_003E4__this;

		public string user;

		public string password;

		public string searchPattern;

		private UnityWebRequest _003CwebRequest_003E5__2;

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
		public _003CSearchFilenames_003Ed__62(int _003C_003E1__state)
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

		private void _003C_003Em__Finally1()
		{
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003CSync_003Ed__20 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ES3Cloud _003C_003E4__this;

		public ES3Settings settings;

		public string user;

		public string password;

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
		public _003CSync_003Ed__20(int _003C_003E1__state)
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
	private sealed class _003CUploadFile_003Ed__33 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ES3Cloud _003C_003E4__this;

		public ES3Settings settings;

		public long fileTimestamp;

		public string user;

		public string password;

		public byte[] bytes;

		private UnityWebRequest _003CwebRequest_003E5__2;

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
		public _003CUploadFile_003Ed__33(int _003C_003E1__state)
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

		private void _003C_003Em__Finally1()
		{
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	private int timeout;

	public Encoding encoding;

	private byte[] _data;

	public byte[] data => null;

	public string text => null;

	public string[] filenames => null;

	public DateTime timestamp => default;

	public ES3Cloud(string url, string apiKey)
		: base(null, null)
	{
	}

	public ES3Cloud(string url, string apiKey, int timeout)
		: base(null, null)
	{
	}

	public IEnumerator Sync()
	{
		return null;
	}

	public IEnumerator Sync(string filePath)
	{
		return null;
	}

	public IEnumerator Sync(string filePath, string user)
	{
		return null;
	}

	public IEnumerator Sync(string filePath, string user, string password)
	{
		return null;
	}

	public IEnumerator Sync(string filePath, ES3Settings settings)
	{
		return null;
	}

	public IEnumerator Sync(string filePath, string user, ES3Settings settings)
	{
		return null;
	}

	public IEnumerator Sync(string filePath, string user, string password, ES3Settings settings)
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CSync_003Ed__20))]
	private IEnumerator Sync(ES3Settings settings, string user, string password)
	{
		return null;
	}

	public IEnumerator UploadFile()
	{
		return null;
	}

	public IEnumerator UploadFile(string filePath)
	{
		return null;
	}

	public IEnumerator UploadFile(string filePath, string user)
	{
		return null;
	}

	public IEnumerator UploadFile(string filePath, string user, string password)
	{
		return null;
	}

	public IEnumerator UploadFile(string filePath, ES3Settings settings)
	{
		return null;
	}

	public IEnumerator UploadFile(string filePath, string user, ES3Settings settings)
	{
		return null;
	}

	public IEnumerator UploadFile(string filePath, string user, string password, ES3Settings settings)
	{
		return null;
	}

	public IEnumerator UploadFile(ES3File es3File)
	{
		return null;
	}

	public IEnumerator UploadFile(ES3File es3File, string user)
	{
		return null;
	}

	public IEnumerator UploadFile(ES3File es3File, string user, string password)
	{
		return null;
	}

	public IEnumerator UploadFile(ES3Settings settings, string user, string password)
	{
		return null;
	}

	public IEnumerator UploadFile(byte[] bytes, ES3Settings settings, string user, string password)
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CUploadFile_003Ed__33))]
	private IEnumerator UploadFile(byte[] bytes, ES3Settings settings, string user, string password, long fileTimestamp)
	{
		return null;
	}

	public IEnumerator DownloadFile()
	{
		return null;
	}

	public IEnumerator DownloadFile(string filePath)
	{
		return null;
	}

	public IEnumerator DownloadFile(string filePath, string user)
	{
		return null;
	}

	public IEnumerator DownloadFile(string filePath, string user, string password)
	{
		return null;
	}

	public IEnumerator DownloadFile(string filePath, ES3Settings settings)
	{
		return null;
	}

	public IEnumerator DownloadFile(string filePath, string user, ES3Settings settings)
	{
		return null;
	}

	public IEnumerator DownloadFile(string filePath, string user, string password, ES3Settings settings)
	{
		return null;
	}

	public IEnumerator DownloadFile(ES3File es3File)
	{
		return null;
	}

	public IEnumerator DownloadFile(ES3File es3File, string user)
	{
		return null;
	}

	public IEnumerator DownloadFile(ES3File es3File, string user, string password)
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CDownloadFile_003Ed__44))]
	private IEnumerator DownloadFile(ES3File es3File, string user, string password, long timestamp)
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CDownloadFile_003Ed__45))]
	private IEnumerator DownloadFile(ES3Settings settings, string user, string password, long timestamp)
	{
		return null;
	}

	public IEnumerator DeleteFile()
	{
		return null;
	}

	public IEnumerator DeleteFile(string filePath)
	{
		return null;
	}

	public IEnumerator DeleteFile(string filePath, string user)
	{
		return null;
	}

	public IEnumerator DeleteFile(string filePath, string user, string password)
	{
		return null;
	}

	public IEnumerator DeleteFile(string filePath, ES3Settings settings)
	{
		return null;
	}

	public IEnumerator DeleteFile(string filePath, string user, ES3Settings settings)
	{
		return null;
	}

	public IEnumerator DeleteFile(string filePath, string user, string password, ES3Settings settings)
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CDeleteFile_003Ed__53))]
	private IEnumerator DeleteFile(ES3Settings settings, string user, string password)
	{
		return null;
	}

	public IEnumerator RenameFile(string filePath, string newFilePath)
	{
		return null;
	}

	public IEnumerator RenameFile(string filePath, string newFilePath, string user)
	{
		return null;
	}

	public IEnumerator RenameFile(string filePath, string newFilePath, string user, string password)
	{
		return null;
	}

	public IEnumerator RenameFile(string filePath, string newFilePath, ES3Settings settings)
	{
		return null;
	}

	public IEnumerator RenameFile(string filePath, string newFilePath, string user, ES3Settings settings)
	{
		return null;
	}

	public IEnumerator RenameFile(string filePath, string newFilePath, string user, string password, ES3Settings settings)
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CRenameFile_003Ed__60))]
	private IEnumerator RenameFile(ES3Settings settings, ES3Settings newSettings, string user, string password)
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CDownloadFilenames_003Ed__61))]
	public IEnumerator DownloadFilenames(string user = "", string password = "")
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CSearchFilenames_003Ed__62))]
	public IEnumerator SearchFilenames(string searchPattern, string user = "", string password = "")
	{
		return null;
	}

	public IEnumerator DownloadTimestamp()
	{
		return null;
	}

	public IEnumerator DownloadTimestamp(string filePath)
	{
		return null;
	}

	public IEnumerator DownloadTimestamp(string filePath, string user)
	{
		return null;
	}

	public IEnumerator DownloadTimestamp(string filePath, string user, string password)
	{
		return null;
	}

	public IEnumerator DownloadTimestamp(string filePath, ES3Settings settings)
	{
		return null;
	}

	public IEnumerator DownloadTimestamp(string filePath, string user, ES3Settings settings)
	{
		return null;
	}

	public IEnumerator DownloadTimestamp(string filePath, string user, string password, ES3Settings settings)
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CDownloadTimestamp_003Ed__70))]
	private IEnumerator DownloadTimestamp(ES3Settings settings, string user, string password)
	{
		return null;
	}

	private long DateTimeToUnixTimestamp(DateTime dt)
	{
		return 0L;
	}

	private long GetFileTimestamp(ES3Settings settings)
	{
		return 0L;
	}

	protected override void Reset()
	{
	}
}
