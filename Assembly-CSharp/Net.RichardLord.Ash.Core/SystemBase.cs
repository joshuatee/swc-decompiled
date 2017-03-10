using System;
using WinRTBridge;

namespace Net.RichardLord.Ash.Core
{
	public abstract class SystemBase<T>
	{
		internal T Next
		{
			get;
			set;
		}

		internal T Previous
		{
			get;
			set;
		}

		internal int Priority
		{
			get;
			set;
		}

		internal ushort SchedulingPattern
		{
			get;
			set;
		}

		public abstract void AddToGame(IGame game);

		public abstract void RemoveFromGame(IGame game);

		public abstract void Update();

		protected SystemBase()
		{
		}

		protected internal SystemBase(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SystemBase<SimSystemBase>)GCHandledObjects.GCHandleToObject(instance)).AddToGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SystemBase<SimSystemBase>)GCHandledObjects.GCHandleToObject(instance)).Priority);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((SystemBase<SimSystemBase>)GCHandledObjects.GCHandleToObject(instance)).RemoveFromGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((SystemBase<SimSystemBase>)GCHandledObjects.GCHandleToObject(instance)).Priority = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((SystemBase<SimSystemBase>)GCHandledObjects.GCHandleToObject(instance)).Update();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SystemBase<ViewSystemBase>)GCHandledObjects.GCHandleToObject(instance)).AddToGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SystemBase<ViewSystemBase>)GCHandledObjects.GCHandleToObject(instance)).Priority);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SystemBase<ViewSystemBase>)GCHandledObjects.GCHandleToObject(instance)).RemoveFromGame((IGame)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((SystemBase<ViewSystemBase>)GCHandledObjects.GCHandleToObject(instance)).Priority = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((SystemBase<ViewSystemBase>)GCHandledObjects.GCHandleToObject(instance)).Update();
			return -1L;
		}
	}
}
