using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class StoryActionVO : IValueObject
	{
		public static int COLUMN_action_type
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

		public static int COLUMN_log_type
		{
			get;
			private set;
		}

		public static int COLUMN_log_tag
		{
			get;
			private set;
		}

		public static int COLUMN_log_path
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string ActionType
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

		public string LogType
		{
			get;
			set;
		}

		public string LogTag
		{
			get;
			set;
		}

		public string LogPath
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.ActionType = row.TryGetString(StoryActionVO.COLUMN_action_type);
			this.PrepareString = row.TryGetString(StoryActionVO.COLUMN_prepare_string);
			this.Reaction = row.TryGetString(StoryActionVO.COLUMN_reaction);
			this.LogType = row.TryGetString(StoryActionVO.COLUMN_log_type);
			this.LogTag = row.TryGetString(StoryActionVO.COLUMN_log_tag);
			this.LogPath = row.TryGetString(StoryActionVO.COLUMN_log_path);
		}

		public StoryActionVO()
		{
		}

		protected internal StoryActionVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoryActionVO)GCHandledObjects.GCHandleToObject(instance)).ActionType);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StoryActionVO.COLUMN_action_type);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StoryActionVO.COLUMN_log_path);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StoryActionVO.COLUMN_log_tag);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StoryActionVO.COLUMN_log_type);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StoryActionVO.COLUMN_prepare_string);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StoryActionVO.COLUMN_reaction);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoryActionVO)GCHandledObjects.GCHandleToObject(instance)).LogPath);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoryActionVO)GCHandledObjects.GCHandleToObject(instance)).LogTag);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoryActionVO)GCHandledObjects.GCHandleToObject(instance)).LogType);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoryActionVO)GCHandledObjects.GCHandleToObject(instance)).PrepareString);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoryActionVO)GCHandledObjects.GCHandleToObject(instance)).Reaction);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoryActionVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((StoryActionVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((StoryActionVO)GCHandledObjects.GCHandleToObject(instance)).ActionType = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			StoryActionVO.COLUMN_action_type = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			StoryActionVO.COLUMN_log_path = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			StoryActionVO.COLUMN_log_tag = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			StoryActionVO.COLUMN_log_type = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			StoryActionVO.COLUMN_prepare_string = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			StoryActionVO.COLUMN_reaction = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((StoryActionVO)GCHandledObjects.GCHandleToObject(instance)).LogPath = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((StoryActionVO)GCHandledObjects.GCHandleToObject(instance)).LogTag = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((StoryActionVO)GCHandledObjects.GCHandleToObject(instance)).LogType = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((StoryActionVO)GCHandledObjects.GCHandleToObject(instance)).PrepareString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((StoryActionVO)GCHandledObjects.GCHandleToObject(instance)).Reaction = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((StoryActionVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
