using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class SpecialAttackTrapEventData : ITrapEventData
	{
		public string SpecialAttackName
		{
			get;
			private set;
		}

		public ITrapEventData Init(string rawData)
		{
			if (string.IsNullOrEmpty(rawData))
			{
				Service.Get<StaRTSLogger>().Error("All SpecialAttack Traps must list the uid of the special attack");
				return null;
			}
			this.SpecialAttackName = rawData.TrimEnd(new char[]
			{
				' '
			});
			return this;
		}

		public SpecialAttackTrapEventData()
		{
		}

		protected internal SpecialAttackTrapEventData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTrapEventData)GCHandledObjects.GCHandleToObject(instance)).SpecialAttackName);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttackTrapEventData)GCHandledObjects.GCHandleToObject(instance)).Init(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SpecialAttackTrapEventData)GCHandledObjects.GCHandleToObject(instance)).SpecialAttackName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
