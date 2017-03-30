using Net.RichardLord.Ash.Core;
using StaRTS.DataStructures;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Models
{
	public class SpatialIndex
	{
		private PriorityList<SmartEntity> nearnessToBuildings;

		private List<ElementPriorityPair<Entity>> turretsInRange;

		private List<ElementPriorityPair<Entity>> areaTriggerBuildingsInRange;

		public bool AlreadyScannedBuildingsToAttack
		{
			get;
			set;
		}

		public bool AlreadyScannedTurretsInRange
		{
			get;
			set;
		}

		public bool AlreadyScannedAreaTriggerBuildingsInRange
		{
			get;
			set;
		}

		public SpatialIndex()
		{
			this.nearnessToBuildings = new PriorityList<SmartEntity>();
			this.turretsInRange = new List<ElementPriorityPair<Entity>>();
			this.areaTriggerBuildingsInRange = new List<ElementPriorityPair<Entity>>();
			this.AlreadyScannedBuildingsToAttack = false;
			this.AlreadyScannedTurretsInRange = false;
			this.AlreadyScannedAreaTriggerBuildingsInRange = false;
		}

		public void ResetTurretScanedFlag()
		{
			this.AlreadyScannedTurretsInRange = false;
		}

		public void AddBuildingsToAttack(SmartEntity entity, int nearness)
		{
			this.nearnessToBuildings.Add(entity, nearness);
		}

		public void AddTurretsInRangeOf(Entity entity, int distanceSquared, int nearness)
		{
			ShooterComponent shooterComponent = entity.Get<ShooterComponent>();
			if (shooterComponent == null)
			{
				return;
			}
			if (Service.Get<ShooterController>().InRange(distanceSquared, shooterComponent))
			{
				this.turretsInRange.Add(new ElementPriorityPair<Entity>(entity, nearness));
			}
		}

		public void AddAreaTriggerBuildingsInRangeOf(Entity entity, int distanceSquared, int nearness)
		{
			AreaTriggerComponent areaTriggerComponent = entity.Get<AreaTriggerComponent>();
			if (areaTriggerComponent == null)
			{
				return;
			}
			if ((long)distanceSquared <= (long)((ulong)areaTriggerComponent.RangeSquared))
			{
				this.areaTriggerBuildingsInRange.Add(new ElementPriorityPair<Entity>(entity, nearness));
			}
		}

		public List<ElementPriorityPair<Entity>> GetTurretsInRangeOf()
		{
			return this.turretsInRange;
		}

		public List<ElementPriorityPair<Entity>> GetArareaTriggerBuildingsInRange()
		{
			return this.areaTriggerBuildingsInRange;
		}

		public PriorityList<SmartEntity> GetBuildingsToAttack()
		{
			return this.nearnessToBuildings;
		}
	}
}
