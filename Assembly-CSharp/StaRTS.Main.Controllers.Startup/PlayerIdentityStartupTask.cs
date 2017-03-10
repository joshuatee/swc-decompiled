using StaRTS.Main.Models.Player;
using StaRTS.Main.RUF;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Startup
{
	public class PlayerIdentityStartupTask : StartupTask
	{
		public PlayerIdentityStartupTask(float startPercentage) : base(startPercentage)
		{
		}

		public override void Start()
		{
			new PlayerIdentityController();
			new QuestController();
			new SupportController();
			new RUFManager();
			new PopupsManager();
			new AccountSyncController();
			new SocialDataController();
			new SharedPlayerPrefs();
			new LeaderboardController();
			base.Complete();
		}

		protected internal PlayerIdentityStartupTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PlayerIdentityStartupTask)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
