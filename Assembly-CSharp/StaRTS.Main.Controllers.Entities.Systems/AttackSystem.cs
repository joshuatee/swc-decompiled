using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Nodes;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Entities.Systems
{
	public class AttackSystem : SimSystemBase
	{
		private NodeList<BuffNode> buffNodeList;

		private NodeList<TroopNode> troopNodeList;

		private NodeList<TurretNode> turretNodeList;

		private TroopAttackController troopAttackController;

		private SpecialAttackController specialAttackController;

		private TurretAttackController turretAttackController;

		public override void AddToGame(IGame game)
		{
			EntityController entityController = Service.Get<EntityController>();
			this.buffNodeList = entityController.GetNodeList<BuffNode>();
			this.troopNodeList = entityController.GetNodeList<TroopNode>();
			this.turretNodeList = entityController.GetNodeList<TurretNode>();
			this.troopAttackController = Service.Get<TroopAttackController>();
			this.specialAttackController = Service.Get<SpecialAttackController>();
			this.turretAttackController = Service.Get<TurretAttackController>();
		}

		public override void RemoveFromGame(IGame game)
		{
		}

		protected override void Update(uint dt)
		{
			BuffNode next;
			for (BuffNode buffNode = this.buffNodeList.Head; buffNode != null; buffNode = next)
			{
				next = buffNode.Next;
				buffNode.BuffComp.UpdateBuffs(dt);
			}
			for (TroopNode troopNode = this.troopNodeList.Head; troopNode != null; troopNode = troopNode.Next)
			{
				this.troopAttackController.UpdateTroop((SmartEntity)troopNode.Entity);
			}
			this.specialAttackController.UpdateSpecialAttacks();
			for (TurretNode turretNode = this.turretNodeList.Head; turretNode != null; turretNode = turretNode.Next)
			{
				this.turretAttackController.UpdateTurret((SmartEntity)turretNode.Entity);
			}
		}

		public AttackSystem()
		{
		}

		protected internal AttackSystem(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AttackSystem)GCHandledObjects.GCHandleToObject(instance)).AddToGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AttackSystem)GCHandledObjects.GCHandleToObject(instance)).RemoveFromGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
