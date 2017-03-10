using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class DeployableInfoActionButtonTag
	{
		public string ActionId
		{
			get;
			private set;
		}

		public List<string> DataList
		{
			get;
			private set;
		}

		public DeployableInfoActionButtonTag(string action, string data)
		{
			this.ActionId = action;
			if (!string.IsNullOrEmpty(data))
			{
				this.DataList = new List<string>(data.Split(new char[]
				{
					' '
				}));
			}
		}

		protected internal DeployableInfoActionButtonTag(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableInfoActionButtonTag)GCHandledObjects.GCHandleToObject(instance)).ActionId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableInfoActionButtonTag)GCHandledObjects.GCHandleToObject(instance)).DataList);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((DeployableInfoActionButtonTag)GCHandledObjects.GCHandleToObject(instance)).ActionId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((DeployableInfoActionButtonTag)GCHandledObjects.GCHandleToObject(instance)).DataList = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
