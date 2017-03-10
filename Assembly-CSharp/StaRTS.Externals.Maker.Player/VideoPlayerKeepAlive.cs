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
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Externals.Maker.Player
{
	public class VideoPlayerKeepAlive : MonoBehaviour, IUnitySerializable
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
			if (int.TryParse(percentPlayed, ref num))
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

		public VideoPlayerKeepAlive()
		{
		}

		public override void Unity_Serialize(int depth)
		{
		}

		public override void Unity_Deserialize(int depth)
		{
		}

		public override void Unity_RemapPPtrs(int depth)
		{
		}

		public override void Unity_NamedSerialize(int depth)
		{
		}

		public override void Unity_NamedDeserialize(int depth)
		{
		}

		protected internal VideoPlayerKeepAlive(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((VideoPlayerKeepAlive)GCHandledObjects.GCHandleToObject(instance)).AbandonPlayback();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((VideoPlayerKeepAlive)GCHandledObjects.GCHandleToObject(instance)).Begin(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((VideoPlayerKeepAlive)GCHandledObjects.GCHandleToObject(instance)).CleanupPlayback(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((VideoPlayerKeepAlive)GCHandledObjects.GCHandleToObject(instance)).Dispatch();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((VideoPlayerKeepAlive)GCHandledObjects.GCHandleToObject(instance)).EndPlayback(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoPlayerKeepAlive)GCHandledObjects.GCHandleToObject(instance)).Guid);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(VideoPlayerKeepAlive.Instance);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoPlayerKeepAlive)GCHandledObjects.GCHandleToObject(instance)).ShouldDispatch);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			VideoPlayerKeepAlive.Init();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideoPlayerKeepAlive)GCHandledObjects.GCHandleToObject(instance)).IsDisplayed());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((VideoPlayerKeepAlive)GCHandledObjects.GCHandleToObject(instance)).Pause();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((VideoPlayerKeepAlive)GCHandledObjects.GCHandleToObject(instance)).Play();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((VideoPlayerKeepAlive)GCHandledObjects.GCHandleToObject(instance)).Guid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((VideoPlayerKeepAlive)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((VideoPlayerKeepAlive)GCHandledObjects.GCHandleToObject(instance)).StartPlayback();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((VideoPlayerKeepAlive)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((VideoPlayerKeepAlive)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((VideoPlayerKeepAlive)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((VideoPlayerKeepAlive)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((VideoPlayerKeepAlive)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((VideoPlayerKeepAlive)GCHandledObjects.GCHandleToObject(instance)).Update();
			return -1L;
		}
	}
}
