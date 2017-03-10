using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class ProfanityVO : IValueObject
	{
		public static int COLUMN_words
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string[] Words
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.Words = row.TryGetStringArray(ProfanityVO.COLUMN_words);
		}

		public ProfanityVO()
		{
		}

		protected internal ProfanityVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ProfanityVO.COLUMN_words);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProfanityVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProfanityVO)GCHandledObjects.GCHandleToObject(instance)).Words);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ProfanityVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			ProfanityVO.COLUMN_words = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ProfanityVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((ProfanityVO)GCHandledObjects.GCHandleToObject(instance)).Words = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}
	}
}
