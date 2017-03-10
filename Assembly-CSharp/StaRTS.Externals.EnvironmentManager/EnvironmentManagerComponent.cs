using StaRTS.Main.Controllers;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Externals.EnvironmentManager
{
	public class EnvironmentManagerComponent : MonoBehaviour, IUnitySerializable
	{
		private const string MUSIC_INTERRUPTED = "interrupted";

		private const string MUSIC_PAUSED = "paused";

		private const string MUSIC_PLAYING = "playing";

		private const string MUSIC_STOPPED = "stopped";

		public void OnNativeAlertDismissed(string buttonName)
		{
			bool flag = true;
			if (buttonName.Equals("no"))
			{
				flag = false;
			}
			if (Service.IsSet<GameIdleController>())
			{
				Service.Get<GameIdleController>().Enabled = true;
			}
			if (Service.IsSet<EventManager>())
			{
				Service.Get<EventManager>().SendEvent(EventId.NativeAlertBoxDismissed, flag);
			}
		}

		public void PlaybackStateChanged(string state)
		{
			EventManager eventManager = Service.Get<EventManager>();
			if (eventManager == null)
			{
				return;
			}
			if (state == "playing")
			{
				eventManager.SendEvent(EventId.DeviceMusicPlayerStateChanged, true);
				return;
			}
			if (state == "interrupted" || state == "paused" || state == "stopped")
			{
				eventManager.SendEvent(EventId.DeviceMusicPlayerStateChanged, false);
			}
		}

		public EnvironmentManagerComponent()
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

		protected internal EnvironmentManagerComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((EnvironmentManagerComponent)GCHandledObjects.GCHandleToObject(instance)).OnNativeAlertDismissed(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((EnvironmentManagerComponent)GCHandledObjects.GCHandleToObject(instance)).PlaybackStateChanged(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((EnvironmentManagerComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((EnvironmentManagerComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((EnvironmentManagerComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((EnvironmentManagerComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((EnvironmentManagerComponent)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
			return -1L;
		}
	}
}
