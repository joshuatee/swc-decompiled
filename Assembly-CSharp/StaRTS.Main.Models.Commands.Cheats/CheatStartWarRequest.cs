using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Cheats
{
	public class CheatStartWarRequest : PlayerIdRequest
	{
		public string GuildId
		{
			get;
			private set;
		}

		public CheatStartWarRequest(string guildId)
		{
			base.PlayerId = Service.Get<CurrentPlayer>().PlayerId;
			this.GuildId = guildId;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("guildId", this.GuildId);
			return serializer.End().ToString();
		}

		protected internal CheatStartWarRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatStartWarRequest)GCHandledObjects.GCHandleToObject(instance)).GuildId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((CheatStartWarRequest)GCHandledObjects.GCHandleToObject(instance)).GuildId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CheatStartWarRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
