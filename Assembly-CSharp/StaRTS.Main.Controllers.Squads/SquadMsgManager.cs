using StaRTS.Main.Models.Squads;
using StaRTS.Main.Views.UX.Squads;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Squads
{
	public class SquadMsgManager
	{
		private SquadController controller;

		private List<SquadMsg> msgs;

		private Dictionary<string, SquadMsg> msgsByIds;

		private List<AbstractSquadMsgDisplay> observers;

		private List<SquadMsg> msgsToProcess;

		private Dictionary<string, List<SquadMsg>> linkedMsgs;

		public SquadMsgManager(SquadController controller)
		{
			this.controller = controller;
			this.msgs = new List<SquadMsg>();
			this.msgsByIds = new Dictionary<string, SquadMsg>();
			this.observers = new List<AbstractSquadMsgDisplay>();
			this.msgsToProcess = new List<SquadMsg>();
			this.linkedMsgs = new Dictionary<string, List<SquadMsg>>();
		}

		public void Enable()
		{
			this.controller.ServerManager.AddSquadMsgCallback(new SquadController.SquadMsgsCallback(this.OnNewSquadMsgs));
		}

		public void RegisterObserver(AbstractSquadMsgDisplay observer)
		{
			if (this.observers.Contains(observer))
			{
				return;
			}
			this.observers.Add(observer);
		}

		public void UnregisterObserver(AbstractSquadMsgDisplay observer)
		{
			if (this.observers.Contains(observer))
			{
				this.observers.Remove(observer);
			}
		}

		private void OnNewSquadMsgs(List<SquadMsg> msgs)
		{
			int count = msgs.Count;
			if (count == 0)
			{
				return;
			}
			this.msgsToProcess.Clear();
			for (int i = 0; i < count; i++)
			{
				SquadMsg msg = msgs[i];
				if ((string.IsNullOrEmpty(msg.NotifId) || !this.msgsByIds.ContainsKey(msg.NotifId)) && this.msgs.Find((SquadMsg m) => m.Type == SquadMsgType.Chat && msg.Type == SquadMsgType.Chat && m.TimeSent == msg.TimeSent && ((m.OwnerData == null && msg.OwnerData == null) || (m.OwnerData.PlayerId == null && msg.OwnerData.PlayerId == null) || m.OwnerData.PlayerId.CompareTo(msg.OwnerData.PlayerId) == 0) && ((m.ChatData == null && msg.ChatData == null) || (m.ChatData.Message == null && msg.ChatData.Message == null) || (m.ChatData.Message.CompareTo(msg.ChatData.Message) == 0 && m.ChatData.Tag.CompareTo(msg.ChatData.Tag) == 0 && m.ChatData.Time.CompareTo(msg.ChatData.Time) == 0))) == null)
				{
					this.msgs.Add(msg);
					this.msgsToProcess.Add(msg);
					SquadMsg parentMsg = this.GetParentMsg(msg);
					if (parentMsg != null)
					{
						List<SquadMsg> list;
						if (this.linkedMsgs.ContainsKey(parentMsg.NotifId))
						{
							list = this.linkedMsgs[parentMsg.NotifId];
						}
						else
						{
							list = new List<SquadMsg>();
							this.linkedMsgs.Add(parentMsg.NotifId, list);
						}
						list.Add(msg);
					}
					if (!string.IsNullOrEmpty(msg.NotifId))
					{
						this.msgsByIds.Add(msg.NotifId, msg);
					}
				}
			}
			if (this.msgsToProcess.Count > 0)
			{
				this.msgs.Sort(new Comparison<SquadMsg>(this.SortMsg));
				this.msgsToProcess.Sort(new Comparison<SquadMsg>(this.SortMsg));
			}
			this.TrimMessages(this.msgs, this.msgsToProcess);
			this.controller.OnNewSquadMsgsReceived(this.msgsToProcess);
			int j = 0;
			int count2 = this.observers.Count;
			while (j < count2)
			{
				this.observers[j].ProcessNewMessages(this.msgsToProcess);
				j++;
			}
		}

		private SquadMsg GetParentMsg(SquadMsg msg)
		{
			SquadMsg result = null;
			if (msg.Type == SquadMsgType.TroopDonation && msg.DonationData != null && !string.IsNullOrEmpty(msg.DonationData.RequestId) && this.msgsByIds.ContainsKey(msg.DonationData.RequestId))
			{
				result = this.msgsByIds[msg.DonationData.RequestId];
			}
			return result;
		}

		private void TrimMessages(List<SquadMsg> msgs, List<SquadMsg> msgsToProcess)
		{
			List<SquadMsg> list = new List<SquadMsg>();
			for (int i = msgs.Count - 1 - this.controller.MessageLimit; i >= 0; i--)
			{
				SquadMsg squadMsg = msgs[i];
				if (squadMsg.Type != SquadMsgType.TroopDonation)
				{
					list.Add(squadMsg);
					if (!string.IsNullOrEmpty(squadMsg.NotifId) && this.linkedMsgs.ContainsKey(squadMsg.NotifId))
					{
						List<SquadMsg> list2 = this.linkedMsgs[squadMsg.NotifId];
						if (list2 != null)
						{
							list.AddRange(list2);
						}
					}
				}
			}
			int j = 0;
			int count = list.Count;
			while (j < count)
			{
				SquadMsg squadMsg2 = list[j];
				this.RemoveMsg(squadMsg2, false);
				msgsToProcess.Remove(squadMsg2);
				j++;
			}
		}

		public List<SquadMsg> GetExistingMessages()
		{
			return this.msgs;
		}

		private int SortMsg(SquadMsg a, SquadMsg b)
		{
			if (a.TimeSent < b.TimeSent)
			{
				return -1;
			}
			if (a.TimeSent > b.TimeSent)
			{
				return 1;
			}
			if (a.DonationData != null && b.DonationData == null)
			{
				return 1;
			}
			if (a.DonationData == null && b.DonationData != null)
			{
				return -1;
			}
			return 0;
		}

		public SquadMsg GetMsgById(string id)
		{
			if (!this.msgsByIds.ContainsKey(id))
			{
				return null;
			}
			return this.msgsByIds[id];
		}

		public void RemoveMsgsByType(string playerId, SquadMsgType[] types)
		{
			if (types == null)
			{
				return;
			}
			for (int i = this.msgs.Count - 1; i >= 0; i--)
			{
				SquadMsg squadMsg = this.msgs[i];
				if (squadMsg.OwnerData != null && squadMsg.OwnerData.PlayerId == playerId)
				{
					int j = 0;
					int num = types.Length;
					while (j < num)
					{
						if (squadMsg.Type == types[j])
						{
							this.RemoveMsg(squadMsg, true);
							break;
						}
						j++;
					}
				}
			}
		}

		private void RemoveMsg(SquadMsg msg, bool notifyObservers)
		{
			this.msgs.Remove(msg);
			if (!string.IsNullOrEmpty(msg.NotifId))
			{
				this.msgsByIds.Remove(msg.NotifId);
				this.linkedMsgs.Remove(msg.NotifId);
			}
			if (notifyObservers)
			{
				int i = 0;
				int count = this.observers.Count;
				while (i < count)
				{
					this.observers[i].RemoveMessage(msg);
					i++;
				}
			}
		}

		public void ClearAllMsgs()
		{
			this.msgs.Clear();
			this.msgsByIds.Clear();
			this.msgsToProcess.Clear();
			this.linkedMsgs.Clear();
		}

		public void Destroy()
		{
			this.ClearAllMsgs();
			this.controller = null;
			this.msgs = null;
			this.msgsByIds = null;
			this.observers.Clear();
			this.observers = null;
			this.msgsToProcess = null;
			this.linkedMsgs = null;
		}

		protected internal SquadMsgManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SquadMsgManager)GCHandledObjects.GCHandleToObject(instance)).ClearAllMsgs();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SquadMsgManager)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SquadMsgManager)GCHandledObjects.GCHandleToObject(instance)).Enable();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMsgManager)GCHandledObjects.GCHandleToObject(instance)).GetExistingMessages());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMsgManager)GCHandledObjects.GCHandleToObject(instance)).GetMsgById(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMsgManager)GCHandledObjects.GCHandleToObject(instance)).GetParentMsg((SquadMsg)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SquadMsgManager)GCHandledObjects.GCHandleToObject(instance)).OnNewSquadMsgs((List<SquadMsg>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SquadMsgManager)GCHandledObjects.GCHandleToObject(instance)).RegisterObserver((AbstractSquadMsgDisplay)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((SquadMsgManager)GCHandledObjects.GCHandleToObject(instance)).RemoveMsg((SquadMsg)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((SquadMsgManager)GCHandledObjects.GCHandleToObject(instance)).RemoveMsgsByType(Marshal.PtrToStringUni(*(IntPtr*)args), (SquadMsgType[])GCHandledObjects.GCHandleToPinnedArrayObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMsgManager)GCHandledObjects.GCHandleToObject(instance)).SortMsg((SquadMsg)GCHandledObjects.GCHandleToObject(*args), (SquadMsg)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((SquadMsgManager)GCHandledObjects.GCHandleToObject(instance)).TrimMessages((List<SquadMsg>)GCHandledObjects.GCHandleToObject(*args), (List<SquadMsg>)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((SquadMsgManager)GCHandledObjects.GCHandleToObject(instance)).UnregisterObserver((AbstractSquadMsgDisplay)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
