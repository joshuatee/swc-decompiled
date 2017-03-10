using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.State;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.GameStates
{
	public class GameStateMachine : StateMachine
	{
		private bool stateLock;

		public GameStateMachine()
		{
			this.stateLock = false;
			Service.Set<GameStateMachine>(this);
			base.SetLegalTransition(typeof(ApplicationLoadState), typeof(HomeState));
			base.SetLegalTransition(typeof(ApplicationLoadState), typeof(IntroCameraState));
			base.SetLegalTransition(typeof(IntroCameraState), typeof(FueBattleStartState));
			base.SetLegalTransition(typeof(IntroCameraState), typeof(HomeState));
			base.SetLegalTransition(typeof(FueBattleStartState), typeof(BattlePlayState));
			base.SetLegalTransition(typeof(BattleStartState), typeof(BattlePlayState));
			base.SetLegalTransition(typeof(BattlePlayState), typeof(BattleEndState));
			base.SetLegalTransition(typeof(BattleEndPlaybackState), typeof(BattlePlaybackState));
			base.SetLegalTransition(typeof(BattlePlaybackState), typeof(BattleEndPlaybackState));
			base.SetLegalTransition(typeof(BattleEndPlaybackState), typeof(HomeState));
			base.SetLegalTransition(typeof(BattleEndState), typeof(BattlePlaybackState));
			base.SetLegalTransition(typeof(HomeState), typeof(BattlePlaybackState));
			base.SetLegalTransition(typeof(BattleEndState), typeof(HomeState));
			base.SetLegalTransition(typeof(BattleStartState), typeof(HomeState));
			base.SetLegalTransition(typeof(HomeState), typeof(BattleStartState));
			base.SetLegalTransition(typeof(HomeState), typeof(EditBaseState));
			base.SetLegalTransition(typeof(EditBaseState), typeof(HomeState));
			base.SetLegalTransition(typeof(EditBaseState), typeof(BaseLayoutToolState));
			base.SetLegalTransition(typeof(BaseLayoutToolState), typeof(EditBaseState));
			base.SetLegalTransition(typeof(HomeState), typeof(VideoPlayBackState));
			base.SetLegalTransition(typeof(VideoPlayBackState), typeof(HomeState));
			base.SetLegalTransition(typeof(HomeState), typeof(NeighborVisitState));
			base.SetLegalTransition(typeof(NeighborVisitState), typeof(HomeState));
			base.SetLegalTransition(typeof(HomeState), typeof(WarBoardState));
			base.SetLegalTransition(typeof(WarBoardState), typeof(HomeState));
			base.SetLegalTransition(typeof(WarBoardState), typeof(WarBaseEditorState));
			base.SetLegalTransition(typeof(WarBaseEditorState), typeof(WarBoardState));
			base.SetLegalTransition(typeof(HomeState), typeof(GalaxyState));
			base.SetLegalTransition(typeof(GalaxyState), typeof(HomeState));
			base.SetLegalTransition(typeof(GalaxyState), typeof(BattleStartState));
			base.SetLegalTransition(typeof(BattleStartState), typeof(WarBoardState));
		}

		public override bool SetState(IState state)
		{
			if (this.stateLock)
			{
				string text = "NO_CURRENT_STATE";
				if (base.CurrentState != null)
				{
					text = base.CurrentState.GetType().get_Name();
				}
				Service.Get<StaRTSLogger>().Warn(string.Concat(new string[]
				{
					"While in state: ",
					text,
					"Tried to change to ",
					state.GetType().get_Name(),
					" while in middle of another state change. This state change has been ignored."
				}));
				return false;
			}
			this.stateLock = true;
			if (Service.IsSet<EventManager>())
			{
				Service.Get<EventManager>().SendEvent(EventId.GameStateAboutToChange, state);
			}
			bool result = base.SetState(state);
			this.stateLock = false;
			if (Service.IsSet<EventManager>())
			{
				Service.Get<EventManager>().SendEvent(EventId.GameStateChanged, base.PreviousStateType);
			}
			return result;
		}

		protected internal GameStateMachine(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameStateMachine)GCHandledObjects.GCHandleToObject(instance)).SetState((IState)GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
