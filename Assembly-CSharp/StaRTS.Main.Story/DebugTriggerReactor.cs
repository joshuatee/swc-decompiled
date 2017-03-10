using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story
{
	public class DebugTriggerReactor : ITriggerReactor
	{
		public DebugTriggerReactor()
		{
		}

		public void SatisfyTrigger(IStoryTrigger trigger)
		{
			Service.Get<StaRTSLogger>().Debug("Test Story Trigger Satisfied!");
		}

		protected internal DebugTriggerReactor(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DebugTriggerReactor)GCHandledObjects.GCHandleToObject(instance)).SatisfyTrigger((IStoryTrigger)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
