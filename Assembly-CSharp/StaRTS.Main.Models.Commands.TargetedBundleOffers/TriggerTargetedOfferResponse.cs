using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.TargetedBundleOffers
{
	public class TriggerTargetedOfferResponse : AbstractResponse
	{
		public string OfferId
		{
			get;
			private set;
		}

		public uint TriggerDate
		{
			get;
			private set;
		}

		public TriggerTargetedOfferResponse()
		{
		}

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null)
			{
				return this;
			}
			if (dictionary.ContainsKey("offerUid"))
			{
				this.OfferId = Convert.ToString(dictionary["offerUid"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("triggerDate"))
			{
				this.TriggerDate = Convert.ToUInt32(dictionary["triggerDate"], CultureInfo.InvariantCulture);
			}
			return this;
		}

		protected internal TriggerTargetedOfferResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TriggerTargetedOfferResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TriggerTargetedOfferResponse)GCHandledObjects.GCHandleToObject(instance)).OfferId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TriggerTargetedOfferResponse)GCHandledObjects.GCHandleToObject(instance)).OfferId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
