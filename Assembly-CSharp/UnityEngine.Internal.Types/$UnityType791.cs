using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers.CombatTriggers;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType791 : $UnityType
	{
		public unsafe $UnityType791()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 567892) = ldftn($Invoke0);
			*(data + 567920) = ldftn($Invoke1);
			*(data + 567948) = ldftn($Invoke2);
			*(data + 567976) = ldftn($Invoke3);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICombatTrigger)GCHandledObjects.GCHandleToObject(instance)).Owner);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICombatTrigger)GCHandledObjects.GCHandleToObject(instance)).Type);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ICombatTrigger)GCHandledObjects.GCHandleToObject(instance)).IsAlreadyTriggered());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ICombatTrigger)GCHandledObjects.GCHandleToObject(instance)).Trigger((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
