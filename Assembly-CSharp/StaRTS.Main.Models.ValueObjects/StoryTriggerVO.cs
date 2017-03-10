using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class StoryTriggerVO : IValueObject
	{
		public static int COLUMN_trigger_type
		{
			get;
			private set;
		}

		public static int COLUMN_prepare_string
		{
			get;
			private set;
		}

		public static int COLUMN_reaction
		{
			get;
			private set;
		}

		public static int COLUMN_ui_action
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string TriggerType
		{
			get;
			set;
		}

		public string PrepareString
		{
			get;
			set;
		}

		public string Reaction
		{
			get;
			set;
		}

		public string UpdateAction
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.TriggerType = row.TryGetString(StoryTriggerVO.COLUMN_trigger_type);
			this.PrepareString = row.TryGetString(StoryTriggerVO.COLUMN_prepare_string);
			this.Reaction = row.TryGetString(StoryTriggerVO.COLUMN_reaction);
			this.UpdateAction = row.TryGetString(StoryTriggerVO.COLUMN_ui_action);
		}

		public StoryTriggerVO()
		{
		}

		protected internal StoryTriggerVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StoryTriggerVO.COLUMN_prepare_string);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StoryTriggerVO.COLUMN_reaction);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StoryTriggerVO.COLUMN_trigger_type);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StoryTriggerVO.COLUMN_ui_action);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoryTriggerVO)GCHandledObjects.GCHandleToObject(instance)).PrepareString);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoryTriggerVO)GCHandledObjects.GCHandleToObject(instance)).Reaction);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoryTriggerVO)GCHandledObjects.GCHandleToObject(instance)).TriggerType);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoryTriggerVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoryTriggerVO)GCHandledObjects.GCHandleToObject(instance)).UpdateAction);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((StoryTriggerVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			StoryTriggerVO.COLUMN_prepare_string = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			StoryTriggerVO.COLUMN_reaction = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			StoryTriggerVO.COLUMN_trigger_type = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			StoryTriggerVO.COLUMN_ui_action = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((StoryTriggerVO)GCHandledObjects.GCHandleToObject(instance)).PrepareString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((StoryTriggerVO)GCHandledObjects.GCHandleToObject(instance)).Reaction = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((StoryTriggerVO)GCHandledObjects.GCHandleToObject(instance)).TriggerType = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((StoryTriggerVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((StoryTriggerVO)GCHandledObjects.GCHandleToObject(instance)).UpdateAction = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
