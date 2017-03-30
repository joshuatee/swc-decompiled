using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Story.Actions
{
	public class ShowHologramStoryAction : AbstractStoryAction, IEventObserver
	{
		private const int CHARACTER_ASSET_ARG = 0;

		private const int GREET_ARG = 1;

		private const string GREET = "greet";

		private const float SECONDS_DELAY_AFTER_TRANSITION = 1.5f;

		private bool greet;

		private uint timerId;

		public string Character
		{
			get
			{
				return this.prepareArgs[0];
			}
		}

		public ShowHologramStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(new int[]
			{
				1,
				2
			});
			if (this.prepareArgs.Length > 1)
			{
				this.greet = (this.prepareArgs[1] == "greet");
			}
			try
			{
				Service.Get<IDataController>().Get<CharacterVO>(this.Character);
				this.parent.ChildPrepared(this);
			}
			catch (KeyNotFoundException ex)
			{
				Service.Get<Logger>().ErrorFormat("Cannot find character {0}, in Story Action {1}", new object[]
				{
					this.Character,
					this.vo.Uid
				});
				throw ex;
			}
		}

		public override void Execute()
		{
			if (!Service.IsSet<WorldTransitioner>() || Service.Get<WorldTransitioner>().IsTransitioning())
			{
				Service.Get<EventManager>().RegisterObserver(this, EventId.WorldInTransitionComplete, EventPriority.Default);
				return;
			}
			if (Service.Get<GameStateMachine>().CurrentState is ApplicationLoadState)
			{
				Service.Get<EventManager>().RegisterObserver(this, EventId.GameStateChanged, EventPriority.Default);
				return;
			}
			base.Execute();
			if (Service.Get<HoloController>().CharacterAlreadyShowing(this.prepareArgs[0]))
			{
				this.parent.ChildComplete(this);
				return;
			}
			Service.Get<EventManager>().RegisterObserver(this, EventId.ShowHologramComplete, EventPriority.Default);
			Service.Get<EventManager>().SendEvent(EventId.ShowHologram, this);
		}

		private void ExecuteAfterDelay(uint id, object cookie)
		{
			this.timerId = 0u;
			this.Execute();
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.WorldInTransitionComplete)
			{
				if (id != EventId.GameStateChanged)
				{
					if (id == EventId.ShowHologramComplete)
					{
						Service.Get<EventManager>().UnregisterObserver(this, EventId.ShowHologramComplete);
						this.TryPlayGreet();
						this.parent.ChildComplete(this);
					}
				}
				else if (!(Service.Get<GameStateMachine>().CurrentState is IntroCameraState))
				{
					Service.Get<EventManager>().UnregisterObserver(this, EventId.GameStateChanged);
					this.timerId = Service.Get<ViewTimerManager>().CreateViewTimer(1.5f, false, new TimerDelegate(this.ExecuteAfterDelay), null);
				}
			}
			else
			{
				Service.Get<EventManager>().UnregisterObserver(this, EventId.WorldInTransitionComplete);
				this.timerId = Service.Get<ViewTimerManager>().CreateViewTimer(1.5f, false, new TimerDelegate(this.ExecuteAfterDelay), null);
			}
			return EatResponse.NotEaten;
		}

		private void TryPlayGreet()
		{
			if (this.greet)
			{
				CharacterVO characterVO = Service.Get<IDataController>().Get<CharacterVO>(this.Character);
				if (characterVO.Greets != null)
				{
					int num = Service.Get<Rand>().ViewRangeInt(0, characterVO.Greets.Length);
					Service.Get<EventManager>().SendEvent(EventId.PlayHoloGreet, characterVO.Greets[num]);
				}
			}
		}

		public override void Destroy()
		{
			if (this.timerId != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.timerId);
			}
		}
	}
}
