using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.RUF.RUFTasks
{
	public class AbstractRUFTask
	{
		protected const float LOGIN_NOTIFICATION_TIME_DELAY = 2f;

		public int Priority
		{
			get;
			set;
		}

		public bool ShouldPurgeQueue
		{
			get;
			set;
		}

		public bool ShouldProcess
		{
			get;
			set;
		}

		public bool ShouldPlayFromLoadState
		{
			get;
			set;
		}

		public int PriorityPurgeThreshold
		{
			get;
			set;
		}

		public virtual void Process(bool continueProcessing)
		{
			Service.Get<RUFManager>().ProcessQueue(continueProcessing);
		}

		public AbstractRUFTask()
		{
		}

		protected internal AbstractRUFTask(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractRUFTask)GCHandledObjects.GCHandleToObject(instance)).Priority);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractRUFTask)GCHandledObjects.GCHandleToObject(instance)).PriorityPurgeThreshold);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractRUFTask)GCHandledObjects.GCHandleToObject(instance)).ShouldPlayFromLoadState);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractRUFTask)GCHandledObjects.GCHandleToObject(instance)).ShouldProcess);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractRUFTask)GCHandledObjects.GCHandleToObject(instance)).ShouldPurgeQueue);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((AbstractRUFTask)GCHandledObjects.GCHandleToObject(instance)).Process(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((AbstractRUFTask)GCHandledObjects.GCHandleToObject(instance)).Priority = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((AbstractRUFTask)GCHandledObjects.GCHandleToObject(instance)).PriorityPurgeThreshold = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((AbstractRUFTask)GCHandledObjects.GCHandleToObject(instance)).ShouldPlayFromLoadState = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((AbstractRUFTask)GCHandledObjects.GCHandleToObject(instance)).ShouldProcess = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((AbstractRUFTask)GCHandledObjects.GCHandleToObject(instance)).ShouldPurgeQueue = (*(sbyte*)args != 0);
			return -1L;
		}
	}
}
