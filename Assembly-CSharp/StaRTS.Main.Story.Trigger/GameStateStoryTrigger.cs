using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Story.Trigger
{
	public class GameStateStoryTrigger : AbstractStoryTrigger, IEventObserver
	{
		private const int STATE_ARG = 0;

		private const string HOME_STATE = "home";

		private const string EDIT_STATE = "edit";

		private const string BATTLE_START_STATE = "battle_start";

		private const string BATTLE_PLAY_STATE = "battle_play";

		private const string BATTLE_END_STATE = "battle_end";

		public GameStateStoryTrigger(StoryTriggerVO vo, ITriggerReactor parent) : base(vo, parent)
		{
		}

		public override void Activate()
		{
			Service.Get<EventManager>().RegisterObserver(this, EventId.GameStateChanged, EventPriority.Default);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.GameStateChanged)
			{
				IState currentState = Service.Get<GameStateMachine>().CurrentState;
				if (this.EvaluateState(currentState, this.prepareArgs[0]))
				{
					this.parent.SatisfyTrigger(this);
				}
			}
			return EatResponse.NotEaten;
		}

		private bool EvaluateState(IState state, string desiredState)
		{
			if (desiredState == "home")
			{
				return state is HomeState;
			}
			if (desiredState == "edit")
			{
				return state is EditBaseState;
			}
			if (desiredState == "battle_start")
			{
				return state is BattleStartState;
			}
			if (desiredState == "battle_end")
			{
				return state is BattleEndState;
			}
			return desiredState == "battle_play" && state is BattlePlayState;
		}

		public override void Destroy()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.GameStateChanged);
			base.Destroy();
		}

		protected internal GameStateStoryTrigger(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((GameStateStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Activate();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((GameStateStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameStateStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).EvaluateState((IState)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameStateStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
