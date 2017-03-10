using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Requests
{
	public class TroopDonateRequest : PlayerIdRequest
	{
		public Dictionary<string, int> Donations
		{
			get;
			set;
		}

		public string RecipientId
		{
			get;
			set;
		}

		public string RequestId
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("recipientId", this.RecipientId);
			serializer.AddString("requestId", this.RequestId);
			serializer.AddDictionary<int>("troopsDonated", this.Donations);
			return serializer.End().ToString();
		}

		public TroopDonateRequest()
		{
		}

		protected internal TroopDonateRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDonateRequest)GCHandledObjects.GCHandleToObject(instance)).Donations);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDonateRequest)GCHandledObjects.GCHandleToObject(instance)).RecipientId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDonateRequest)GCHandledObjects.GCHandleToObject(instance)).RequestId);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TroopDonateRequest)GCHandledObjects.GCHandleToObject(instance)).Donations = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((TroopDonateRequest)GCHandledObjects.GCHandleToObject(instance)).RecipientId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((TroopDonateRequest)GCHandledObjects.GCHandleToObject(instance)).RequestId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDonateRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
