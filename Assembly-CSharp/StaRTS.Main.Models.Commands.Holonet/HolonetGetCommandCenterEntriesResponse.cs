using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Holonet
{
	public class HolonetGetCommandCenterEntriesResponse : AbstractResponse
	{
		public List<CommandCenterVO> CCVOs
		{
			get;
			private set;
		}

		public HolonetGetCommandCenterEntriesResponse()
		{
		}

		public override ISerializable FromObject(object obj)
		{
			this.CCVOs = new List<CommandCenterVO>();
			List<object> list = obj as List<object>;
			if (list != null)
			{
				int count = list.Count;
				IDataController dataController = Service.Get<IDataController>();
				for (int i = 0; i < count; i++)
				{
					string text = list[i] as string;
					if (!string.IsNullOrEmpty(text))
					{
						this.CCVOs.Add(dataController.Get<CommandCenterVO>(text));
					}
				}
			}
			return this;
		}

		protected internal HolonetGetCommandCenterEntriesResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HolonetGetCommandCenterEntriesResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HolonetGetCommandCenterEntriesResponse)GCHandledObjects.GCHandleToObject(instance)).CCVOs);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((HolonetGetCommandCenterEntriesResponse)GCHandledObjects.GCHandleToObject(instance)).CCVOs = (List<CommandCenterVO>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
