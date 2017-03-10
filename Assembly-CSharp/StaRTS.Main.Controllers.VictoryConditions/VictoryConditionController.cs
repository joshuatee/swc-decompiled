using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers.VictoryConditions
{
	public class VictoryConditionController : IConditionParent
	{
		private const string DEFAULT_VICTORY_CONDITION1 = "DestroyHQ";

		private const string DEFAULT_VICTORY_CONDITION2 = "DestroyHalfBuildings";

		private const string DEFAULT_VICTORY_CONDITION3 = "DestroyAllBuildings";

		private List<AbstractCondition> currentVictoryConditions;

		private EventManager events;

		private AbstractCondition failureCondition;

		private IDataController sdc;

		public List<string> Successes;

		public List<string> Failures;

		public ConditionVO FailureConditionVO
		{
			get
			{
				if (this.failureCondition != null)
				{
					return this.failureCondition.GetConditionVo();
				}
				return null;
			}
		}

		public List<AbstractCondition> ActiveConditions
		{
			get
			{
				return this.currentVictoryConditions;
			}
		}

		public VictoryConditionController()
		{
			Service.Set<VictoryConditionController>(this);
			this.events = Service.Get<EventManager>();
			this.sdc = Service.Get<IDataController>();
			this.Successes = new List<string>();
			this.Failures = new List<string>();
		}

		public void ActivateConditionSet(List<ConditionVO> voSet)
		{
			this.CancelCurrentConditions();
			this.Successes = new List<string>();
			this.Failures = new List<string>();
			this.currentVictoryConditions = new List<AbstractCondition>();
			AbstractCondition abstractCondition = null;
			for (int i = 0; i < voSet.Count; i++)
			{
				abstractCondition = ConditionFactory.GenerateCondition(voSet[i], this);
				this.currentVictoryConditions.Add(abstractCondition);
				abstractCondition.Start();
			}
			for (int j = voSet.Count; j < 3; j++)
			{
				abstractCondition = ConditionFactory.GenerateCondition(abstractCondition.GetConditionVo(), this);
				this.currentVictoryConditions.Add(abstractCondition);
				abstractCondition.Start();
			}
		}

		public void ActivateFailureCondition(ConditionVO condition)
		{
			this.failureCondition = ConditionFactory.GenerateCondition(condition, this);
			this.failureCondition.Start();
		}

		public void CancelCurrentConditions()
		{
			if (this.currentVictoryConditions != null)
			{
				for (int i = 0; i < this.currentVictoryConditions.Count; i++)
				{
					this.currentVictoryConditions[i].Destroy();
				}
			}
			this.currentVictoryConditions = null;
			this.Successes.Clear();
			this.Failures.Clear();
			if (this.failureCondition != null)
			{
				this.failureCondition.Destroy();
				this.failureCondition = null;
			}
		}

		public List<ConditionVO> GetDefaultConditions()
		{
			List<ConditionVO> list = new List<ConditionVO>();
			ConditionVO item = this.sdc.Get<ConditionVO>("DestroyHQ");
			list.Add(item);
			item = this.sdc.Get<ConditionVO>("DestroyHalfBuildings");
			list.Add(item);
			item = this.sdc.Get<ConditionVO>("DestroyAllBuildings");
			list.Add(item);
			return list;
		}

		public bool HasCurrentActiveConditions()
		{
			return this.currentVictoryConditions != null;
		}

		public void ChildSatisfied(AbstractCondition child)
		{
			if (child == this.failureCondition)
			{
				return;
			}
			ConditionVO conditionVo = child.GetConditionVo();
			child.Destroy();
			this.Successes.Add(conditionVo.Uid);
			this.events.SendEvent(EventId.VictoryConditionSuccess, conditionVo);
		}

		public void ChildFailed(AbstractCondition child)
		{
			ConditionVO conditionVo = child.GetConditionVo();
			child.Destroy();
			this.Failures.Add(conditionVo.Uid);
			this.events.SendEvent(EventId.VictoryConditionFailure, conditionVo);
		}

		public void ChildUpdated(AbstractCondition child, int delta)
		{
		}

		protected internal VictoryConditionController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((VictoryConditionController)GCHandledObjects.GCHandleToObject(instance)).ActivateConditionSet((List<ConditionVO>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((VictoryConditionController)GCHandledObjects.GCHandleToObject(instance)).ActivateFailureCondition((ConditionVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((VictoryConditionController)GCHandledObjects.GCHandleToObject(instance)).CancelCurrentConditions();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((VictoryConditionController)GCHandledObjects.GCHandleToObject(instance)).ChildFailed((AbstractCondition)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((VictoryConditionController)GCHandledObjects.GCHandleToObject(instance)).ChildSatisfied((AbstractCondition)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((VictoryConditionController)GCHandledObjects.GCHandleToObject(instance)).ChildUpdated((AbstractCondition)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VictoryConditionController)GCHandledObjects.GCHandleToObject(instance)).ActiveConditions);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VictoryConditionController)GCHandledObjects.GCHandleToObject(instance)).FailureConditionVO);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VictoryConditionController)GCHandledObjects.GCHandleToObject(instance)).GetDefaultConditions());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VictoryConditionController)GCHandledObjects.GCHandleToObject(instance)).HasCurrentActiveConditions());
		}
	}
}
