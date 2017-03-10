using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Squads
{
	public class SquadDonatedTroop
	{
		public string TroopUid
		{
			get;
			private set;
		}

		public Dictionary<string, int> SenderAmounts
		{
			get;
			private set;
		}

		public SquadDonatedTroop(string troopUid)
		{
			this.TroopUid = troopUid;
			this.SenderAmounts = new Dictionary<string, int>();
		}

		public void AddSenderAmount(string senderId, int amount)
		{
			if (this.SenderAmounts.ContainsKey(senderId))
			{
				Dictionary<string, int> senderAmounts = this.SenderAmounts;
				senderAmounts[senderId] += amount;
				return;
			}
			this.SenderAmounts.Add(senderId, amount);
		}

		public int GetTotalAmount()
		{
			int num = 0;
			foreach (KeyValuePair<string, int> current in this.SenderAmounts)
			{
				num += current.get_Value();
			}
			return num;
		}

		protected internal SquadDonatedTroop(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SquadDonatedTroop)GCHandledObjects.GCHandleToObject(instance)).AddSenderAmount(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadDonatedTroop)GCHandledObjects.GCHandleToObject(instance)).SenderAmounts);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadDonatedTroop)GCHandledObjects.GCHandleToObject(instance)).TroopUid);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadDonatedTroop)GCHandledObjects.GCHandleToObject(instance)).GetTotalAmount());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SquadDonatedTroop)GCHandledObjects.GCHandleToObject(instance)).SenderAmounts = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SquadDonatedTroop)GCHandledObjects.GCHandleToObject(instance)).TroopUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
