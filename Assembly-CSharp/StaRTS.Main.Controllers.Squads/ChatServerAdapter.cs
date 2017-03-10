using StaRTS.Main.Models;
using StaRTS.Main.Models.Chat;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Chat;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Squads
{
	public class ChatServerAdapter : AbstractSquadServerAdapter
	{
		private ChatSessionState sessionState;

		private string sessionUrl;

		private string signedSession;

		private string sessionTimeTag;

		private uint pullRequestId;

		private string channelUrl;

		private Queue<string> queuedPublishUrls;

		private uint publishTimerId;

		private int wwwRetryCount;

		private const int WWW_MAX_RETRY_DEFAULT = 3;

		private int wwwMaxRetry;

		private const float PUBLISH_TIMER_DELAY_DEFAULT = 2f;

		private float publishTimerDelay;

		public ChatServerAdapter()
		{
			this.queuedPublishUrls = new Queue<string>();
			this.Reset();
		}

		private void Reset()
		{
			this.sessionState = ChatSessionState.Disconnected;
			this.signedSession = null;
			this.sessionUrl = null;
			this.channelUrl = null;
			this.pullRequestId = 0u;
			this.sessionTimeTag = "&tag=12&time=Mon%2C%2003%20Mar%202014%2001%3A41%3A22%20GMT";
			this.queuedPublishUrls.Clear();
			if (this.publishTimerId != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.publishTimerId);
				this.publishTimerId = 0u;
			}
		}

		public void StartSession(ChatType chatType, string channelId)
		{
			if (this.sessionState == ChatSessionState.Disconnected)
			{
				this.sessionState = ChatSessionState.Connecting;
				uint unixTimestamp = ChatTimeConversionUtils.GetUnixTimestamp();
				uint num = unixTimestamp + 86400u;
				CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
				string locale = Service.Get<Lang>().ExtractLanguageFromLocale();
				string json = ChatSessionUtils.CreateSessionString(currentPlayer.PlayerId, currentPlayer.PlayerName, locale, num.ToString(), chatType, channelId);
				this.signedSession = ChatSessionUtils.GetSignedString(json);
				this.channelUrl = string.Format("https://startswin-prod-chat-manager.playdom.com/dsg/chat/v1/strtw/getChannel?session={0}", new object[]
				{
					this.signedSession
				});
				Service.Get<Engine>().StartCoroutine(this.ConnectToChannel());
			}
		}

		public override void Disable()
		{
			if (this.sessionState != ChatSessionState.Disconnected)
			{
				this.Reset();
			}
			base.Disable();
		}

		public override void Enable(SquadController.SquadMsgsCallback callback, float pollFrequency)
		{
			this.wwwMaxRetry = ((GameConstants.WWW_MAX_RETRY == 0) ? 3 : GameConstants.WWW_MAX_RETRY);
			this.publishTimerDelay = ((GameConstants.PUBLISH_TIMER_DELAY == 0) ? 2f : ((float)GameConstants.PUBLISH_TIMER_DELAY));
			base.Enable(callback, pollFrequency);
		}

		public void PublishMessage(string message)
		{
			if (string.IsNullOrEmpty(message))
			{
				return;
			}
			SquadMsg item = SquadMsgUtils.GenerateMessageFromChatMessage(message);
			this.list.Clear();
			this.list.Add(item);
			this.callback(this.list);
			string item2 = string.Format("https://startswin-prod-chat-manager.playdom.com/dsg/chat/v1/strtw/postMessage?session={0}&message={1}", new object[]
			{
				this.signedSession,
				WWW.EscapeURL(message)
			});
			this.queuedPublishUrls.Enqueue(item2);
			if (this.publishTimerId == 0u)
			{
				this.publishTimerId = Service.Get<ViewTimerManager>().CreateViewTimer(this.publishTimerDelay, true, new TimerDelegate(this.OnPublishTimer), null);
			}
			Service.Get<EventManager>().SendEvent(EventId.SquadChatSent, null);
		}

		private void OnPublishTimer(uint id, object cookie)
		{
			if (this.queuedPublishUrls.Count == 0)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.publishTimerId);
				this.publishTimerId = 0u;
				return;
			}
			ChatSessionState chatSessionState = this.sessionState;
			if (chatSessionState == ChatSessionState.Connecting)
			{
				return;
			}
			if (chatSessionState != ChatSessionState.Disconnected)
			{
				string url = this.queuedPublishUrls.Dequeue();
				Service.Get<Engine>().StartCoroutine(this.Publish(url));
				return;
			}
			this.ReconnectSession();
		}

		private void ReconnectSession()
		{
			this.sessionState = ChatSessionState.Connecting;
			Service.Get<Engine>().StartCoroutine(this.ConnectToChannel());
		}

		protected override void Poll()
		{
			if (this.sessionState == ChatSessionState.Connected)
			{
				this.pullRequestId += 1u;
				Service.Get<Engine>().StartCoroutine(this.PullMessages());
			}
		}

		[IteratorStateMachine(typeof(ChatServerAdapter.<ConnectToChannel>d__22))]
		private IEnumerator ConnectToChannel()
		{
			WWW wWW = new WWW(this.channelUrl);
			WWWManager.Add(wWW);
			yield return wWW;
			if (WWWManager.Remove(wWW) && this.sessionState == ChatSessionState.Connecting)
			{
				string error = wWW.error;
				string text = wWW.text;
				if (error == null)
				{
					this.wwwRetryCount = 0;
					this.sessionState = ChatSessionState.Connected;
					this.sessionUrl = ChatSessionUtils.GetSessionUrlFromChannelResponse(text);
					if (!string.IsNullOrEmpty(this.sessionUrl))
					{
						this.Poll();
					}
					else
					{
						Service.Get<StaRTSLogger>().Error("Invalid chat channel response: " + text);
					}
				}
				else
				{
					int num = this.wwwRetryCount + 1;
					this.wwwRetryCount = num;
					if (num < this.wwwMaxRetry)
					{
						this.ReconnectSession();
						Service.Get<StaRTSLogger>().Warn("Unable to start chat session. Retrying: " + error);
					}
					else
					{
						this.sessionState = ChatSessionState.Disconnected;
						Service.Get<StaRTSLogger>().Warn("Unable to start chat session: " + error);
					}
				}
			}
			wWW.Dispose();
			yield break;
		}

		[IteratorStateMachine(typeof(ChatServerAdapter.<PullMessages>d__23))]
		private IEnumerator PullMessages()
		{
			string url = this.sessionUrl + this.sessionTimeTag;
			uint num = this.pullRequestId;
			WWW wWW = new WWW(url);
			WWWManager.Add(wWW);
			yield return wWW;
			if (WWWManager.Remove(wWW) && num == this.pullRequestId)
			{
				if (wWW.error == null)
				{
					this.OnPollFinished(wWW.text);
				}
				else if (this.sessionState == ChatSessionState.Connected)
				{
					this.OnPollFinished(null);
				}
				else
				{
					this.ReconnectSession();
					Service.Get<StaRTSLogger>().Warn("Unable to pull chat messages. Reconnecting: " + wWW.error);
				}
			}
			wWW.Dispose();
			yield break;
		}

		[IteratorStateMachine(typeof(ChatServerAdapter.<Publish>d__24))]
		private IEnumerator Publish(string url)
		{
			WWW wWW = new WWW(url);
			WWWManager.Add(wWW);
			yield return wWW;
			if (WWWManager.Remove(wWW))
			{
				if (wWW.error == null)
				{
					this.wwwRetryCount = 0;
				}
				else
				{
					int num = this.wwwRetryCount + 1;
					this.wwwRetryCount = num;
					if (num < this.wwwMaxRetry)
					{
						Service.Get<Engine>().StartCoroutine(this.Publish(url));
						Service.Get<StaRTSLogger>().Warn("Unable to publish chat message. Retrying: " + wWW.error);
					}
					else
					{
						Service.Get<StaRTSLogger>().Warn("Unable to publish chat: " + wWW.error);
					}
				}
			}
			wWW.Dispose();
			yield break;
		}

		protected override void PopulateSquadMsgsReceived(object response)
		{
			string text = response as string;
			if (text == null)
			{
				return;
			}
			text = text.Replace("}{", "},{");
			text = "[" + text + "]";
			SquadMsg squadMsg = null;
			object obj = new JsonParser(text).Parse();
			List<object> list = obj as List<object>;
			if (list != null)
			{
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					SquadMsg squadMsg2 = SquadMsgUtils.GenerateMessageFromChatObject(list[i]);
					if (squadMsg2 != null && this.IsMsgValid(squadMsg2))
					{
						this.list.Add(squadMsg2);
						if (squadMsg == null || squadMsg2.TimeSent > squadMsg.TimeSent)
						{
							squadMsg = squadMsg2;
						}
					}
					i++;
				}
			}
			if (squadMsg != null && squadMsg.ChatData != null)
			{
				string text2 = WWW.EscapeURL(squadMsg.ChatData.Time).Replace("+", "%20");
				this.sessionTimeTag = string.Format("&tag={0}&time={1}", new object[]
				{
					squadMsg.ChatData.Tag,
					text2
				});
			}
		}

		private bool IsMsgValid(SquadMsg msg)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			return msg.OwnerData == null || !(currentPlayer.PlayerId == msg.OwnerData.PlayerId) || currentPlayer.LoginTime >= msg.TimeSent;
		}

		protected internal ChatServerAdapter(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatServerAdapter)GCHandledObjects.GCHandleToObject(instance)).ConnectToChannel());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ChatServerAdapter)GCHandledObjects.GCHandleToObject(instance)).Disable();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ChatServerAdapter)GCHandledObjects.GCHandleToObject(instance)).Enable((SquadController.SquadMsgsCallback)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatServerAdapter)GCHandledObjects.GCHandleToObject(instance)).IsMsgValid((SquadMsg)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ChatServerAdapter)GCHandledObjects.GCHandleToObject(instance)).Poll();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ChatServerAdapter)GCHandledObjects.GCHandleToObject(instance)).PopulateSquadMsgsReceived(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatServerAdapter)GCHandledObjects.GCHandleToObject(instance)).Publish(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((ChatServerAdapter)GCHandledObjects.GCHandleToObject(instance)).PublishMessage(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChatServerAdapter)GCHandledObjects.GCHandleToObject(instance)).PullMessages());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((ChatServerAdapter)GCHandledObjects.GCHandleToObject(instance)).ReconnectSession();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((ChatServerAdapter)GCHandledObjects.GCHandleToObject(instance)).Reset();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((ChatServerAdapter)GCHandledObjects.GCHandleToObject(instance)).StartSession((ChatType)(*(int*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}
	}
}
