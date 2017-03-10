using StaRTS.Externals.IAP;
using StaRTS.Externals.Manimal;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Cheats;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Utils.Test
{
	public class PromoCodeTest : IEventObserver
	{
		public PromoCodeTest()
		{
			if (!GameConstants.PROMO_UNIT_TEST_ENABLED)
			{
				return;
			}
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.WorldLoadComplete, EventPriority.Default);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			EventManager eventManager = Service.Get<EventManager>();
			if (id != EventId.WorldLoadComplete)
			{
				if (id == EventId.IAPProductIDsReady)
				{
					eventManager.UnregisterObserver(this, EventId.IAPProductIDsReady);
					PromoCodeTest.RunTest();
				}
			}
			else
			{
				eventManager.UnregisterObserver(this, EventId.WorldLoadComplete);
				if (!Service.Get<InAppPurchaseController>().AreProductIdsReady)
				{
					eventManager.RegisterObserver(this, EventId.IAPProductIDsReady, EventPriority.Default);
				}
				else
				{
					PromoCodeTest.RunTest();
				}
			}
			return EatResponse.NotEaten;
		}

		public static void RunTest()
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			PromoCodeTestRequest promoCodeTestRequest = new PromoCodeTestRequest();
			promoCodeTestRequest.PlayerId = currentPlayer.PlayerId;
			string productId = "";
			promoCodeTestRequest.SetProductId(productId);
			PromoCodeTestCommand command = new PromoCodeTestCommand(promoCodeTestRequest);
			Service.Get<ServerAPI>().Sync(command);
		}

		protected internal PromoCodeTest(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PromoCodeTest)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			PromoCodeTest.RunTest();
			return -1L;
		}
	}
}
