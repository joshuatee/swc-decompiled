using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;

namespace StaRTS.Main.Story
{
	public class DebugTriggerReactor : ITriggerReactor
	{
		public void SatisfyTrigger(IStoryTrigger trigger)
		{
			Service.Get<Logger>().Debug("Test Story Trigger Satisfied!");
		}
	}
}
