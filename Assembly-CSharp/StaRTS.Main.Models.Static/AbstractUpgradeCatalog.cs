using StaRTS.Main.Models.ValueObjects;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Static
{
	public abstract class AbstractUpgradeCatalog
	{
		protected virtual IUpgradeableVO InternalGetByLevel(string upgradeGroup, int level)
		{
			return null;
		}

		public IUpgradeableVO GetByLevel(string upgradeGroup, int level)
		{
			return this.InternalGetByLevel(upgradeGroup, level);
		}

		public IUpgradeableVO GetByLevel(IUpgradeableVO vo, int level)
		{
			return this.InternalGetByLevel(vo.UpgradeGroup, level);
		}

		protected AbstractUpgradeCatalog()
		{
		}

		protected internal AbstractUpgradeCatalog(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractUpgradeCatalog)GCHandledObjects.GCHandleToObject(instance)).GetByLevel((IUpgradeableVO)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractUpgradeCatalog)GCHandledObjects.GCHandleToObject(instance)).GetByLevel(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractUpgradeCatalog)GCHandledObjects.GCHandleToObject(instance)).InternalGetByLevel(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1)));
		}
	}
}
