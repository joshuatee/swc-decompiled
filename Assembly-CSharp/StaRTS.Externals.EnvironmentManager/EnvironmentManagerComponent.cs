using StaRTS.Main.Controllers;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using System;
using UnityEngine;

namespace StaRTS.Externals.EnvironmentManager
{
	public class EnvironmentManagerComponent : MonoBehaviour
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
			}
			else if (state == "interrupted" || state == "paused" || state == "stopped")
			{
				eventManager.SendEvent(EventId.DeviceMusicPlayerStateChanged, false);
			}
		}
	}
}
