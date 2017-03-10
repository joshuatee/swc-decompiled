using StaRTS.Externals.Manimal;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class LimitedEditionItemController : IEventObserver
	{
		private uint nextUpdateTimer;

		public List<LimitedEditionItemVO> ValidLEIs
		{
			get;
			private set;
		}

		public LimitedEditionItemController()
		{
			this.ValidLEIs = new List<LimitedEditionItemVO>();
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.GameStateChanged);
			Service.Set<LimitedEditionItemController>(this);
		}

		public void UpdateValidLEIs()
		{
			this.ValidLEIs.Clear();
			IDataController dataController = Service.Get<IDataController>();
			CurrentPlayer player = Service.Get<CurrentPlayer>();
			int serverTime = (int)Service.Get<ServerAPI>().ServerTime;
			Dictionary<string, LimitedEditionItemVO>.ValueCollection all = dataController.GetAll<LimitedEditionItemVO>();
			int num = -1;
			foreach (LimitedEditionItemVO current in all)
			{
				int num2 = current.EndTime - serverTime;
				if (num2 >= 0)
				{
					if (num == -1 || num2 < num)
					{
						num = num2;
					}
					int num3 = current.StartTime - serverTime;
					if (num3 > 0)
					{
						if (num == -1 || num3 < num)
						{
							num = num3;
						}
					}
					else if (LimitedEditionItemController.IsValidForPlayer(current, player))
					{
						this.ValidLEIs.Add(current);
					}
				}
			}
			if (num != -1 && (long)num < 432000L)
			{
				this.nextUpdateTimer = Service.Get<ViewTimerManager>().CreateViewTimer((float)num, false, new TimerDelegate(this.UpdateValidLEIs), null);
			}
		}

		public void UpdateValidLEIs(uint id, object cookie)
		{
			this.UpdateValidLEIs();
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.GameStateChanged)
			{
				if (id == EventId.ContractCompleted)
				{
					this.UpdateValidLEIs();
				}
			}
			else
			{
				IGameState gameState = (IGameState)Service.Get<GameStateMachine>().CurrentState;
				if (gameState != null && gameState.CanUpdateHomeContracts())
				{
					this.StartUpdating();
				}
				else
				{
					this.StopUpdating();
				}
			}
			return EatResponse.NotEaten;
		}

		private void StartUpdating()
		{
			this.UpdateValidLEIs();
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.RegisterObserver(this, EventId.ContractCompleted);
		}

		private void StopUpdating()
		{
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.UnregisterObserver(this, EventId.ContractCompleted);
			if (this.nextUpdateTimer != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.nextUpdateTimer);
				this.nextUpdateTimer = 0u;
			}
		}

		private static bool IsValidForPlayer(LimitedEditionItemVO vo, CurrentPlayer player)
		{
			return vo.Faction == player.Faction && CrateUtils.AllConditionsMet(vo.AudienceConditions);
		}

		protected internal LimitedEditionItemController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LimitedEditionItemController)GCHandledObjects.GCHandleToObject(instance)).ValidLEIs);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(LimitedEditionItemController.IsValidForPlayer((LimitedEditionItemVO)GCHandledObjects.GCHandleToObject(*args), (CurrentPlayer)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LimitedEditionItemController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((LimitedEditionItemController)GCHandledObjects.GCHandleToObject(instance)).ValidLEIs = (List<LimitedEditionItemVO>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((LimitedEditionItemController)GCHandledObjects.GCHandleToObject(instance)).StartUpdating();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((LimitedEditionItemController)GCHandledObjects.GCHandleToObject(instance)).StopUpdating();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((LimitedEditionItemController)GCHandledObjects.GCHandleToObject(instance)).UpdateValidLEIs();
			return -1L;
		}
	}
}
