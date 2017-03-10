using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers.ServerMessages
{
	public class ContractFinishedMessage : AbstractMessage
	{
		private List<ContractTO> FinishedContracts;

		public override object MessageCookie
		{
			get
			{
				return this.FinishedContracts;
			}
		}

		public override EventId MessageEventId
		{
			get
			{
				return EventId.ContractsCompletedWhileOffline;
			}
		}

		public override ISerializable FromObject(object obj)
		{
			this.FinishedContracts = new List<ContractTO>();
			List<object> list = (List<object>)obj;
			for (int i = 0; i < list.Count; i++)
			{
				Dictionary<string, object> dictionary = list[i] as Dictionary<string, object>;
				List<object> list2 = dictionary["message"] as List<object>;
				for (int j = 0; j < list2.Count; j++)
				{
					ContractTO item = new ContractTO().FromObject(list2[j]) as ContractTO;
					this.FinishedContracts.Add(item);
				}
			}
			return this;
		}

		public ContractFinishedMessage()
		{
		}

		protected internal ContractFinishedMessage(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContractFinishedMessage)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContractFinishedMessage)GCHandledObjects.GCHandleToObject(instance)).MessageCookie);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ContractFinishedMessage)GCHandledObjects.GCHandleToObject(instance)).MessageEventId);
		}
	}
}
