using StaRTS.Utils.MetaData;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class BattleScriptVO : IValueObject
	{
		public static int COLUMN_scripts
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string Scripts
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.Scripts = row.TryGetString(BattleScriptVO.COLUMN_scripts);
		}

		public BattleScriptVO()
		{
		}

		protected internal BattleScriptVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BattleScriptVO.COLUMN_scripts);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleScriptVO)GCHandledObjects.GCHandleToObject(instance)).Scripts);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BattleScriptVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((BattleScriptVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			BattleScriptVO.COLUMN_scripts = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((BattleScriptVO)GCHandledObjects.GCHandleToObject(instance)).Scripts = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((BattleScriptVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
