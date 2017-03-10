using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Holonet
{
	public class HolonetGetMessagesResponse : AbstractResponse
	{
		public List<TransmissionVO> MessageVOs
		{
			get;
			private set;
		}

		public HolonetGetMessagesResponse()
		{
		}

		public override ISerializable FromObject(object obj)
		{
			this.MessageVOs = new List<TransmissionVO>();
			List<object> list = obj as List<object>;
			if (list != null)
			{
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					this.MessageVOs.Add(TransmissionVO.CreateFromServerObject(list[i]));
				}
			}
			return this;
		}

		protected internal HolonetGetMessagesResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HolonetGetMessagesResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HolonetGetMessagesResponse)GCHandledObjects.GCHandleToObject(instance)).MessageVOs);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((HolonetGetMessagesResponse)GCHandledObjects.GCHandleToObject(instance)).MessageVOs = (List<TransmissionVO>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
