using StaRTS.Main.Views.UserInput;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;

namespace StaRTS.Main.Controllers.Missions
{
	public class AbstractMissionProcessor
	{
		protected MissionConductor parent;

		public AbstractMissionProcessor(MissionConductor parent)
		{
			this.parent = parent;
		}

		public virtual void Start()
		{
			Service.Get<Logger>().ErrorFormat("The mission processor for mission {0} does not have a Start() method defined.", new object[]
			{
				this.parent.MissionVO.Uid
			});
		}

		public virtual void Resume()
		{
			Service.Get<Logger>().ErrorFormat("The mission processor for mission {0} does not have a Resume() method defined.", new object[]
			{
				this.parent.MissionVO.Uid
			});
		}

		public virtual void OnIntroHookComplete()
		{
		}

		public virtual void OnSuccessHookComplete()
		{
		}

		public virtual void OnFailureHookComplete()
		{
		}

		public virtual void OnGoalFailureHookComplete()
		{
		}

		public virtual void OnCancel()
		{
		}

		public virtual void Destroy()
		{
		}

		protected void PauseBattle()
		{
			Service.Get<SimTimeEngine>().ScaleTime(0u);
			Service.Get<UserInputInhibitor>().DenyAll();
		}

		protected void ResumeBattle()
		{
			Service.Get<SimTimeEngine>().ScaleTime(1u);
			Service.Get<UserInputInhibitor>().AllowAll();
		}
	}
}
