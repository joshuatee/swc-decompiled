using StaRTS.Main.Models;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.GameStates
{
	public class BattleEndState : IGameState, IState
	{
		private bool isSquadWarBattle;

		public BattleEndState(bool isSquadWarBattle)
		{
			this.isSquadWarBattle = isSquadWarBattle;
		}

		public virtual void OnEnter()
		{
			Service.Get<BattleRecordController>().EndRecord();
			AlertScreen highestLevelScreen = Service.Get<ScreenController>().GetHighestLevelScreen<AlertScreen>();
			if (highestLevelScreen != null && !highestLevelScreen.IsFatal && !highestLevelScreen.IsAlwaysOnTop)
			{
				highestLevelScreen.Close(null);
			}
			this.ShowBattleEndScreen(false);
			Service.Get<UXController>().HUD.ConfigureControls(new HudConfig(new string[0]));
			if (Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress)
			{
				Service.Get<UXController>().MiscElementsManager.HideFingerAnimation();
			}
		}

		public void OnExit(IState nextState)
		{
			Service.Get<BattleController>().Clear();
		}

		protected void ShowBattleEndScreen(bool isReplay)
		{
			if (this.isSquadWarBattle)
			{
				Service.Get<ScreenController>().AddScreen(new SquadWarBattleEndScreen(isReplay), true, false);
				return;
			}
			Service.Get<ScreenController>().AddScreen(new BattleEndScreen(isReplay), true, false);
		}

		public bool CanUpdateHomeContracts()
		{
			return false;
		}

		protected internal BattleEndState(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleEndState)GCHandledObjects.GCHandleToObject(instance)).CanUpdateHomeContracts());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((BattleEndState)GCHandledObjects.GCHandleToObject(instance)).OnEnter();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((BattleEndState)GCHandledObjects.GCHandleToObject(instance)).OnExit((IState)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((BattleEndState)GCHandledObjects.GCHandleToObject(instance)).ShowBattleEndScreen(*(sbyte*)args != 0);
			return -1L;
		}
	}
}
