using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Externals.Manimal.TransferObjects.Request
{
	public class PlanetIdRequest : PlayerIdRequest
	{
		public string PlanetId
		{
			get;
			set;
		}

		public PlanetIdRequest(string planetId)
		{
			this.PlanetId = planetId;
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("planetId", this.PlanetId);
			return serializer.End().ToString();
		}

		protected internal PlanetIdRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetIdRequest)GCHandledObjects.GCHandleToObject(instance)).PlanetId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PlanetIdRequest)GCHandledObjects.GCHandleToObject(instance)).PlanetId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetIdRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
