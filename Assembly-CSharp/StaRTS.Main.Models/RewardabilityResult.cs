using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class RewardabilityResult
	{
		public bool CanAward
		{
			get;
			set;
		}

		public string Reason
		{
			get;
			set;
		}

		public RewardabilityResult()
		{
			this.CanAward = true;
		}

		protected internal RewardabilityResult(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RewardabilityResult)GCHandledObjects.GCHandleToObject(instance)).CanAward);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RewardabilityResult)GCHandledObjects.GCHandleToObject(instance)).Reason);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((RewardabilityResult)GCHandledObjects.GCHandleToObject(instance)).CanAward = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((RewardabilityResult)GCHandledObjects.GCHandleToObject(instance)).Reason = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
