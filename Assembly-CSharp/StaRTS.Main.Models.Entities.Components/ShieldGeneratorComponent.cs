using Net.RichardLord.Ash.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class ShieldGeneratorComponent : ComponentBase
	{
		private int currentRadius;

		public int PointsRange
		{
			get;
			set;
		}

		public Entity ShieldBorderEntity
		{
			get;
			set;
		}

		public int CurrentRadius
		{
			get
			{
				return this.currentRadius;
			}
			set
			{
				this.currentRadius = value;
				this.RadiusSquared = value * value;
			}
		}

		public int RadiusSquared
		{
			get;
			private set;
		}

		public ShieldGeneratorComponent()
		{
		}

		protected internal ShieldGeneratorComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldGeneratorComponent)GCHandledObjects.GCHandleToObject(instance)).CurrentRadius);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldGeneratorComponent)GCHandledObjects.GCHandleToObject(instance)).PointsRange);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldGeneratorComponent)GCHandledObjects.GCHandleToObject(instance)).RadiusSquared);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldGeneratorComponent)GCHandledObjects.GCHandleToObject(instance)).ShieldBorderEntity);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ShieldGeneratorComponent)GCHandledObjects.GCHandleToObject(instance)).CurrentRadius = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ShieldGeneratorComponent)GCHandledObjects.GCHandleToObject(instance)).PointsRange = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((ShieldGeneratorComponent)GCHandledObjects.GCHandleToObject(instance)).RadiusSquared = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((ShieldGeneratorComponent)GCHandledObjects.GCHandleToObject(instance)).ShieldBorderEntity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
