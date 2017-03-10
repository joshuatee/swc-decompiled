using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class DefenseEncounterVO : IValueObject
	{
		public static int COLUMN_waves
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string WaveGroup
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.WaveGroup = row.TryGetString(DefenseEncounterVO.COLUMN_waves);
		}

		public DefenseEncounterVO()
		{
		}

		protected internal DefenseEncounterVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(DefenseEncounterVO.COLUMN_waves);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefenseEncounterVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefenseEncounterVO)GCHandledObjects.GCHandleToObject(instance)).WaveGroup);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((DefenseEncounterVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			DefenseEncounterVO.COLUMN_waves = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((DefenseEncounterVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((DefenseEncounterVO)GCHandledObjects.GCHandleToObject(instance)).WaveGroup = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
