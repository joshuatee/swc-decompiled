using StaRTS.Assets;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;

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
			new PromoPopupManager();
			new UXController();
			new UserInputInhibitor();
			new BackButtonManager();
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
			Service.Get<AssetManager>().UnloadDependencyBundle("gui_shared");
			base.Complete();
		}
	}
}
