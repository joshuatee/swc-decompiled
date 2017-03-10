using StaRTS.Main.Controllers;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

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

		[IteratorStateMachine(typeof(NetworkConnectionTester.<Download>d__7))]
		private IEnumerator Download(string url)
		{
			WWW wWW = new WWW(url);
			WWWManager.Add(wWW);
			yield return wWW;
			if (!WWWManager.Remove(wWW))
			{
				yield break;
			}
			if (!string.IsNullOrEmpty(wWW.error) && wWW.error.StartsWith("Could not resolve host"))
			{
				Lang lang = Service.Get<Lang>();
				AlertScreen.ShowModal(true, lang.Get("NO_INTERNET_TITLE", new object[0]), lang.Get("NO_INTERNET", new object[0]), null, null);
			}
			wWW.Dispose();
			yield break;
		}

		protected internal NetworkConnectionTester(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((NetworkConnectionTester)GCHandledObjects.GCHandleToObject(instance)).CheckNetworkConnectionAvailable(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NetworkConnectionTester)GCHandledObjects.GCHandleToObject(instance)).Download(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NetworkConnectionTester)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
