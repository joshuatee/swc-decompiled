using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class PromoCodeTestRequest : PlayerIdRequest
	{
		private string productId;

		public void SetProductId(string value)
		{
			this.productId = value;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("productId", this.productId);
			return serializer.End().ToString();
		}

		public PromoCodeTestRequest()
		{
		}

		protected internal PromoCodeTestRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PromoCodeTestRequest)GCHandledObjects.GCHandleToObject(instance)).SetProductId(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PromoCodeTestRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
