using System;
using WinRTBridge;

namespace StaRTS.Main.Story
{
	public class DebugStoryReactor : IStoryReactor
	{
		public DebugStoryReactor()
		{
		}

		public void ChildPrepared(IStoryAction child)
		{
			child.Execute();
		}

		public void ChildComplete(IStoryAction child)
		{
		}

		protected internal DebugStoryReactor(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DebugStoryReactor)GCHandledObjects.GCHandleToObject(instance)).ChildComplete((IStoryAction)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DebugStoryReactor)GCHandledObjects.GCHandleToObject(instance)).ChildPrepared((IStoryAction)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
