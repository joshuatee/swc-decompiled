using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Trigger
{
	public class ClusterORDERStoryTrigger : AbstractStoryTrigger, ITriggerReactor
	{
		private const int TRIGGER_LIST_ARG = 0;

		private string[] uids;

		private int childIndex;

		private IStoryTrigger currentTrigger;

		private IDataController sdc;

		public ClusterORDERStoryTrigger(StoryTriggerVO vo, ITriggerReactor parent) : base(vo, parent)
		{
		}

		public override void Activate()
		{
			this.sdc = Service.Get<IDataController>();
			this.uids = this.prepareArgs[0].Split(new char[]
			{
				','
			});
			this.childIndex = -1;
			this.ActivateNextChild();
		}

		private void ActivateNextChild()
		{
			this.childIndex++;
			if (this.childIndex < this.uids.Length)
			{
				StoryTriggerVO vo = this.sdc.Get<StoryTriggerVO>(this.uids[this.childIndex]);
				this.currentTrigger = StoryTriggerFactory.GenerateStoryTrigger(vo, this);
				this.currentTrigger.Activate();
				return;
			}
			this.parent.SatisfyTrigger(this);
		}

		public void SatisfyTrigger(IStoryTrigger trigger)
		{
			this.ActivateNextChild();
		}

		public override void Destroy()
		{
			if (this.currentTrigger != null)
			{
				this.currentTrigger.Destroy();
				this.currentTrigger = null;
			}
			this.uids = null;
			base.Destroy();
		}

		protected internal ClusterORDERStoryTrigger(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ClusterORDERStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Activate();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ClusterORDERStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).ActivateNextChild();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ClusterORDERStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ClusterORDERStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).SatisfyTrigger((IStoryTrigger)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
