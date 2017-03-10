using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.TargetedBundleOffers
{
	public class ReserveTargetedOfferIDRequest : PlayerIdRequest
	{
		public string OfferId
		{
			get;
			set;
		}

		public string ProductId
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("offerUid", this.OfferId);
			serializer.AddString("offerProductId", this.ProductId);
			return serializer.End().ToString();
		}

		public ReserveTargetedOfferIDRequest()
		{
		}

		protected internal ReserveTargetedOfferIDRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ReserveTargetedOfferIDRequest)GCHandledObjects.GCHandleToObject(instance)).OfferId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ReserveTargetedOfferIDRequest)GCHandledObjects.GCHandleToObject(instance)).ProductId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ReserveTargetedOfferIDRequest)GCHandledObjects.GCHandleToObject(instance)).OfferId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ReserveTargetedOfferIDRequest)GCHandledObjects.GCHandleToObject(instance)).ProductId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ReserveTargetedOfferIDRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
