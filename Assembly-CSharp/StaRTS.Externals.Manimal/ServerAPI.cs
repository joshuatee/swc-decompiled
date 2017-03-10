using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Utils;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Externals.Manimal
{
	public class ServerAPI : IResponseHandler, IViewFrameTimeObserver
	{
		public delegate void DesynHandler(string message, uint status, ProtocolResult result);

		public delegate void MessageHandler(Dictionary<string, object> messages);

		private const bool PICKUP_MESSAGES = true;

		private const float QUEUE_DISPATCH_DELAY = 3f;

		private ServerAPI.DesynHandler desyncHandler;

		private ServerAPI.MessageHandler messageHandler;

		private uint protocolVersion;

		private int serverTimeOffset;

		private Dispatcher dispatcher;

		private uint lastDispatchTime;

		private uint lastKeepAliveTime;

		private uint firstCommandQueuedTime;

		private static readonly string url = "{0}/batch/json";

		private Dictionary<DesyncType, string> desyncMessages;

		private List<ICommand> queue;

		private ICommand keepAliveCommand;

		private float keepAliveWaitTime;

		private const string DESYNC_COMMAND_MAX_RETRY = "DESYNC_COMMAND_MAX_RETRY";

		private const string DESYNC_CRITICAL_COMMAND_FAIL = "DESYNC_CRITICAL_COMMAND_FAIL";

		private const string DESYNC_RECEIPT_FAILED = "DESYNC_RECEIPT_FAILED";

		public bool Enabled
		{
			get;
			set;
		}

		public bool IsUsingRealAuthentication
		{
			get;
			set;
		}

		public DateTime ServerDateTime
		{
			get
			{
				return DateUtils.DateFromSeconds(this.ServerTime);
			}
		}

		public uint ServerTime
		{
			get;
			private set;
		}

		public double ServerTimePrecise
		{
			get;
			private set;
		}

		public string SessionId
		{
			get;
			private set;
		}

		public ServerAPI(string baseUrl, uint protocolVersion, ViewTimerManager timerManager, MonoBehaviour engine, ServerAPI.DesynHandler desyncHandler, ServerAPI.MessageHandler messageHandler)
		{
			this.SetDispatcher(baseUrl, engine);
			this.serverTimeOffset = 0;
			this.protocolVersion = protocolVersion;
			this.desyncHandler = desyncHandler;
			this.messageHandler = messageHandler;
			RequestId.Reset();
			this.queue = new List<ICommand>();
			this.keepAliveCommand = null;
			this.Enabled = true;
			this.ServerTime = 0u;
			this.ServerTimePrecise = 0.0;
			this.desyncMessages = new Dictionary<DesyncType, string>();
			this.desyncMessages.Add(DesyncType.BatchMaxRetry, LangUtils.DESYNC_BATCH_MAX_RETRY);
			this.desyncMessages.Add(DesyncType.CommandMaxRetry, "DESYNC_COMMAND_MAX_RETRY");
			this.desyncMessages.Add(DesyncType.CriticalCommandFail, "DESYNC_CRITICAL_COMMAND_FAIL");
			this.desyncMessages.Add(DesyncType.ReceiptVerificationFailed, "DESYNC_RECEIPT_FAILED");
			this.SessionId = Guid.NewGuid().ToString();
			Service.Set<ServerAPI>(this);
		}

		public void SetDispatcher(string baseUrl, MonoBehaviour engine)
		{
			Service.Get<StaRTSLogger>().Debug("Dispatcher URL set to " + baseUrl);
			this.dispatcher = new Dispatcher(string.Format(ServerAPI.url, new object[]
			{
				baseUrl
			}), engine, true, this);
		}

		public Dispatcher GetDispatcher()
		{
			return this.dispatcher;
		}

		public void SetAuth(string authKey)
		{
			this.dispatcher.AuthKey = authKey;
		}

		public void SetKeepAlive(ICommand keepAliveCommand, float keepAliveWaitTime)
		{
			this.keepAliveCommand = keepAliveCommand;
			this.keepAliveWaitTime = keepAliveWaitTime;
		}

		public void StartSession(uint loginTime)
		{
			this.dispatcher.LastLoginTime = loginTime;
			this.serverTimeOffset = GameUtils.GetTimeDifferenceSafe(loginTime, DateUtils.GetNowSeconds()) - 1;
			this.ServerTime = loginTime;
			Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
		}

		public void OnViewFrameTime(float dt)
		{
			this.UpdateServerTime();
			uint num = this.ServerTime - this.lastDispatchTime;
			if (num >= 3f)
			{
				uint num2 = this.ServerTime - this.lastKeepAliveTime;
				if (this.keepAliveCommand != null && num >= this.keepAliveWaitTime && num2 >= this.keepAliveWaitTime)
				{
					this.Enqueue(this.keepAliveCommand);
					this.lastKeepAliveTime = this.ServerTime;
					this.TryDispatchQueue();
					return;
				}
				if (this.ServerTime - this.firstCommandQueuedTime >= 3f)
				{
					this.TryDispatchQueue();
				}
			}
		}

		private void UpdateServerTime()
		{
			this.ServerTimePrecise = DateUtils.GetNowSecondsPrecise();
			this.ServerTime = GameUtils.GetModifiedTimeSafe(DateUtils.GetNowSeconds(), this.serverTimeOffset);
			if (this.ServerTime < this.dispatcher.LastLoginTime)
			{
				this.ServerTime = this.dispatcher.LastLoginTime;
			}
		}

		private void TryDispatchQueue()
		{
			if (this.Enabled && this.queue.Count > 0 && this.dispatcher.IsFree())
			{
				Batch batch = new Batch(this.queue);
				this.dispatcher.Exec(batch);
				this.queue.Clear();
				this.firstCommandQueuedTime = 0u;
				this.lastDispatchTime = this.ServerTime;
			}
		}

		public void Async(ICommand command)
		{
			if (this.Enabled)
			{
				command.SetTime(this.ServerTime);
				Batch batch = new Batch(command);
				batch.Sync = false;
				this.dispatcher.Exec(batch);
			}
		}

		public void Enqueue(ICommand command)
		{
			command.SetTime(this.ServerTime);
			this.queue.Add(command);
			if (this.firstCommandQueuedTime == 0u)
			{
				this.firstCommandQueuedTime = this.ServerTime;
			}
		}

		public void Sync(ICommand command)
		{
			this.Enqueue(command);
			this.Sync();
		}

		public void Sync()
		{
			this.TryDispatchQueue();
		}

		public List<ICommand> GetQueue()
		{
			return this.queue;
		}

		public bool MatchProtocolVersion(uint protocolVersion)
		{
			bool flag = this.protocolVersion == protocolVersion;
			if (!flag)
			{
				string message = string.Format("Protocol version mismatch. Client: {0}, Server: {1}", new object[]
				{
					this.protocolVersion,
					protocolVersion
				});
				ProtocolResult result = (this.protocolVersion > protocolVersion) ? ProtocolResult.Higher : ProtocolResult.Lower;
				this.desyncHandler(message, 0u, result);
			}
			return flag;
		}

		public void SendMessages(Dictionary<string, object> messages)
		{
			this.messageHandler(messages);
		}

		public bool Desync(DesyncType type, uint statusCode)
		{
			string id = this.desyncMessages[type];
			string text = LangUtils.ProcessStringWithNewlines(Service.Get<Lang>().Get(id, new object[0]));
			string text2 = Service.Get<Lang>().Get("DESYNC_STATUS", new object[0]);
			text = text + "\n\n" + string.Format(text2, new object[]
			{
				statusCode
			}) + "\n\n";
			this.desyncHandler(text, statusCode, ProtocolResult.Match);
			return false;
		}

		protected internal ServerAPI(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ServerAPI)GCHandledObjects.GCHandleToObject(instance)).Async((ICommand)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ServerAPI)GCHandledObjects.GCHandleToObject(instance)).Enqueue((ICommand)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ServerAPI)GCHandledObjects.GCHandleToObject(instance)).Enabled);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ServerAPI)GCHandledObjects.GCHandleToObject(instance)).IsUsingRealAuthentication);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ServerAPI)GCHandledObjects.GCHandleToObject(instance)).ServerDateTime);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ServerAPI)GCHandledObjects.GCHandleToObject(instance)).SessionId);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ServerAPI)GCHandledObjects.GCHandleToObject(instance)).GetDispatcher());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ServerAPI)GCHandledObjects.GCHandleToObject(instance)).GetQueue());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((ServerAPI)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((ServerAPI)GCHandledObjects.GCHandleToObject(instance)).SendMessages((Dictionary<string, object>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((ServerAPI)GCHandledObjects.GCHandleToObject(instance)).Enabled = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((ServerAPI)GCHandledObjects.GCHandleToObject(instance)).IsUsingRealAuthentication = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((ServerAPI)GCHandledObjects.GCHandleToObject(instance)).ServerTimePrecise = *(double*)args;
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((ServerAPI)GCHandledObjects.GCHandleToObject(instance)).SessionId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((ServerAPI)GCHandledObjects.GCHandleToObject(instance)).SetAuth(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((ServerAPI)GCHandledObjects.GCHandleToObject(instance)).SetDispatcher(Marshal.PtrToStringUni(*(IntPtr*)args), (MonoBehaviour)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((ServerAPI)GCHandledObjects.GCHandleToObject(instance)).SetKeepAlive((ICommand)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((ServerAPI)GCHandledObjects.GCHandleToObject(instance)).Sync();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((ServerAPI)GCHandledObjects.GCHandleToObject(instance)).Sync((ICommand)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((ServerAPI)GCHandledObjects.GCHandleToObject(instance)).TryDispatchQueue();
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((ServerAPI)GCHandledObjects.GCHandleToObject(instance)).UpdateServerTime();
			return -1L;
		}
	}
}
