using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class StoryVO : IValueObject
	{
		public static int COLUMN_character
		{
			get;
			private set;
		}

		public static int COLUMN_side
		{
			get;
			private set;
		}

		public static int COLUMN_transcript
		{
			get;
			private set;
		}

		public static int COLUMN_audio_asset
		{
			get;
			private set;
		}

		public static int COLUMN_next_step
		{
			get;
			private set;
		}

		public static int COLUMN_next_step_data
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string Character
		{
			get;
			set;
		}

		public string Side
		{
			get;
			set;
		}

		public string Transcript
		{
			get;
			set;
		}

		public string AudioAsset
		{
			get;
			set;
		}

		public string NextStep
		{
			get;
			set;
		}

		public string NextStepData
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.Character = row.TryGetString(StoryVO.COLUMN_character);
			this.Side = row.TryGetString(StoryVO.COLUMN_side);
			this.Transcript = row.TryGetString(StoryVO.COLUMN_transcript);
			this.AudioAsset = row.TryGetString(StoryVO.COLUMN_audio_asset);
			this.NextStep = row.TryGetString(StoryVO.COLUMN_next_step);
			this.NextStepData = row.TryGetString(StoryVO.COLUMN_next_step_data);
		}

		public StoryVO()
		{
		}

		protected internal StoryVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoryVO)GCHandledObjects.GCHandleToObject(instance)).AudioAsset);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoryVO)GCHandledObjects.GCHandleToObject(instance)).Character);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StoryVO.COLUMN_audio_asset);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StoryVO.COLUMN_character);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StoryVO.COLUMN_next_step);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StoryVO.COLUMN_next_step_data);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StoryVO.COLUMN_side);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(StoryVO.COLUMN_transcript);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoryVO)GCHandledObjects.GCHandleToObject(instance)).NextStep);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoryVO)GCHandledObjects.GCHandleToObject(instance)).NextStepData);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoryVO)GCHandledObjects.GCHandleToObject(instance)).Side);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoryVO)GCHandledObjects.GCHandleToObject(instance)).Transcript);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoryVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((StoryVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((StoryVO)GCHandledObjects.GCHandleToObject(instance)).AudioAsset = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((StoryVO)GCHandledObjects.GCHandleToObject(instance)).Character = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			StoryVO.COLUMN_audio_asset = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			StoryVO.COLUMN_character = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			StoryVO.COLUMN_next_step = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			StoryVO.COLUMN_next_step_data = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			StoryVO.COLUMN_side = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			StoryVO.COLUMN_transcript = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((StoryVO)GCHandledObjects.GCHandleToObject(instance)).NextStep = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((StoryVO)GCHandledObjects.GCHandleToObject(instance)).NextStepData = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((StoryVO)GCHandledObjects.GCHandleToObject(instance)).Side = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((StoryVO)GCHandledObjects.GCHandleToObject(instance)).Transcript = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((StoryVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
