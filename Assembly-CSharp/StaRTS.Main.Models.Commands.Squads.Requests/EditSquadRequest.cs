using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Requests
{
	public class EditSquadRequest : PlayerIdRequest
	{
		public bool OpenSquad
		{
			get;
			set;
		}

		public int MinTrophy
		{
			get;
			set;
		}

		public string Icon
		{
			get;
			set;
		}

		public string Desc
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddBool("openEnrollment", this.OpenSquad);
			serializer.Add<int>("minScoreAtEnrollment", this.MinTrophy);
			serializer.AddString("icon", this.Icon);
			serializer.AddString("description", this.Desc);
			return serializer.End().ToString();
		}

		public EditSquadRequest()
		{
		}

		protected internal EditSquadRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EditSquadRequest)GCHandledObjects.GCHandleToObject(instance)).Desc);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EditSquadRequest)GCHandledObjects.GCHandleToObject(instance)).Icon);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EditSquadRequest)GCHandledObjects.GCHandleToObject(instance)).MinTrophy);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EditSquadRequest)GCHandledObjects.GCHandleToObject(instance)).OpenSquad);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((EditSquadRequest)GCHandledObjects.GCHandleToObject(instance)).Desc = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((EditSquadRequest)GCHandledObjects.GCHandleToObject(instance)).Icon = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((EditSquadRequest)GCHandledObjects.GCHandleToObject(instance)).MinTrophy = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((EditSquadRequest)GCHandledObjects.GCHandleToObject(instance)).OpenSquad = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EditSquadRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
