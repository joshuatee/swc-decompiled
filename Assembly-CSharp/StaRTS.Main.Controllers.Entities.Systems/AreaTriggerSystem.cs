using Net.RichardLord.Ash.Core;
using StaRTS.DataStructures;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Entities.Systems
{
	public class AreaTriggerSystem : SimSystemBase
	{
		private EntityController entityController;

		private NodeList<OffensiveTroopNode> offensiveTroopNodeList;

		private NodeList<OffensiveHealerNode> offensiveHealerNodeList;

		private SpatialIndexController spatialIndexController;

		private TargetingController targetingController;

		private CombatTriggerManager combatTriggerManager;

		private HashSet<ShooterComponent> reusableResetReevaluateTargetSet;

		public override void AddToGame(IGame game)
		{
			this.entityController = Service.Get<EntityController>();
			this.offensiveTroopNodeList = this.entityController.GetNodeList<OffensiveTroopNode>();
			this.offensiveHealerNodeList = this.entityController.GetNodeList<OffensiveHealerNode>();
			this.spatialIndexController = Service.Get<SpatialIndexController>();
			this.targetingController = Service.Get<TargetingController>();
			this.combatTriggerManager = Service.Get<CombatTriggerManager>();
			this.reusableResetReevaluateTargetSet = new HashSet<ShooterComponent>();
		}

		public override void RemoveFromGame(IGame game)
		{
		}

		private void FindIfTroopIsInRangeOfTrigger(SmartEntity entity)
		{
			TransformComponent transformComp = entity.TransformComp;
			List<ElementPriorityPair<Entity>> areaTriggerBuildingsInRangeOf = this.spatialIndexController.GetAreaTriggerBuildingsInRangeOf(transformComp.CenterGridX(), transformComp.CenterGridZ());
			if (areaTriggerBuildingsInRangeOf != null && areaTriggerBuildingsInRangeOf.Count > 0)
			{
				this.combatTriggerManager.InformAreaTriggerBuildings(areaTriggerBuildingsInRangeOf, entity);
			}
		}

		protected override void Update(uint dt)
		{
			for (OffensiveTroopNode offensiveTroopNode = this.offensiveTroopNodeList.Head; offensiveTroopNode != null; offensiveTroopNode = offensiveTroopNode.Next)
			{
				SmartEntity entity = (SmartEntity)offensiveTroopNode.Entity;
				this.FindIfTroopIsInRangeOfTrigger(entity);
			}
			for (OffensiveHealerNode offensiveHealerNode = this.offensiveHealerNodeList.Head; offensiveHealerNode != null; offensiveHealerNode = offensiveHealerNode.Next)
			{
				SmartEntity entity2 = (SmartEntity)offensiveHealerNode.Entity;
				this.FindIfTroopIsInRangeOfTrigger(entity2);
			}
			this.InformTurretsAboutTroops<OffensiveTroopNode>(this.offensiveTroopNodeList, this.reusableResetReevaluateTargetSet);
			this.InformTurretsAboutTroops<OffensiveHealerNode>(this.offensiveHealerNodeList, this.reusableResetReevaluateTargetSet);
			if (this.reusableResetReevaluateTargetSet.Count > 0)
			{
				foreach (ShooterComponent current in this.reusableResetReevaluateTargetSet)
				{
					current.ReevaluateTarget = false;
				}
				this.reusableResetReevaluateTargetSet.Clear();
			}
		}

		private void InformTurretsAboutTroops<T>(NodeList<T> nodeList, HashSet<ShooterComponent> resetReevaluateTargetSet) where T : Node<T>, new()
		{
			for (T t = nodeList.Head; t != null; t = t.Next)
			{
				SmartEntity smartEntity = (SmartEntity)t.Entity;
				TransformComponent transformComp = smartEntity.TransformComp;
				List<ElementPriorityPair<Entity>> turretsInRangeOf = this.spatialIndexController.GetTurretsInRangeOf(transformComp.CenterGridX(), transformComp.CenterGridZ());
				if (turretsInRangeOf != null && turretsInRangeOf.Count > 0)
				{
					this.targetingController.InformTurretsAboutTroop(turretsInRangeOf, smartEntity, resetReevaluateTargetSet);
				}
			}
		}

		public AreaTriggerSystem()
		{
		}

		protected internal AreaTriggerSystem(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AreaTriggerSystem)GCHandledObjects.GCHandleToObject(instance)).AddToGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AreaTriggerSystem)GCHandledObjects.GCHandleToObject(instance)).FindIfTroopIsInRangeOfTrigger((SmartEntity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AreaTriggerSystem)GCHandledObjects.GCHandleToObject(instance)).RemoveFromGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
