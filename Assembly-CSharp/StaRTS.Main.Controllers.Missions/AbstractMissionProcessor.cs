using StaRTS.Main.Views.UserInput;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using WinRTBridge;

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
			Service.Get<StaRTSLogger>().ErrorFormat("The mission processor for mission {0} does not have a Start() method defined.", new object[]
			{
				this.parent.MissionVO.Uid
			});
		}

		public virtual void Resume()
		{
			Service.Get<StaRTSLogger>().ErrorFormat("The mission processor for mission {0} does not have a Resume() method defined.", new object[]
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

		protected internal AbstractMissionProcessor(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AbstractMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AbstractMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnCancel();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AbstractMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnFailureHookComplete();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AbstractMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnGoalFailureHookComplete();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AbstractMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnIntroHookComplete();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((AbstractMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnSuccessHookComplete();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((AbstractMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).PauseBattle();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((AbstractMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).Resume();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((AbstractMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).ResumeBattle();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((AbstractMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
