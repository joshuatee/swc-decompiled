using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Battle
{
	public class DefenseTroopGroup
	{
		public string TroopUid
		{
			get;
			set;
		}

		public int Quantity
		{
			get;
			set;
		}

		public uint Seconds
		{
			get;
			set;
		}

		public int Direction
		{
			get;
			set;
		}

		public int Spread
		{
			get;
			set;
		}

		public int Range
		{
			get;
			set;
		}

		public DefenseTroopGroup()
		{
		}

		protected internal DefenseTroopGroup(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefenseTroopGroup)GCHandledObjects.GCHandleToObject(instance)).Direction);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefenseTroopGroup)GCHandledObjects.GCHandleToObject(instance)).Quantity);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefenseTroopGroup)GCHandledObjects.GCHandleToObject(instance)).Range);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefenseTroopGroup)GCHandledObjects.GCHandleToObject(instance)).Spread);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DefenseTroopGroup)GCHandledObjects.GCHandleToObject(instance)).TroopUid);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((DefenseTroopGroup)GCHandledObjects.GCHandleToObject(instance)).Direction = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((DefenseTroopGroup)GCHandledObjects.GCHandleToObject(instance)).Quantity = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((DefenseTroopGroup)GCHandledObjects.GCHandleToObject(instance)).Range = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((DefenseTroopGroup)GCHandledObjects.GCHandleToObject(instance)).Spread = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((DefenseTroopGroup)GCHandledObjects.GCHandleToObject(instance)).TroopUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
