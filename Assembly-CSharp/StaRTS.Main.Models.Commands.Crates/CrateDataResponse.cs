using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Crates
{
	public class CrateDataResponse : AbstractResponse
	{
		public CrateData CrateDataTO
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
			this.CrateDataTO = new CrateData();
			this.CrateDataTO.FromObject(dictionary);
			return this;
		}

		public CrateDataResponse()
		{
		}

		protected internal CrateDataResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateDataResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CrateDataResponse)GCHandledObjects.GCHandleToObject(instance)).CrateDataTO);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((CrateDataResponse)GCHandledObjects.GCHandleToObject(instance)).CrateDataTO = (CrateData)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
