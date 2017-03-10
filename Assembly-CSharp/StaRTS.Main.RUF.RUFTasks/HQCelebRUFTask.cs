using System;
using WinRTBridge;

namespace StaRTS.Main.RUF.RUFTasks
{
	public class HQCelebRUFTask : AbstractRUFTask
	{
		public HQCelebRUFTask()
		{
			base.Priority = 80;
			base.ShouldProcess = true;
			base.ShouldPurgeQueue = true;
		}

		public override void Process(bool continueProcessing)
		{
			bool arg_06_0 = base.ShouldProcess;
			base.Process(continueProcessing);
		}

		protected internal HQCelebRUFTask(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((HQCelebRUFTask)GCHandledObjects.GCHandleToObject(instance)).Process(*(sbyte*)args != 0);
			return -1L;
		}
	}
}
