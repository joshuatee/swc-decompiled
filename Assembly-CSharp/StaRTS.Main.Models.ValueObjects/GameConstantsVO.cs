using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class GameConstantsVO : IValueObject
	{
		public static int COLUMN_value
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string Value
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.Value = row.TryGetString(GameConstantsVO.COLUMN_value);
		}

		public GameConstantsVO()
		{
		}

		protected internal GameConstantsVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(GameConstantsVO.COLUMN_value);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameConstantsVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameConstantsVO)GCHandledObjects.GCHandleToObject(instance)).Value);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((GameConstantsVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			GameConstantsVO.COLUMN_value = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((GameConstantsVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((GameConstantsVO)GCHandledObjects.GCHandleToObject(instance)).Value = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
