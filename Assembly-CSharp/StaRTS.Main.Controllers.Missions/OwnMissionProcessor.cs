using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.Planets;
using StaRTS.Main.Controllers.VictoryConditions;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Missions
{
	public class OwnMissionProcessor : AbstractMissionProcessor, IConditionParent
	{
		private int conditionCount;

		private int successes;

		private List<AbstractCondition> conditions;

		public OwnMissionProcessor(MissionConductor parent) : base(parent)
		{
		}

		public override void Start()
		{
			PlanetDetailsScreen highestLevelScreen = Service.Get<ScreenController>().GetHighestLevelScreen<PlanetDetailsScreen>();
			if (highestLevelScreen != null)
			{
				highestLevelScreen.Close(null);
			}
			if (Service.Get<GameStateMachine>().CurrentState is GalaxyState)
			{
				Service.Get<GalaxyViewController>().GoToHome();
			}
			if (this.parent.OnIntroHook())
			{
				Service.Get<UserInputInhibitor>().DenyAll();
				return;
			}
			this.StartCounting();
		}

		public override void Resume()
		{
			this.StartCounting();
		}

		public override void OnIntroHookComplete()
		{
			Service.Get<UserInputInhibitor>().AllowAll();
			this.StartCounting();
		}

		private void StartCounting()
		{
			this.successes = 0;
			this.conditionCount = this.parent.MissionVO.Conditions.Count;
			this.conditions = new List<AbstractCondition>();
			for (int i = 0; i < this.conditionCount; i++)
			{
				AbstractCondition abstractCondition = ConditionFactory.GenerateCondition(this.parent.MissionVO.Conditions[i], this);
				this.conditions.Add(abstractCondition);
				abstractCondition.Start();
			}
		}

		public void ChildSatisfied(AbstractCondition child)
		{
			this.successes++;
			if (this.successes >= this.conditionCount)
			{
				this.parent.CompleteMission(3);
				this.parent.OnSuccessHook();
			}
		}

		public void ChildFailed(AbstractCondition child)
		{
		}

		public void ChildUpdated(AbstractCondition child, int delta)
		{
		}

		public override void Destroy()
		{
			for (int i = 0; i < this.conditions.Count; i++)
			{
				this.conditions[i].Destroy();
			}
			this.conditions.Clear();
			this.conditions = null;
		}

		protected internal OwnMissionProcessor(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((OwnMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).ChildFailed((AbstractCondition)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((OwnMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).ChildSatisfied((AbstractCondition)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((OwnMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).ChildUpdated((AbstractCondition)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((OwnMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((OwnMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnIntroHookComplete();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((OwnMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).Resume();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((OwnMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((OwnMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).StartCounting();
			return -1L;
		}
	}
}
