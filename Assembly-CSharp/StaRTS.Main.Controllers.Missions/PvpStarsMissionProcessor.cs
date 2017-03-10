using StaRTS.Main.Models.Battle;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UserInput;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Missions
{
	public class PvpStarsMissionProcessor : AbstractMissionProcessor, IEventObserver
	{
		private EventManager eventManager;

		public PvpStarsMissionProcessor(MissionConductor parent) : base(parent)
		{
			this.eventManager = Service.Get<EventManager>();
		}

		public override void Start()
		{
			if (this.parent.OnIntroHook())
			{
				Service.Get<UserInputInhibitor>().DenyAll();
			}
			this.eventManager.RegisterObserver(this, EventId.PvpBattleStarting, EventPriority.Default);
		}

		public override void Resume()
		{
			this.eventManager.RegisterObserver(this, EventId.PvpBattleStarting, EventPriority.Default);
		}

		public override void OnIntroHookComplete()
		{
			Service.Get<UserInputInhibitor>().AllowAll();
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.BattleEndFullyProcessed)
			{
				if (id == EventId.PvpBattleStarting)
				{
					this.eventManager.RegisterObserver(this, EventId.BattleEndFullyProcessed, EventPriority.Default);
					Service.Get<BattleController>().GetCurrentBattle().PvpMissionUid = this.parent.MissionVO.Uid;
				}
			}
			else
			{
				this.eventManager.UnregisterObserver(this, EventId.BattleEndFullyProcessed);
				CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
				if (currentBattle.Won)
				{
					this.parent.CompleteMission(currentBattle.EarnedStars);
					if (this.parent.OnSuccessHook())
					{
						base.PauseBattle();
					}
				}
				else if (this.parent.OnFailureHook())
				{
					base.PauseBattle();
				}
			}
			return EatResponse.NotEaten;
		}

		public override void OnSuccessHookComplete()
		{
			base.ResumeBattle();
		}

		public override void OnFailureHookComplete()
		{
			base.ResumeBattle();
		}

		public override void OnGoalFailureHookComplete()
		{
			base.ResumeBattle();
		}

		public override void OnCancel()
		{
			this.Destroy();
		}

		public override void Destroy()
		{
			this.eventManager.UnregisterObserver(this, EventId.PvpBattleStarting);
			this.eventManager.UnregisterObserver(this, EventId.BattleEndFullyProcessed);
		}

		protected internal PvpStarsMissionProcessor(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PvpStarsMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PvpStarsMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnCancel();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PvpStarsMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((PvpStarsMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnFailureHookComplete();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((PvpStarsMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnGoalFailureHookComplete();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((PvpStarsMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnIntroHookComplete();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((PvpStarsMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).OnSuccessHookComplete();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((PvpStarsMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).Resume();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((PvpStarsMissionProcessor)GCHandledObjects.GCHandleToObject(instance)).Start();
			return -1L;
		}
	}
}
