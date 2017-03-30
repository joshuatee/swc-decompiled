using StaRTS.Main.Models.Battle;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UserInput;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;

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
	}
}
