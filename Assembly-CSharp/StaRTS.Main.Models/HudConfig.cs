using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class HudConfig
	{
		private List<string> elements;

		public HudConfig(params string[] list)
		{
			this.elements = new List<string>();
			for (int i = 0; i < list.Length; i++)
			{
				string item = list[i];
				this.elements.Add(item);
			}
		}

		public void Add(string elementName)
		{
			if (!this.elements.Contains(elementName))
			{
				this.elements.Add(elementName);
			}
		}

		public void Remove(string elementName)
		{
			if (this.elements.Contains(elementName))
			{
				this.elements.Remove(elementName);
			}
		}

		public bool Has(string elementName)
		{
			return this.elements.Contains(elementName);
		}

		protected internal HudConfig(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((HudConfig)GCHandledObjects.GCHandleToObject(instance)).Add(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HudConfig)GCHandledObjects.GCHandleToObject(instance)).Has(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((HudConfig)GCHandledObjects.GCHandleToObject(instance)).Remove(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}
	}
}
