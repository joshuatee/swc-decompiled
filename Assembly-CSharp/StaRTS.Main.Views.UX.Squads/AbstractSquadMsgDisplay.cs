using StaRTS.Main.Models.Squads;
using StaRTS.Main.Views.UX.Elements;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Squads
{
	public abstract class AbstractSquadMsgDisplay
	{
		protected UXTable table;

		public AbstractSquadMsgDisplay(UXTable table)
		{
			this.table = table;
		}

		public void ProcessNewMessages(List<SquadMsg> messages)
		{
			bool flag = false;
			int i = 0;
			int count = messages.Count;
			while (i < count)
			{
				bool flag2 = this.ProcessMessage(messages[i]);
				flag |= flag2;
				i++;
			}
			if (flag)
			{
				this.table.RepositionItems();
			}
		}

		protected virtual bool ProcessMessage(SquadMsg msg)
		{
			return true;
		}

		public virtual void RemoveMessage(SquadMsg msg)
		{
		}

		public virtual void Destroy()
		{
			this.table.Clear();
			this.table = null;
		}

		protected internal AbstractSquadMsgDisplay(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AbstractSquadMsgDisplay)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractSquadMsgDisplay)GCHandledObjects.GCHandleToObject(instance)).ProcessMessage((SquadMsg)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AbstractSquadMsgDisplay)GCHandledObjects.GCHandleToObject(instance)).ProcessNewMessages((List<SquadMsg>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AbstractSquadMsgDisplay)GCHandledObjects.GCHandleToObject(instance)).RemoveMessage((SquadMsg)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
