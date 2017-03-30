using StaRTS.Externals.BI;
using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace StaRTS.Externals.Maker.Player
{
	public class VideoPlayerKeepAlive : MonoBehaviour
	{
		private const string NAME = "VideoPlayerKeepAlive";

		private bool playing;

		private bool displayed;

		private uint lastDispatchTime;

		private ICommand keepAliveCommand;

		private IState preVideoState;

		private bool ShouldDispatch
		{
			get
			{
				return Service.IsSet<ServerAPI>() && Service.Get<ServerAPI>().ServerTime - this.lastDispatchTime >= GameConstants.KEEP_ALIVE_DISPATCH_WAIT_TIME;
			}
		}

		public static VideoPlayerKeepAlive Instance
		{
			get
			{
				GameObject gameObject = GameObject.Find("VideoPlayerKeepAlive");
				if (gameObject == null)
				{
					return null;
				}
				return gameObject.GetComponent<VideoPlayerKeepAlive>();
			}
		}

		public string Guid
		{
			get;
			set;
		}

		public static void Init()
		{
			GameObject gameObject = GameObject.Find("VideoPlayerKeepAlive");
			if (gameObject == null)
			{
				gameObject = new GameObject("VideoPlayerKeepAlive");
				gameObject.AddComponent<VideoPlayerKeepAlive>();
			}
		}

		private void Start()
		{
		}

		private void Update()
		{
			if (this.playing && this.ShouldDispatch)
			{
				this.Dispatch();
			}
		}

		public void Begin(string videoGuid)
		{
			this.Guid = videoGuid;
			this.displayed = true;
			if (this.keepAliveCommand == null)
			{
				this.keepAliveCommand = new KeepAliveCommand(new KeepAliveRequest());
			}
			this.Dispatch();
		}

		public void Play()
		{
			this.playing = true;
		}

		public void Pause()
		{
			this.playing = false;
		}

		public void StartPlayback()
		{
			if (Service.IsSet<GameStateMachine>())
			{
				this.preVideoState = Service.Get<GameStateMachine>().CurrentState;
				Service.Get<GameStateMachine>().SetState(new VideoPlayBackState());
			}
			if (Service.IsSet<EventManager>())
			{
				Service.Get<EventManager>().SendEvent(EventId.VideoStart, this.Guid);
			}
		}

		private void CleanupPlayback(bool normalEnd)
		{
			if (Service.IsSet<GameStateMachine>() && this.preVideoState != null)
			{
				if (this.preVideoState is HomeState)
				{
					HomeState.GoToHomeStateAndReloadMap();
				}
				else
				{
					Service.Get<GameStateMachine>().SetState(this.preVideoState);
					if (Service.IsSet<WorldInitializer>() && Service.IsSet<CurrentPlayer>())
					{
						Service.Get<WorldInitializer>().ProcessMapData(Service.Get<CurrentPlayer>().Map);
					}
				}
			}
			this.playing = false;
			this.displayed = false;
			this.preVideoState = null;
			KeyValuePair<bool, string> keyValuePair = new KeyValuePair<bool, string>(normalEnd, this.Guid);
			if (Service.IsSet<EventManager>())
			{
				Service.Get<EventManager>().SendEvent(EventId.VideoEnd, keyValuePair);
			}
		}

		public void EndPlayback(string percentPlayed)
		{
			this.CleanupPlayback(true);
			int num;
			if (int.TryParse(percentPlayed, out num))
			{
				num = num / 10 * 10;
			}
			else
			{
				num = 0;
			}
			if (Service.IsSet<BILoggingController>())
			{
				Service.Get<BILoggingController>().TrackGameAction("video_end", num.ToString(), this.Guid, null);
			}
		}

		public void AbandonPlayback()
		{
			this.CleanupPlayback(false);
			if (Service.IsSet<BILoggingController>())
			{
				Service.Get<BILoggingController>().TrackGameAction("video_fail", "abandon_playback", this.Guid, null);
			}
		}

		private void Dispatch()
		{
			if (!Service.IsSet<ServerAPI>())
			{
				return;
			}
			Service.Get<ServerAPI>().Sync(this.keepAliveCommand);
			this.lastDispatchTime = Service.Get<ServerAPI>().ServerTime;
		}

		public bool IsDisplayed()
		{
			return this.displayed;
		}
	}
}
