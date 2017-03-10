using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.TargetedBundleOffers
{
	public class TargetedOfferIDRequest : PlayerIdRequest
	{
		public string OfferId
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("offerUid", this.OfferId);
			return serializer.End().ToString();
		}

		public TargetedOfferIDRequest()
		{
		}

		protected internal TargetedOfferIDRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedOfferIDRequest)GCHandledObjects.GCHandleToObject(instance)).OfferId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TargetedOfferIDRequest)GCHandledObjects.GCHandleToObject(instance)).OfferId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedOfferIDRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
