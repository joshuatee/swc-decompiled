using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Utils.MetaData
{
	public class Column
	{
		public string ColName
		{
			get;
			private set;
		}

		public ColumnType ColType
		{
			get;
			private set;
		}

		public Column(string colName, ColumnType colType)
		{
			this.ColName = colName;
			this.ColType = colType;
		}

		protected internal Column(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Column)GCHandledObjects.GCHandleToObject(instance)).ColName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Column)GCHandledObjects.GCHandleToObject(instance)).ColType);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((Column)GCHandledObjects.GCHandleToObject(instance)).ColName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((Column)GCHandledObjects.GCHandleToObject(instance)).ColType = (ColumnType)(*(int*)args);
			return -1L;
		}
	}
}
