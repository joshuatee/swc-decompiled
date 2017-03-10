using Net.RichardLord.Ash.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class DroidComponent : ComponentBase
	{
		public Entity Target
		{
			get;
			set;
		}

		public float Delay
		{
			get;
			set;
		}

		public bool AnimateTravel
		{
			get;
			set;
		}

		public DroidComponent()
		{
			this.Target = null;
			this.Delay = 0f;
			this.AnimateTravel = true;
		}

		protected internal DroidComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DroidComponent)GCHandledObjects.GCHandleToObject(instance)).AnimateTravel);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DroidComponent)GCHandledObjects.GCHandleToObject(instance)).Delay);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DroidComponent)GCHandledObjects.GCHandleToObject(instance)).Target);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((DroidComponent)GCHandledObjects.GCHandleToObject(instance)).AnimateTravel = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((DroidComponent)GCHandledObjects.GCHandleToObject(instance)).Delay = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((DroidComponent)GCHandledObjects.GCHandleToObject(instance)).Target = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
