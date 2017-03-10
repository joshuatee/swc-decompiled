using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Player.Misc
{
	public abstract class AbstractTimedEvent : ISerializable
	{
		public string Uid
		{
			get;
			set;
		}

		public bool Collected
		{
			get;
			set;
		}

		public string ToJson()
		{
			return "{}";
		}

		public virtual ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			this.Uid = dictionary["uid"].ToString();
			if (dictionary.ContainsKey("collected"))
			{
				this.Collected = (bool)dictionary["collected"];
			}
			return this;
		}

		protected AbstractTimedEvent()
		{
		}

		protected internal AbstractTimedEvent(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractTimedEvent)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractTimedEvent)GCHandledObjects.GCHandleToObject(instance)).Collected);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractTimedEvent)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AbstractTimedEvent)GCHandledObjects.GCHandleToObject(instance)).Collected = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AbstractTimedEvent)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractTimedEvent)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
