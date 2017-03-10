using Net.RichardLord.Ash.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class DefenderComponent : ComponentBase
	{
		public int SpawnX
		{
			get;
			private set;
		}

		public int SpawnZ
		{
			get;
			private set;
		}

		public bool Leashed
		{
			get;
			private set;
		}

		public DamageableComponent SpawnBuilding
		{
			get;
			private set;
		}

		public int PatrolLoc
		{
			get;
			set;
		}

		public bool Patrolling
		{
			get;
			set;
		}

		public DefenderComponent(int spawnX, int spawnZ, DamageableComponent building, bool leashed, int spawnLocIndex)
		{
			this.SpawnX = spawnX;
			this.SpawnZ = spawnZ;
			this.SpawnBuilding = building;
			this.PatrolLoc = spawnLocIndex;
			this.Leashed = leashed;
			this.Patrolling = false;
		}

		protected internal DefenderComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefenderComponent)GCHandledObjects.GCHandleToObject(instance)).Leashed);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefenderComponent)GCHandledObjects.GCHandleToObject(instance)).Patrolling);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefenderComponent)GCHandledObjects.GCHandleToObject(instance)).PatrolLoc);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefenderComponent)GCHandledObjects.GCHandleToObject(instance)).SpawnBuilding);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefenderComponent)GCHandledObjects.GCHandleToObject(instance)).SpawnX);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefenderComponent)GCHandledObjects.GCHandleToObject(instance)).SpawnZ);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((DefenderComponent)GCHandledObjects.GCHandleToObject(instance)).Leashed = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((DefenderComponent)GCHandledObjects.GCHandleToObject(instance)).Patrolling = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((DefenderComponent)GCHandledObjects.GCHandleToObject(instance)).PatrolLoc = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((DefenderComponent)GCHandledObjects.GCHandleToObject(instance)).SpawnBuilding = (DamageableComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((DefenderComponent)GCHandledObjects.GCHandleToObject(instance)).SpawnX = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((DefenderComponent)GCHandledObjects.GCHandleToObject(instance)).SpawnZ = *(int*)args;
			return -1L;
		}
	}
}
