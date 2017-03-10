using Net.RichardLord.Ash.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.World
{
	public class FootprintMoveData
	{
		public Entity Building
		{
			get;
			private set;
		}

		public float WorldAnchorX
		{
			get;
			private set;
		}

		public float WorldAnchorZ
		{
			get;
			private set;
		}

		public bool CanOccupy
		{
			get;
			private set;
		}

		public FootprintMoveData(Entity building, float worldAnchorX, float worldAnchorZ, bool canOccupy)
		{
			this.Building = building;
			this.WorldAnchorX = worldAnchorX;
			this.WorldAnchorZ = worldAnchorZ;
			this.CanOccupy = canOccupy;
		}

		protected internal FootprintMoveData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FootprintMoveData)GCHandledObjects.GCHandleToObject(instance)).Building);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FootprintMoveData)GCHandledObjects.GCHandleToObject(instance)).CanOccupy);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FootprintMoveData)GCHandledObjects.GCHandleToObject(instance)).WorldAnchorX);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((FootprintMoveData)GCHandledObjects.GCHandleToObject(instance)).WorldAnchorZ);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((FootprintMoveData)GCHandledObjects.GCHandleToObject(instance)).Building = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((FootprintMoveData)GCHandledObjects.GCHandleToObject(instance)).CanOccupy = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((FootprintMoveData)GCHandledObjects.GCHandleToObject(instance)).WorldAnchorX = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((FootprintMoveData)GCHandledObjects.GCHandleToObject(instance)).WorldAnchorZ = *(float*)args;
			return -1L;
		}
	}
}
