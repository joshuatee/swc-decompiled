using StaRTS.Main.Models;
using StaRTS.Main.Models.Chat;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Chat;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace StaRTS.Main.Controllers.Squads
{
	public class ChatServerAdapter : AbstractSquadServerAdapter
	{
		private const int WWW_MAX_RETRY_DEFAULT = 3;

		private const float PUBLISH_TIMER_DELAY_DEFAULT = 2f;

		private ChatSessionState sessionState;

		private string sessionUrl;

		private string signedSession;

		private string sessionTimeTag;

		private uint pullRequestId;

		private string channelUrl;

		private Queue<string> queuedPublishUrls;

		private uint publishTimerId;

		private int wwwRetryCount;

		private int wwwMaxRetry;

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
				this.channelUrl = string.Format("https://n7-starts-prod-chat-manager.playdom.com/dsg/chat/v1/starts/getChannel?session={0}", this.signedSession);
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
			this.wwwMaxRetry = ((GameConstants.WWW_MAX_RETRY != 0) ? GameConstants.WWW_MAX_RETRY : 3);
			this.publishTimerDelay = ((GameConstants.PUBLISH_TIMER_DELAY != 0) ? ((float)GameConstants.PUBLISH_TIMER_DELAY) : 2f);
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
			string item2 = string.Format("https://n7-starts-prod-chat-manager.playdom.com/dsg/chat/v1/starts/postMessage?session={0}&message={1}", this.signedSession, WWW.EscapeURL(message));
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

		[DebuggerHidden]
		private IEnumerator ConnectToChannel()
		{
			ChatServerAdapter.<ConnectToChannel>c__Iterator16 <ConnectToChannel>c__Iterator = new ChatServerAdapter.<ConnectToChannel>c__Iterator16();
			<ConnectToChannel>c__Iterator.<>f__this = this;
			return <ConnectToChannel>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator PullMessages()
		{
			ChatServerAdapter.<PullMessages>c__Iterator17 <PullMessages>c__Iterator = new ChatServerAdapter.<PullMessages>c__Iterator17();
			<PullMessages>c__Iterator.<>f__this = this;
			return <PullMessages>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator Publish(string url)
		{
			ChatServerAdapter.<Publish>c__Iterator18 <Publish>c__Iterator = new ChatServerAdapter.<Publish>c__Iterator18();
			<Publish>c__Iterator.url = url;
			<Publish>c__Iterator.<$>url = url;
			<Publish>c__Iterator.<>f__this = this;
			return <Publish>c__Iterator;
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
					if (squadMsg2 != null)
					{
						if (this.IsMsgValid(squadMsg2))
						{
							this.list.Add(squadMsg2);
							if (squadMsg == null || squadMsg2.TimeSent > squadMsg.TimeSent)
							{
								squadMsg = squadMsg2;
							}
						}
					}
					i++;
				}
			}
			if (squadMsg != null && squadMsg.ChatData != null)
			{
				string arg = WWW.EscapeURL(squadMsg.ChatData.Time).Replace("+", "%20");
				this.sessionTimeTag = string.Format("&tag={0}&time={1}", squadMsg.ChatData.Tag, arg);
			}
		}

		private bool IsMsgValid(SquadMsg msg)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			return msg.OwnerData == null || !(currentPlayer.PlayerId == msg.OwnerData.PlayerId) || currentPlayer.LoginTime >= msg.TimeSent;
		}
	}
}
