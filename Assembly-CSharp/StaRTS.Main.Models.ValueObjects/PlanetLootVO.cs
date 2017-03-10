using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class PlanetLootVO : IValueObject
	{
		public static int COLUMN_empirePlanetLootEntryUids
		{
			get;
			private set;
		}

		public static int COLUMN_rebelPlanetLootEntryUids
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string[] EmpirePlanetLootEntryUids
		{
			get;
			private set;
		}

		public string[] RebelPlanetLootEntryUids
		{
			get;
			private set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.EmpirePlanetLootEntryUids = row.TryGetStringArray(PlanetLootVO.COLUMN_empirePlanetLootEntryUids);
			this.RebelPlanetLootEntryUids = row.TryGetStringArray(PlanetLootVO.COLUMN_rebelPlanetLootEntryUids);
		}

		public PlanetLootVO()
		{
		}

		protected internal PlanetLootVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetLootVO.COLUMN_empirePlanetLootEntryUids);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(PlanetLootVO.COLUMN_rebelPlanetLootEntryUids);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetLootVO)GCHandledObjects.GCHandleToObject(instance)).EmpirePlanetLootEntryUids);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetLootVO)GCHandledObjects.GCHandleToObject(instance)).RebelPlanetLootEntryUids);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetLootVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((PlanetLootVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			PlanetLootVO.COLUMN_empirePlanetLootEntryUids = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			PlanetLootVO.COLUMN_rebelPlanetLootEntryUids = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((PlanetLootVO)GCHandledObjects.GCHandleToObject(instance)).EmpirePlanetLootEntryUids = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((PlanetLootVO)GCHandledObjects.GCHandleToObject(instance)).RebelPlanetLootEntryUids = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((PlanetLootVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
