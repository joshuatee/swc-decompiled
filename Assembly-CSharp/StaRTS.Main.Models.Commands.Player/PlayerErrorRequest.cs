using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player
{
	public class PlayerErrorRequest : PlayerIdRequest
	{
		public string Prefix
		{
			get;
			set;
		}

		public string ClientCheckSumString
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("prefix", this.Prefix);
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("clientState", this.ClientCheckSumString);
			return serializer.End().ToString();
		}

		public PlayerErrorRequest()
		{
		}

		protected internal PlayerErrorRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerErrorRequest)GCHandledObjects.GCHandleToObject(instance)).ClientCheckSumString);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerErrorRequest)GCHandledObjects.GCHandleToObject(instance)).Prefix);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((PlayerErrorRequest)GCHandledObjects.GCHandleToObject(instance)).ClientCheckSumString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((PlayerErrorRequest)GCHandledObjects.GCHandleToObject(instance)).Prefix = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlayerErrorRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
