using StaRTS.DataStructures;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
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
	public class SpecialAttackDeployer : AbstractDeployer
	{
		private SpecialAttackTypeVO currentSpecialAttackType;

		public SpecialAttackDeployer()
		{
			this.currentSpecialAttackType = null;
		}

		public bool EnterPlacementMode(SpecialAttackTypeVO specialAttackType)
		{
			if (this.currentSpecialAttackType != null)
			{
				this.ExitMode();
			}
			this.currentSpecialAttackType = specialAttackType;
			this.EnterMode();
			return true;
		}

		public override void ExitMode()
		{
			base.ExitMode();
			this.currentSpecialAttackType = null;
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
				this.DeploySpecialAttack();
			}
			return EatResponse.NotEaten;
		}

		private SpecialAttack DeploySpecialAttack()
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
			if (battleController.GetPlayerDeployableSpecialAttackCount(this.currentSpecialAttackType.Uid) == 0)
			{
				Service.Get<EventManager>().SendEvent(EventId.TroopNotPlacedInvalidTroop, this.currentWorldPosition);
				return null;
			}
			if (this.currentSpecialAttackType != null)
			{
				TeamType teamType = TeamType.Attacker;
				if (battleController.GetCurrentBattle().Type == BattleType.PveDefend)
				{
					teamType = TeamType.Defender;
				}
				SpecialAttack specialAttack = Service.Get<SpecialAttackController>().DeploySpecialAttack(this.currentSpecialAttackType, teamType, this.currentWorldPosition);
				if (specialAttack != null)
				{
					IntPosition boardPosition = Units.WorldToBoardIntPosition(this.currentWorldPosition);
					battleController.OnSpecialAttackDeployed(this.currentSpecialAttackType.Uid, teamType, boardPosition);
					Service.Get<EventManager>().SendEvent(EventId.SpecialAttackDeployed, specialAttack);
					return specialAttack;
				}
			}
			return null;
		}

		protected internal SpecialAttackDeployer(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackDeployer)GCHandledObjects.GCHandleToObject(instance)).DeploySpecialAttack());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackDeployer)GCHandledObjects.GCHandleToObject(instance)).EnterPlacementMode((SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SpecialAttackDeployer)GCHandledObjects.GCHandleToObject(instance)).ExitMode();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackDeployer)GCHandledObjects.GCHandleToObject(instance)).OnDrag((GameObject)GCHandledObjects.GCHandleToObject(*args), *(*(IntPtr*)(args + 1)), *(*(IntPtr*)(args + 2))));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackDeployer)GCHandledObjects.GCHandleToObject(instance)).OnPress((GameObject)GCHandledObjects.GCHandleToObject(*args), *(*(IntPtr*)(args + 1)), *(*(IntPtr*)(args + 2))));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackDeployer)GCHandledObjects.GCHandleToObject(instance)).OnRelease());
		}
	}
}
