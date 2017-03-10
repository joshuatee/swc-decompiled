using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Requests
{
	public class GetSquadNotifsRequest : PlayerIdRequest
	{
		public uint Since
		{
			get;
			set;
		}

		public string BattleVersion
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.Add<uint>("since", this.Since);
			serializer.AddString("battleVersion", this.BattleVersion);
			return serializer.End().ToString();
		}

		public GetSquadNotifsRequest()
		{
		}

		protected internal GetSquadNotifsRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetSquadNotifsRequest)GCHandledObjects.GCHandleToObject(instance)).BattleVersion);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((GetSquadNotifsRequest)GCHandledObjects.GCHandleToObject(instance)).BattleVersion = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetSquadNotifsRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
