using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player
{
	public class MoneyReceiptVerifyRequest : PlayerIdRequest
	{
		public string VendorKey
		{
			get;
			set;
		}

		public string Receipt
		{
			get;
			set;
		}

		public Dictionary<string, string> ExtraParams
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("receipt", this.Receipt);
			serializer.AddString("vendorKey", this.VendorKey);
			if (this.ExtraParams != null)
			{
				serializer.AddDictionary<string>("extraParams", this.ExtraParams);
			}
			serializer.End();
			return serializer.ToString();
		}

		public MoneyReceiptVerifyRequest()
		{
		}

		protected internal MoneyReceiptVerifyRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MoneyReceiptVerifyRequest)GCHandledObjects.GCHandleToObject(instance)).ExtraParams);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MoneyReceiptVerifyRequest)GCHandledObjects.GCHandleToObject(instance)).Receipt);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MoneyReceiptVerifyRequest)GCHandledObjects.GCHandleToObject(instance)).VendorKey);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((MoneyReceiptVerifyRequest)GCHandledObjects.GCHandleToObject(instance)).ExtraParams = (Dictionary<string, string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((MoneyReceiptVerifyRequest)GCHandledObjects.GCHandleToObject(instance)).Receipt = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((MoneyReceiptVerifyRequest)GCHandledObjects.GCHandleToObject(instance)).VendorKey = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MoneyReceiptVerifyRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
