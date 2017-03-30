using StaRTS.Main.Controllers;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections;
using System.Diagnostics;

namespace StaRTS.Main.Utils
{
	public class NetworkConnectionTester : IEventObserver
	{
		private const string NO_INTERNET_ERROR_PREFIX = "Could not resolve host";

		private const string CDN_URL = "https://starts0.content.disney.io/cloud-cms/";

		private const string TEST_ASSET = "starts/prod/connection_test.txt/260ca9dd8a4577fc00b7bd5810298076.connection_test.txt";

		private Engine engine;

		public NetworkConnectionTester()
		{
			Service.Get<EventManager>().RegisterObserver(this, EventId.ApplicationPauseToggled, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.BattleEndFullyProcessed, EventPriority.Default);
			Service.Get<EventManager>().RegisterObserver(this, EventId.WorldLoadComplete, EventPriority.Default);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.ApplicationPauseToggled)
			{
				this.CheckNetworkConnectionAvailable((bool)cookie);
			}
			return EatResponse.NotEaten;
		}

		public void CheckNetworkConnectionAvailable(bool paused)
		{
			if (!paused)
			{
				this.engine = Service.Get<Engine>();
				string url = "https://starts0.content.disney.io/cloud-cms/starts/prod/connection_test.txt/260ca9dd8a4577fc00b7bd5810298076.connection_test.txt?r=" + StringUtils.GenerateRandom(32u);
				this.engine.StartCoroutine(this.Download(url));
			}
		}

		[DebuggerHidden]
		private IEnumerator Download(string url)
		{
			NetworkConnectionTester.<Download>c__Iterator19 <Download>c__Iterator = new NetworkConnectionTester.<Download>c__Iterator19();
			<Download>c__Iterator.url = url;
			<Download>c__Iterator.<$>url = url;
			return <Download>c__Iterator;
		}
	}
}
