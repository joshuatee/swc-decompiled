using StaRTS.DataStructures;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UserInput;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.World.Deploying
{
	public class TroopDeployer : AbstractDeployer
	{
		private const float TROOP_SPAWN_RATE_SEC = 0.2f;

		private const float AUTO_SPAWN_DELAY_SEC = 0.25f;

		private const float MULTI_TAP_DELAY_SEC = 0.5f;

		private TroopTypeVO currentTroopType;

		private uint autoSpawnTimerID;

		private uint pressHoldTimerID;

		private uint multiTapTimerID;

		public static readonly IntPosition[] OFFSETS = new IntPosition[]
		{
			new IntPosition(0, 0),
			new IntPosition(-1, 0),
			new IntPosition(0, -1),
			new IntPosition(1, 0),
			new IntPosition(0, 1)
		};

		private int currentOffsetIndex;

		public TroopDeployer()
		{
			this.currentTroopType = null;
			this.autoSpawnTimerID = 0u;
			this.pressHoldTimerID = 0u;
			this.multiTapTimerID = 0u;
			this.currentOffsetIndex = 0;
		}

		public bool EnterPlacementMode(TroopTypeVO troopType)
		{
			if (this.currentTroopType != null)
			{
				this.ExitMode();
			}
			this.currentTroopType = troopType;
			this.EnterMode();
			return true;
		}

		public override void ExitMode()
		{
			this.KillAutoSpawnTimer();
			this.KillPressHoldTimer();
			this.currentTroopType = null;
			base.ExitMode();
		}

		public override EatResponse OnPress(GameObject target, Vector2 screenPosition, Vector3 groundPosition)
		{
			this.CreatePressHoldTimer();
			return EatResponse.NotEaten;
		}

		public override EatResponse OnDrag(GameObject target, Vector2 screenPosition, Vector3 groundPosition)
		{
			if (this.dragged)
			{
				this.KillPressHoldTimer();
			}
			if (this.currentTroopType != null && this.autoSpawnTimerID != 0u)
			{
				this.currentWorldPosition = groundPosition;
				return EatResponse.Eaten;
			}
			return EatResponse.NotEaten;
		}

		public override EatResponse OnRelease()
		{
			if (base.IsNotDraggedAndReleasingOwnPress() && this.autoSpawnTimerID == 0u)
			{
				this.DeployTroop();
			}
			this.KillAutoSpawnTimer();
			this.KillPressHoldTimer();
			return EatResponse.NotEaten;
		}

		private SmartEntity DeployTroop()
		{
			if (Service.Get<SimTimeEngine>().IsPaused())
			{
				return null;
			}
			BattleController battleController = Service.Get<BattleController>();
			if (battleController.BattleEndProcessing)
			{
				return null;
			}
			if (battleController.GetPlayerDeployableTroopCount(this.currentTroopType.Uid) == 0)
			{
				Service.Get<EventManager>().SendEvent(EventId.TroopNotPlacedInvalidTroop, this.currentWorldPosition);
				return null;
			}
			if (this.currentTroopType == null)
			{
				return null;
			}
			TeamType teamType = TeamType.Attacker;
			if (battleController.GetCurrentBattle().Type == BattleType.PveDefend)
			{
				teamType = TeamType.Defender;
			}
			IntPosition intPosition = Units.WorldToBoardIntDeployPosition(this.currentWorldPosition);
			intPosition += TroopDeployer.OFFSETS[this.currentOffsetIndex] * this.currentTroopType.AutoSpawnSpreadingScale;
			intPosition = Units.NormalizeDeployPosition(intPosition);
			int num = this.currentOffsetIndex + 1;
			this.currentOffsetIndex = num;
			if (num == TroopDeployer.OFFSETS.Length)
			{
				this.currentOffsetIndex = 0;
			}
			SmartEntity smartEntity = Service.Get<TroopController>().SpawnTroop(this.currentTroopType, teamType, intPosition, (teamType == TeamType.Defender) ? TroopSpawnMode.LeashedToBuilding : TroopSpawnMode.Unleashed, true);
			if (smartEntity == null)
			{
				return null;
			}
			base.PlaySpawnEffect(smartEntity);
			battleController.OnTroopDeployed(this.currentTroopType.Uid, teamType, intPosition);
			Service.Get<EventManager>().SendEvent(EventId.TroopDeployed, smartEntity);
			return smartEntity;
		}

		private void CreateAutoSpawnTimer()
		{
			this.KillAutoSpawnTimer();
			this.autoSpawnTimerID = Service.Get<ViewTimerManager>().CreateViewTimer(0.2f * this.currentTroopType.AutoSpawnRateScale, true, new TimerDelegate(this.OnAutoSpawnTimer), null);
		}

		private void KillAutoSpawnTimer()
		{
			if (this.autoSpawnTimerID != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.autoSpawnTimerID);
				this.autoSpawnTimerID = 0u;
			}
		}

		public void OnAutoSpawnTimer(uint id, object cookie)
		{
			if (id == this.autoSpawnTimerID)
			{
				this.DeployTroop();
			}
		}

		private void CreatePressHoldTimer()
		{
			this.KillPressHoldTimer();
			if (!this.IsMultiTap())
			{
				this.currentOffsetIndex = 0;
			}
			this.KillMultitapTimer();
			this.multiTapTimerID = Service.Get<ViewTimerManager>().CreateViewTimer(0.5f, false, new TimerDelegate(this.OnMultitapTimer), null);
			this.pressHoldTimerID = Service.Get<ViewTimerManager>().CreateViewTimer(0.25f, false, new TimerDelegate(this.OnPressHoldTimer), null);
		}

		private void KillPressHoldTimer()
		{
			if (this.pressHoldTimerID != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.pressHoldTimerID);
				this.pressHoldTimerID = 0u;
			}
		}

		private void KillMultitapTimer()
		{
			if (this.multiTapTimerID != 0u)
			{
				Service.Get<ViewTimerManager>().KillViewTimer(this.multiTapTimerID);
				this.multiTapTimerID = 0u;
			}
		}

		public void OnPressHoldTimer(uint id, object cookie)
		{
			if (id == this.pressHoldTimerID && !this.dragged)
			{
				this.DeployTroop();
				this.CreateAutoSpawnTimer();
				this.KillPressHoldTimer();
				Service.Get<UserInputManager>().ReleaseSubordinates(this, UserInputLayer.World, 0);
			}
		}

		public void OnMultitapTimer(uint id, object cookie)
		{
			this.multiTapTimerID = 0u;
		}

		private bool IsMultiTap()
		{
			return this.multiTapTimerID > 0u;
		}

		protected internal TroopDeployer(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TroopDeployer)GCHandledObjects.GCHandleToObject(instance)).CreateAutoSpawnTimer();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TroopDeployer)GCHandledObjects.GCHandleToObject(instance)).CreatePressHoldTimer();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDeployer)GCHandledObjects.GCHandleToObject(instance)).DeployTroop());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDeployer)GCHandledObjects.GCHandleToObject(instance)).EnterPlacementMode((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((TroopDeployer)GCHandledObjects.GCHandleToObject(instance)).ExitMode();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDeployer)GCHandledObjects.GCHandleToObject(instance)).IsMultiTap());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((TroopDeployer)GCHandledObjects.GCHandleToObject(instance)).KillAutoSpawnTimer();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((TroopDeployer)GCHandledObjects.GCHandleToObject(instance)).KillMultitapTimer();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((TroopDeployer)GCHandledObjects.GCHandleToObject(instance)).KillPressHoldTimer();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDeployer)GCHandledObjects.GCHandleToObject(instance)).OnDrag((GameObject)GCHandledObjects.GCHandleToObject(*args), *(*(IntPtr*)(args + 1)), *(*(IntPtr*)(args + 2))));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDeployer)GCHandledObjects.GCHandleToObject(instance)).OnPress((GameObject)GCHandledObjects.GCHandleToObject(*args), *(*(IntPtr*)(args + 1)), *(*(IntPtr*)(args + 2))));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopDeployer)GCHandledObjects.GCHandleToObject(instance)).OnRelease());
		}
	}
}
