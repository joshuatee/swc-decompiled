using StaRTS.DataStructures;
using StaRTS.Main.Controllers;
using StaRTS.Main.Utils;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.World.Deploying
{
	public class SquadTroopDeployer : AbstractDeployer
	{
		public bool EnterPlacementMode()
		{
			if (!Service.Get<SquadTroopAttackController>().Spawning)
			{
				this.EnterMode();
				return true;
			}
			return false;
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
			if (base.IsNotDraggedAndReleasingOwnPress() && !Service.Get<SquadTroopAttackController>().Spawning && !Service.Get<SimTimeEngine>().IsPaused() && !Service.Get<BattleController>().BattleEndProcessing && Service.Get<BattleController>().CanPlayerDeploySquadTroops())
			{
				IntPosition boardPos = Units.WorldToBoardIntDeployPosition(this.currentWorldPosition);
				Service.Get<SquadTroopAttackController>().DeploySquadTroops(boardPos);
			}
			return EatResponse.NotEaten;
		}

		public SquadTroopDeployer()
		{
		}

		protected internal SquadTroopDeployer(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadTroopDeployer)GCHandledObjects.GCHandleToObject(instance)).EnterPlacementMode());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadTroopDeployer)GCHandledObjects.GCHandleToObject(instance)).OnDrag((GameObject)GCHandledObjects.GCHandleToObject(*args), *(*(IntPtr*)(args + 1)), *(*(IntPtr*)(args + 2))));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadTroopDeployer)GCHandledObjects.GCHandleToObject(instance)).OnPress((GameObject)GCHandledObjects.GCHandleToObject(*args), *(*(IntPtr*)(args + 1)), *(*(IntPtr*)(args + 2))));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadTroopDeployer)GCHandledObjects.GCHandleToObject(instance)).OnRelease());
		}
	}
}
