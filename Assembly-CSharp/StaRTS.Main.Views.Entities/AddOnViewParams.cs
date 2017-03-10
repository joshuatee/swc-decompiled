using StaRTS.Main.Models.Entities;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Views.Entities
{
	public class AddOnViewParams
	{
		public SmartEntity Entity
		{
			get;
			private set;
		}

		public string ParentName
		{
			get;
			private set;
		}

		public AddOnViewParams(SmartEntity entity, string parentName)
		{
			this.Entity = entity;
			this.ParentName = parentName;
		}

		protected internal AddOnViewParams(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AddOnViewParams)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AddOnViewParams)GCHandledObjects.GCHandleToObject(instance)).ParentName);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AddOnViewParams)GCHandledObjects.GCHandleToObject(instance)).Entity = (SmartEntity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AddOnViewParams)GCHandledObjects.GCHandleToObject(instance)).ParentName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
