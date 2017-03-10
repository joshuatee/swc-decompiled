using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Squads
{
	public class SquadInvite : ISerializable
	{
		public string SquadId
		{
			get;
			set;
		}

		public string SenderId
		{
			get;
			set;
		}

		public string SenderName
		{
			get;
			set;
		}

		public string ToJson()
		{
			return "{}";
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null)
			{
				return this;
			}
			if (dictionary.ContainsKey("guildId"))
			{
				this.SquadId = (dictionary["guildId"] as string);
			}
			if (dictionary.ContainsKey("senderId"))
			{
				this.SenderId = (dictionary["senderId"] as string);
			}
			if (dictionary.ContainsKey("senderName"))
			{
				this.SenderName = (dictionary["senderName"] as string);
			}
			return this;
		}

		public SquadInvite()
		{
		}

		protected internal SquadInvite(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadInvite)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadInvite)GCHandledObjects.GCHandleToObject(instance)).SenderId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadInvite)GCHandledObjects.GCHandleToObject(instance)).SenderName);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadInvite)GCHandledObjects.GCHandleToObject(instance)).SquadId);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SquadInvite)GCHandledObjects.GCHandleToObject(instance)).SenderId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SquadInvite)GCHandledObjects.GCHandleToObject(instance)).SenderName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SquadInvite)GCHandledObjects.GCHandleToObject(instance)).SquadId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadInvite)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
