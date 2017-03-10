using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace Source.StaRTS.Main.Models.Commands.Player
{
	public class GeneratePlayerRequest : AbstractRequest
	{
		public string LocalePreference
		{
			get;
			set;
		}

		public string Network
		{
			get;
			set;
		}

		public string ViewNetwork
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			if (this.LocalePreference != null)
			{
				serializer.AddString("locale", this.LocalePreference);
			}
			return serializer.End().ToString();
		}

		public GeneratePlayerRequest()
		{
		}

		protected internal GeneratePlayerRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GeneratePlayerRequest)GCHandledObjects.GCHandleToObject(instance)).LocalePreference);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GeneratePlayerRequest)GCHandledObjects.GCHandleToObject(instance)).Network);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GeneratePlayerRequest)GCHandledObjects.GCHandleToObject(instance)).ViewNetwork);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((GeneratePlayerRequest)GCHandledObjects.GCHandleToObject(instance)).LocalePreference = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((GeneratePlayerRequest)GCHandledObjects.GCHandleToObject(instance)).Network = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((GeneratePlayerRequest)GCHandledObjects.GCHandleToObject(instance)).ViewNetwork = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GeneratePlayerRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
