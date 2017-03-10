using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class EncounterProfileVO : IValueObject
	{
		public const char GROUP_DELIMITER = '|';

		public const char ELEMENT_DELIMITER = ',';

		public const int BUILDING_UID_ARG = 0;

		public const int TRIGGER_TYPE_ARG = 1;

		public const int TROOP_UID_ARG = 2;

		public const int TROOP_QUANTITY_ARG = 3;

		public const int LEASHED_ARG = 4;

		public const int STAGGER_ARG = 5;

		public const int LAST_DITCH_DELAY_ARG = 6;

		public static int COLUMN_groups
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string GroupString
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.GroupString = row.TryGetString(EncounterProfileVO.COLUMN_groups);
		}

		public EncounterProfileVO()
		{
		}

		protected internal EncounterProfileVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(EncounterProfileVO.COLUMN_groups);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EncounterProfileVO)GCHandledObjects.GCHandleToObject(instance)).GroupString);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EncounterProfileVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((EncounterProfileVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			EncounterProfileVO.COLUMN_groups = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((EncounterProfileVO)GCHandledObjects.GCHandleToObject(instance)).GroupString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((EncounterProfileVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
