using Net.RichardLord.Ash.Core;
using StaRTS.DataStructures;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

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

		protected internal SpatialIndex(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SpatialIndex)GCHandledObjects.GCHandleToObject(instance)).AddAreaTriggerBuildingsInRangeOf((Entity)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1), *(int*)(args + 2));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SpatialIndex)GCHandledObjects.GCHandleToObject(instance)).AddBuildingsToAttack((SmartEntity)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SpatialIndex)GCHandledObjects.GCHandleToObject(instance)).AddTurretsInRangeOf((Entity)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1), *(int*)(args + 2));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpatialIndex)GCHandledObjects.GCHandleToObject(instance)).AlreadyScannedAreaTriggerBuildingsInRange);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpatialIndex)GCHandledObjects.GCHandleToObject(instance)).AlreadyScannedBuildingsToAttack);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpatialIndex)GCHandledObjects.GCHandleToObject(instance)).AlreadyScannedTurretsInRange);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpatialIndex)GCHandledObjects.GCHandleToObject(instance)).GetArareaTriggerBuildingsInRange());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpatialIndex)GCHandledObjects.GCHandleToObject(instance)).GetBuildingsToAttack());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpatialIndex)GCHandledObjects.GCHandleToObject(instance)).GetTurretsInRangeOf());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((SpatialIndex)GCHandledObjects.GCHandleToObject(instance)).ResetTurretScanedFlag();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((SpatialIndex)GCHandledObjects.GCHandleToObject(instance)).AlreadyScannedAreaTriggerBuildingsInRange = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((SpatialIndex)GCHandledObjects.GCHandleToObject(instance)).AlreadyScannedBuildingsToAttack = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((SpatialIndex)GCHandledObjects.GCHandleToObject(instance)).AlreadyScannedTurretsInRange = (*(sbyte*)args != 0);
			return -1L;
		}
	}
}
