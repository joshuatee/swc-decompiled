using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Crates
{
	public class OpenCrateResponse : AbstractResponse
	{
		public List<string> SupplyIDs
		{
			get;
			private set;
		}

		public OpenCrateResponse()
		{
			this.SupplyIDs = new List<string>();
		}

		public override ISerializable FromObject(object obj)
		{
			List<object> list = obj as List<object>;
			if (list != null)
			{
				this.SupplyIDs.Clear();
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					this.SupplyIDs.Add(Convert.ToString(list[i]));
				}
			}
			return this;
		}

		protected internal OpenCrateResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((OpenCrateResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((OpenCrateResponse)GCHandledObjects.GCHandleToObject(instance)).SupplyIDs);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((OpenCrateResponse)GCHandledObjects.GCHandleToObject(instance)).SupplyIDs = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
