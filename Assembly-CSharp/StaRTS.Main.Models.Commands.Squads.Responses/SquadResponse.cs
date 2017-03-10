using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Responses
{
	public class SquadResponse : AbstractResponse
	{
		public string SquadId
		{
			get;
			private set;
		}

		public object SquadData
		{
			get;
			private set;
		}

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null)
			{
				return this;
			}
			if (dictionary.ContainsKey("id"))
			{
				this.SquadId = Convert.ToString(dictionary["id"], CultureInfo.InvariantCulture);
				this.SquadData = obj;
			}
			return this;
		}

		public SquadResponse()
		{
		}

		protected internal SquadResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadResponse)GCHandledObjects.GCHandleToObject(instance)).SquadData);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadResponse)GCHandledObjects.GCHandleToObject(instance)).SquadId);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SquadResponse)GCHandledObjects.GCHandleToObject(instance)).SquadData = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SquadResponse)GCHandledObjects.GCHandleToObject(instance)).SquadId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
