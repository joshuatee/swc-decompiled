using StaRTS.Assets;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Startup
{
	public class UserInputStartupTask : StartupTask, IEventObserver
	{
		public UserInputStartupTask(float startPercentage) : base(startPercentage)
		{
		}

		public override void Start()
		{
			Service.Get<UserInputManager>().Init();
			Service.Get<EventManager>().RegisterObserver(this, EventId.HudComplete, EventPriority.Default);
			new ScreenController();
			new UXController();
			new UserInputInhibitor();
			new BackButtonManager();
			ScreenSizeController instance = ScreenSizeController.Instance;
			if (instance != null)
			{
				instance.isEnabled = true;
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.HudComplete)
			{
				Service.Get<EventManager>().UnregisterObserver(this, EventId.HudComplete);
				if (!(cookie is HUD))
				{
					throw new Exception("Unable to load the HUD");
				}
				Service.Get<ScreenController>().PreloadAndCacheScreens(new AssetsCompleteDelegate(this.OnPreloadAndCacheScreensComplete), null);
			}
			return EatResponse.NotEaten;
		}

		private void OnPreloadAndCacheScreensComplete(object cookie)
		{
			base.Complete();
		}

		protected internal UserInputStartupTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UserInputStartupTask)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((UserInputStartupTask)GCHandledObjects.GCHandleToObject(instance)).OnPreloadAndCacheScreensComplete(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((UserInputStartupTask)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
