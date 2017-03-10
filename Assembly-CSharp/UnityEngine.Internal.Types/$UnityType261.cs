using Net.RichardLord.Ash.Core;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType261 : $UnityType
	{
		public unsafe $UnityType261()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 418120) = ldftn($Invoke0);
			*(data + 418148) = ldftn($Invoke1);
			*(data + 418176) = ldftn($Invoke2);
			*(data + 418204) = ldftn($Invoke3);
			*(data + 418232) = ldftn($Invoke4);
			*(data + 418260) = ldftn($Invoke5);
			*(data + 418288) = ldftn($Invoke6);
			*(data + 418316) = ldftn($Invoke7);
			*(data + 418344) = ldftn($Invoke8);
			*(data + 418372) = ldftn($Invoke9);
			*(data + 418400) = ldftn($Invoke10);
			*(data + 418428) = ldftn($Invoke11);
			*(data + 418456) = ldftn($Invoke12);
			*(data + 418484) = ldftn($Invoke13);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IGame)GCHandledObjects.GCHandleToObject(instance)).UpdateSimComplete += (Action)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((IGame)GCHandledObjects.GCHandleToObject(instance)).UpdateViewComplete += (Action)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((IGame)GCHandledObjects.GCHandleToObject(instance)).AddEntity((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IGame)GCHandledObjects.GCHandleToObject(instance)).Updating);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IGame)GCHandledObjects.GCHandleToObject(instance)).GetSimSystem((Type)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IGame)GCHandledObjects.GCHandleToObject(instance)).GetViewSystem((Type)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((IGame)GCHandledObjects.GCHandleToObject(instance)).UpdateSimComplete -= (Action)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((IGame)GCHandledObjects.GCHandleToObject(instance)).UpdateViewComplete -= (Action)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((IGame)GCHandledObjects.GCHandleToObject(instance)).RemoveAllEntities();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((IGame)GCHandledObjects.GCHandleToObject(instance)).RemoveAllSystems();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((IGame)GCHandledObjects.GCHandleToObject(instance)).RemoveEntity((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((IGame)GCHandledObjects.GCHandleToObject(instance)).RemoveSimSystem((SimSystemBase)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((IGame)GCHandledObjects.GCHandleToObject(instance)).RemoveViewSystem((ViewSystemBase)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((IGame)GCHandledObjects.GCHandleToObject(instance)).UpdateViewSystems(*(float*)args);
			return -1L;
		}
	}
}
