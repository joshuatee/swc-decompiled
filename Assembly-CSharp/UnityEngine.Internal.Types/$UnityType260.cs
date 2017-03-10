using Net.RichardLord.Ash.Core;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType260 : $UnityType
	{
		public unsafe $UnityType260()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 417952) = ldftn($Invoke0);
			*(data + 417980) = ldftn($Invoke1);
			*(data + 418008) = ldftn($Invoke2);
			*(data + 418036) = ldftn($Invoke3);
			*(data + 418064) = ldftn($Invoke4);
			*(data + 418092) = ldftn($Invoke5);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IFamily)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((IFamily)GCHandledObjects.GCHandleToObject(instance)).ComponentAddedToEntity((Entity)GCHandledObjects.GCHandleToObject(*args), (Type)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((IFamily)GCHandledObjects.GCHandleToObject(instance)).ComponentRemovedFromEntity((Entity)GCHandledObjects.GCHandleToObject(*args), (Type)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((IFamily)GCHandledObjects.GCHandleToObject(instance)).NewEntity((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((IFamily)GCHandledObjects.GCHandleToObject(instance)).RemoveEntity((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((IFamily)GCHandledObjects.GCHandleToObject(instance)).Setup((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
