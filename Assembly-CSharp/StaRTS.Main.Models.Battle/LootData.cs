using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Battle
{
	public class LootData
	{
		public int Credits
		{
			get;
			set;
		}

		public int Materials
		{
			get;
			set;
		}

		public int Contraband
		{
			get;
			set;
		}

		public LootData(int credits, int materials, int contraband)
		{
			this.Credits = credits;
			this.Materials = materials;
			this.Contraband = contraband;
		}

		protected internal LootData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LootData)GCHandledObjects.GCHandleToObject(instance)).Contraband);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LootData)GCHandledObjects.GCHandleToObject(instance)).Credits);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((LootData)GCHandledObjects.GCHandleToObject(instance)).Materials);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((LootData)GCHandledObjects.GCHandleToObject(instance)).Contraband = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((LootData)GCHandledObjects.GCHandleToObject(instance)).Credits = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((LootData)GCHandledObjects.GCHandleToObject(instance)).Materials = *(int*)args;
			return -1L;
		}
	}
}
