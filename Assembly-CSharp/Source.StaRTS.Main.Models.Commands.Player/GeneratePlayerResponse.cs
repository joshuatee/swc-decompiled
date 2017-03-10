using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace Source.StaRTS.Main.Models.Commands.Player
{
	public class GeneratePlayerResponse : AbstractResponse
	{
		public string PlayerId
		{
			get;
			private set;
		}

		public string Secret
		{
			get;
			private set;
		}

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			this.PlayerId = (string)dictionary["playerId"];
			this.Secret = (string)dictionary["secret"];
			return this;
		}

		public GeneratePlayerResponse()
		{
		}

		protected internal GeneratePlayerResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GeneratePlayerResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GeneratePlayerResponse)GCHandledObjects.GCHandleToObject(instance)).PlayerId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GeneratePlayerResponse)GCHandledObjects.GCHandleToObject(instance)).Secret);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((GeneratePlayerResponse)GCHandledObjects.GCHandleToObject(instance)).PlayerId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((GeneratePlayerResponse)GCHandledObjects.GCHandleToObject(instance)).Secret = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
