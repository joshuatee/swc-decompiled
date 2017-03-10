using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.VictoryConditions
{
	public class AbstractCondition
	{
		protected ConditionVO vo;

		protected IConditionParent parent;

		protected EventManager events;

		protected string[] prepareArgs;

		public AbstractCondition(ConditionVO vo, IConditionParent parent)
		{
			this.events = Service.Get<EventManager>();
			this.vo = vo;
			this.parent = parent;
			if (!string.IsNullOrEmpty(vo.PrepareString))
			{
				this.prepareArgs = vo.PrepareString.Split(new char[]
				{
					'|'
				});
				return;
			}
			this.prepareArgs = new string[0];
		}

		public virtual void Destroy()
		{
		}

		public ConditionVO GetConditionVo()
		{
			return this.vo;
		}

		public virtual bool IsConditionSatisfied()
		{
			return false;
		}

		public virtual void GetProgress(out int current, out int total)
		{
			current = 0;
			total = 1;
		}

		public virtual void Start()
		{
		}

		protected internal AbstractCondition(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AbstractCondition)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCondition)GCHandledObjects.GCHandleToObject(instance)).GetConditionVo());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCondition)GCHandledObjects.GCHandleToObject(instance)).IsConditionSatisfied());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AbstractCondition)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
