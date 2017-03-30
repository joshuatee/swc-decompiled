using StaRTS.Assets;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Controllers.Startup
{
	public class LangStartupTask : StartupTask
	{
		public LangStartupTask(float startPercentage) : base(startPercentage)
		{
		}

		public override void Start()
		{
			Lang lang = Service.Get<Lang>();
			string pref = Service.Get<ServerPlayerPrefs>().GetPref(ServerPref.Locale);
			if (pref != lang.Locale)
			{
				lang.Locale = pref;
				LangUtils.AddLocalStringsData(pref);
			}
			LangUtils.LoadStringData(new AssetsCompleteDelegate(this.OnComplete));
			Service.Get<EventManager>().SendEvent(EventId.StringsLoadStart, null);
		}

		private void OnComplete(object cookie)
		{
			Lang lang = Service.Get<Lang>();
			lang.UnloadAssets();
			lang.SetupAvailableLocales(GameConstants.ALL_LOCALES, lang.Get("ALL_LANGUAGES", new object[0]));
			lang.Initialized = true;
			Service.Get<EventManager>().SendEvent(EventId.LanguageLoaded, null);
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (string.IsNullOrEmpty(currentPlayer.PlayerName))
			{
				currentPlayer.PlayerName = Service.Get<Lang>().Get("general_none", new object[0]);
				currentPlayer.PlayerNameInvalid = true;
			}
			new ProfanityController();
			base.Complete();
			Service.Get<EventManager>().SendEvent(EventId.StringsLoadEnd, null);
		}
	}
}
