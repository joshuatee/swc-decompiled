using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Controllers.World.Transitions;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils.Core;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class NeighborVisitManager
	{
		private bool processing;

		public GamePlayer NeighborPlayer;

		public List<SquadDonatedTroop> NeighborSquadTroops
		{
			get;
			private set;
		}

		public NeighborVisitManager()
		{
			Service.Set<NeighborVisitManager>(this);
		}

		public void VisitNeighbor(string neighborId)
		{
			if (this.processing)
			{
				return;
			}
			this.processing = true;
			ProcessingScreen.Show();
			VisitNeighborRequest request = new VisitNeighborRequest(neighborId);
			VisitNeighborCommand visitNeighborCommand = new VisitNeighborCommand(request);
			visitNeighborCommand.AddSuccessCallback(new AbstractCommand<VisitNeighborRequest, VisitNeighborResponse>.OnSuccessCallback(this.OnVisitNeighborSuccess));
			visitNeighborCommand.AddFailureCallback(new AbstractCommand<VisitNeighborRequest, VisitNeighborResponse>.OnFailureCallback(this.OnVisitNeighborFailure));
			Service.Get<ServerAPI>().Sync(visitNeighborCommand);
		}

		private void OnVisitNeighborSuccess(VisitNeighborResponse response, object cookie)
		{
			ProcessingScreen.Hide();
			if (response == null)
			{
				this.processing = false;
				return;
			}
			this.NeighborPlayer = new GamePlayer();
			this.NeighborPlayer.PlayerName = response.Name;
			this.NeighborPlayer.Faction = response.Faction;
			this.NeighborPlayer.Map = response.MapData;
			this.NeighborPlayer.Inventory = response.InventoryData;
			this.NeighborPlayer.AttackRating = response.AttackRating;
			this.NeighborPlayer.DefenseRating = response.DefenseRating;
			this.NeighborPlayer.AttacksWon = response.AttacksWon;
			this.NeighborPlayer.DefensesWon = response.DefensesWon;
			this.NeighborPlayer.Squad = response.Squad;
			this.NeighborPlayer.UnlockedLevels = response.UpgradesData;
			this.NeighborSquadTroops = response.SquadTroops;
			NeighborMapDataLoader mapDataLoader = new NeighborMapDataLoader(response);
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			AbstractTransition transition;
			if (currentState is GalaxyState)
			{
				transition = new GalaxyMapToBaseTransition(new NeighborVisitState(), mapDataLoader, new TransitionCompleteDelegate(this.OnTransitionComplete), false, false);
			}
			else if (currentState is WarBoardState)
			{
				transition = new WarboardToWarbaseTransition(new NeighborVisitState(), mapDataLoader, new TransitionCompleteDelegate(this.OnTransitionComplete), false, false);
			}
			else
			{
				transition = new WorldToWorldTransition(new NeighborVisitState(), mapDataLoader, new TransitionCompleteDelegate(this.OnTransitionComplete), false, true);
			}
			Service.Get<WorldTransitioner>().StartTransition(transition);
		}

		private void OnVisitNeighborFailure(uint id, object cookie)
		{
			ProcessingScreen.Hide();
		}

		private void OnTransitionComplete()
		{
			this.processing = false;
		}

		protected internal NeighborVisitManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NeighborVisitManager)GCHandledObjects.GCHandleToObject(instance)).NeighborSquadTroops);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((NeighborVisitManager)GCHandledObjects.GCHandleToObject(instance)).OnTransitionComplete();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((NeighborVisitManager)GCHandledObjects.GCHandleToObject(instance)).OnVisitNeighborSuccess((VisitNeighborResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((NeighborVisitManager)GCHandledObjects.GCHandleToObject(instance)).NeighborSquadTroops = (List<SquadDonatedTroop>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((NeighborVisitManager)GCHandledObjects.GCHandleToObject(instance)).VisitNeighbor(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}
	}
}
