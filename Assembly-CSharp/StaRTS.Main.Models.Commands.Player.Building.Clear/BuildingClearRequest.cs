using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Building.Clear
{
	public class BuildingClearRequest : PlayerIdChecksumRequest
	{
		public string InstanceId
		{
			get;
			set;
		}

		public bool PayWithHardCurrency
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer startedSerializer = base.GetStartedSerializer();
			startedSerializer.AddString("instanceId", this.InstanceId);
			startedSerializer.AddBool("payWithHardCurrency", this.PayWithHardCurrency);
			return startedSerializer.End().ToString();
		}

		public BuildingClearRequest()
		{
		}

		protected internal BuildingClearRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingClearRequest)GCHandledObjects.GCHandleToObject(instance)).InstanceId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingClearRequest)GCHandledObjects.GCHandleToObject(instance)).PayWithHardCurrency);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((BuildingClearRequest)GCHandledObjects.GCHandleToObject(instance)).InstanceId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((BuildingClearRequest)GCHandledObjects.GCHandleToObject(instance)).PayWithHardCurrency = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingClearRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
