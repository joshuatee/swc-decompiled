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
	}
}
