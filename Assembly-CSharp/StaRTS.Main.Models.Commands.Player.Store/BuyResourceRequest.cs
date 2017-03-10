using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Store
{
	public class BuyResourceRequest : PlayerIdChecksumRequest
	{
		private const string TYPE_RESOURCES = "Resources";

		private const string UID_DROIDS = "droids";

		private const string UID_PROTECTION = "protection";

		private string uid;

		private string currency;

		private string type;

		private int count;

		private string purchaseContext;

		public static BuyResourceRequest MakeBuyResourceRequest(CurrencyType resourceToBuy, int amount)
		{
			string text = null;
			switch (resourceToBuy)
			{
			case CurrencyType.Credits:
				text = "credits";
				break;
			case CurrencyType.Materials:
				text = "materials";
				break;
			case CurrencyType.Contraband:
				text = "contraband";
				break;
			}
			return new BuyResourceRequest(text, amount);
		}

		public static BuyResourceRequest MakeBuyDroidRequest(int amount)
		{
			return new BuyResourceRequest("droids", amount);
		}

		public static BuyResourceRequest MakeBuyProtectionRequest(int packNumber)
		{
			return new BuyResourceRequest("protection", packNumber);
		}

		public void setPurchaseContext(string value)
		{
			this.purchaseContext = value;
		}

		private BuyResourceRequest(string uid, int amount)
		{
			this.uid = uid;
			this.count = amount;
			this.currency = "crystals";
			this.type = "Resources";
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("uid", this.uid);
			startedSerializer.AddString("currency", this.currency);
			startedSerializer.AddString("type", this.type);
			startedSerializer.Add<int>("count", this.count);
			if (this.purchaseContext != null && !this.purchaseContext.Equals(""))
			{
				startedSerializer.AddString("purchaseContext", this.purchaseContext);
			}
			return startedSerializer.End().ToString();
		}

		protected internal BuyResourceRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuyResourceRequest.MakeBuyDroidRequest(*(int*)args));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuyResourceRequest.MakeBuyProtectionRequest(*(int*)args));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuyResourceRequest.MakeBuyResourceRequest((CurrencyType)(*(int*)args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((BuyResourceRequest)GCHandledObjects.GCHandleToObject(instance)).setPurchaseContext(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuyResourceRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
