using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Entities.Systems
{
	public class TargetingSystem : SimSystemBase
	{
		private NodeList<OffensiveTroopNode> offensiveTroopNodeList;

		private NodeList<DefensiveTroopNode> defensiveTroopNodeList;

		private NodeList<TurretNode> turretNodeList;

		private int flip;

		private TargetingController targetingController;

		public override void AddToGame(IGame game)
		{
			this.targetingController = Service.Get<TargetingController>();
			EntityController entityController = Service.Get<EntityController>();
			this.offensiveTroopNodeList = entityController.GetNodeList<OffensiveTroopNode>();
			this.defensiveTroopNodeList = entityController.GetNodeList<DefensiveTroopNode>();
			this.turretNodeList = entityController.GetNodeList<TurretNode>();
			this.flip = 0;
		}

		public override void RemoveFromGame(IGame game)
		{
		}

		protected override void Update(uint dt)
		{
			this.targetingController.Update(ref this.flip, new TargetingController.UpdateTarget(this.UpdateOffensiveTroopTarget), new TargetingController.UpdateTarget(this.UpdateDefensiveTroopTarget), new TargetingController.UpdateTarget(this.UpdateOffensiveTroopPeriodicUpdate));
			for (TurretNode turretNode = this.turretNodeList.Head; turretNode != null; turretNode = turretNode.Next)
			{
				this.targetingController.StopSearchIfTargetFound(turretNode.ShooterComp);
			}
		}

		private void OnTargetingDone(SmartEntity entity)
		{
			TroopComponent troopComp = entity.ShooterComp.Target.TroopComp;
			if (troopComp != null)
			{
				TroopComponent expr_15 = troopComp;
				int targetCount = expr_15.TargetCount;
				expr_15.TargetCount = targetCount + 1;
			}
		}

		private void UpdateOffensiveTroopTarget(ref int numTargetingDone)
		{
			this.targetingController.UpdateNodes<OffensiveTroopNode>(this.offensiveTroopNodeList, ref numTargetingDone, new TargetingController.OnTargetingDone(this.OnTargetingDone), false);
		}

		private void UpdateDefensiveTroopTarget(ref int numTargetingDone)
		{
			this.targetingController.UpdateNodes<DefensiveTroopNode>(this.defensiveTroopNodeList, ref numTargetingDone, new TargetingController.OnTargetingDone(this.OnTargetingDone), false);
		}

		private void UpdateOffensiveTroopPeriodicUpdate(ref int numTargetingDone)
		{
			this.targetingController.UpdateNodes<OffensiveTroopNode>(this.offensiveTroopNodeList, ref numTargetingDone, new TargetingController.OnTargetingDone(this.OnTargetingDone), true);
		}

		public TargetingSystem()
		{
		}

		protected internal TargetingSystem(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TargetingSystem)GCHandledObjects.GCHandleToObject(instance)).AddToGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TargetingSystem)GCHandledObjects.GCHandleToObject(instance)).OnTargetingDone((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TargetingSystem)GCHandledObjects.GCHandleToObject(instance)).RemoveFromGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
