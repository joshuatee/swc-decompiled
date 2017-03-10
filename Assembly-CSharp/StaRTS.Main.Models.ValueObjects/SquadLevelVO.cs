using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class SquadLevelVO : IValueObject
	{
		public static int COLUMN_repThreshold
		{
			get;
			private set;
		}

		public static int COLUMN_level
		{
			get;
			private set;
		}

		public static int COLUMN_slots
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public int RepThreshold
		{
			get;
			private set;
		}

		public int Level
		{
			get;
			private set;
		}

		public int Slots
		{
			get;
			private set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.RepThreshold = row.TryGetInt(SquadLevelVO.COLUMN_repThreshold);
			this.Level = row.TryGetInt(SquadLevelVO.COLUMN_level);
			this.Slots = row.TryGetInt(SquadLevelVO.COLUMN_slots);
		}

		public SquadLevelVO()
		{
		}

		protected internal SquadLevelVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadLevelVO.COLUMN_level);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadLevelVO.COLUMN_repThreshold);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(SquadLevelVO.COLUMN_slots);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadLevelVO)GCHandledObjects.GCHandleToObject(instance)).Level);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadLevelVO)GCHandledObjects.GCHandleToObject(instance)).RepThreshold);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadLevelVO)GCHandledObjects.GCHandleToObject(instance)).Slots);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadLevelVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SquadLevelVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			SquadLevelVO.COLUMN_level = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			SquadLevelVO.COLUMN_repThreshold = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			SquadLevelVO.COLUMN_slots = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((SquadLevelVO)GCHandledObjects.GCHandleToObject(instance)).Level = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((SquadLevelVO)GCHandledObjects.GCHandleToObject(instance)).RepThreshold = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((SquadLevelVO)GCHandledObjects.GCHandleToObject(instance)).Slots = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((SquadLevelVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
