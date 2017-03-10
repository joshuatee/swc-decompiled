using Net.RichardLord.Ash.Core;
using StaRTS.DataStructures;
using StaRTS.GameBoard;
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
	public class HeroDeployer : AbstractDeployer
	{
		public TroopTypeVO CurrentTroopType
		{
			get;
			private set;
		}

		public bool CanDeploy
		{
			get;
			private set;
		}

		public HeroDeployer()
		{
			this.CurrentTroopType = null;
			this.CanDeploy = true;
		}

		public bool EnterMode(TroopTypeVO troopType)
		{
			if (this.CurrentTroopType != null)
			{
				this.ExitMode();
			}
			this.CurrentTroopType = troopType;
			this.EnterMode();
			return true;
		}

		public override void ExitMode()
		{
			this.CanDeploy = true;
			base.ExitMode();
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
				this.DeployHero(this.CurrentTroopType);
			}
			return EatResponse.NotEaten;
		}

		private SmartEntity DeployHero(TroopTypeVO troopType)
		{
			if (Service.Get<SimTimeEngine>().IsPaused())
			{
				return null;
			}
			if (!this.CanDeploy)
			{
				return null;
			}
			BattleController battleController = Service.Get<BattleController>();
			if (battleController.BattleEndProcessing)
			{
				return null;
			}
			if (battleController.GetPlayerDeployableHeroCount(troopType.Uid) == 0)
			{
				this.CanDeploy = false;
				Service.Get<EventManager>().SendEvent(EventId.TroopNotPlacedInvalidTroop, this.currentWorldPosition);
				return null;
			}
			TeamType teamType = TeamType.Attacker;
			if (battleController.GetCurrentBattle().Type == BattleType.PveDefend)
			{
				teamType = TeamType.Defender;
			}
			IntPosition intPosition = Units.WorldToBoardIntDeployPosition(this.currentWorldPosition);
			TroopController troopController = Service.Get<TroopController>();
			if (battleController.GetCurrentBattle().IsRaidDefense())
			{
				Entity entity = null;
				BoardCell<Entity> boardCell = null;
				if (!troopController.FinalizeSafeBoardPosition(troopType, ref entity, ref intPosition, ref boardCell, TeamType.Attacker, TroopSpawnMode.Unleashed, false))
				{
					Service.Get<EventManager>().SendEvent(EventId.TroopNotPlacedInvalidArea, intPosition);
					return null;
				}
			}
			SmartEntity smartEntity = troopController.SpawnHero(troopType, teamType, intPosition, false);
			if (smartEntity != null)
			{
				base.PlaySpawnEffect(smartEntity);
				this.CanDeploy = false;
				battleController.OnHeroDeployed(troopType.Uid, teamType, intPosition);
				Service.Get<EventManager>().SendEvent(EventId.HeroDeployed, smartEntity);
			}
			return smartEntity;
		}

		protected internal HeroDeployer(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HeroDeployer)GCHandledObjects.GCHandleToObject(instance)).DeployHero((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HeroDeployer)GCHandledObjects.GCHandleToObject(instance)).EnterMode((TroopTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((HeroDeployer)GCHandledObjects.GCHandleToObject(instance)).ExitMode();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HeroDeployer)GCHandledObjects.GCHandleToObject(instance)).CanDeploy);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HeroDeployer)GCHandledObjects.GCHandleToObject(instance)).CurrentTroopType);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HeroDeployer)GCHandledObjects.GCHandleToObject(instance)).OnDrag((GameObject)GCHandledObjects.GCHandleToObject(*args), *(*(IntPtr*)(args + 1)), *(*(IntPtr*)(args + 2))));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HeroDeployer)GCHandledObjects.GCHandleToObject(instance)).OnPress((GameObject)GCHandledObjects.GCHandleToObject(*args), *(*(IntPtr*)(args + 1)), *(*(IntPtr*)(args + 2))));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HeroDeployer)GCHandledObjects.GCHandleToObject(instance)).OnRelease());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((HeroDeployer)GCHandledObjects.GCHandleToObject(instance)).CanDeploy = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((HeroDeployer)GCHandledObjects.GCHandleToObject(instance)).CurrentTroopType = (TroopTypeVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
