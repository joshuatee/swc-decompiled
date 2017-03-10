using StaRTS.DataStructures;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.World.Deploying
{
	public class ChampionDeployer : AbstractDeployer
	{
		private TroopTypeVO currentTroopType;

		public ChampionDeployer()
		{
			this.currentTroopType = null;
		}

		public bool EnterMode(TroopTypeVO troopType)
		{
			if (this.currentTroopType != null)
			{
				this.ExitMode();
			}
			this.currentTroopType = troopType;
			this.EnterMode();
			return true;
		}

		public override EatResponse OnPress(GameObject target, Vector2 screenPosition, Vector3 groundPosition)
		{
			return EatResponse.NotEaten;
		}

		public override EatResponse OnDrag(GameObject target, Vector2 screenPosition, Vector3 groundPosition)
		{
			return EatResponse.NotEaten;
		}

		public override EatResponse OnRelease()
		{
			if (base.IsNotDraggedAndReleasingOwnPress())
			{
				this.DeployChampion(this.currentTroopType);
			}
			return EatResponse.NotEaten;
		}

		private SmartEntity DeployChampion(TroopTypeVO troopType)
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
			if (battleController.GetPlayerDeployableChampionCount(troopType.Uid) == 0)
			{
				Service.Get<EventManager>().SendEvent(EventId.TroopNotPlacedInvalidTroop, this.currentWorldPosition);
				return null;
			}
			TeamType teamType = TeamType.Attacker;
			if (battleController.GetCurrentBattle().Type == BattleType.PveDefend)
			{
				teamType = TeamType.Defender;
			}
			IntPosition boardPosition = Units.WorldToBoardIntDeployPosition(this.currentWorldPosition);
			SmartEntity smartEntity = Service.Get<TroopController>().SpawnChampion(troopType, teamType, boardPosition);
			if (smartEntity != null)
			{
				base.PlaySpawnEffect(smartEntity);
				battleController.OnChampionDeployed(troopType.Uid, teamType, boardPosition);
				Service.Get<EventManager>().SendEvent(EventId.ChampionDeployed, smartEntity);
			}
			return smartEntity;
		}

		protected internal ChampionDeployer(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionDeployer)GCHandledObjects.GCHandleToObject(instance)).DeployChampion((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionDeployer)GCHandledObjects.GCHandleToObject(instance)).EnterMode((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionDeployer)GCHandledObjects.GCHandleToObject(instance)).OnDrag((GameObject)GCHandledObjects.GCHandleToObject(*args), *(*(IntPtr*)(args + 1)), *(*(IntPtr*)(args + 2))));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionDeployer)GCHandledObjects.GCHandleToObject(instance)).OnPress((GameObject)GCHandledObjects.GCHandleToObject(*args), *(*(IntPtr*)(args + 1)), *(*(IntPtr*)(args + 2))));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ChampionDeployer)GCHandledObjects.GCHandleToObject(instance)).OnRelease());
		}
	}
}
