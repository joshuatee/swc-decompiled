using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Utils.Core
{
	public class WWWManager
	{
		private List<WWW> outstandingWWWs;

		public WWWManager()
		{
			Service.Set<WWWManager>(this);
			this.outstandingWWWs = new List<WWW>();
		}

		public int ActiveRequestCount()
		{
			return this.outstandingWWWs.Count;
		}

		public void CancelAll()
		{
			int i = 0;
			int count = this.outstandingWWWs.Count;
			while (i < count)
			{
				this.outstandingWWWs[i].Dispose();
				i++;
			}
			this.outstandingWWWs.Clear();
		}

		public static void Add(WWW www)
		{
			if (Service.IsSet<WWWManager>())
			{
				Service.Get<WWWManager>().outstandingWWWs.Add(www);
			}
		}

		public static bool Remove(WWW www)
		{
			return Service.IsSet<WWWManager>() && Service.Get<WWWManager>().outstandingWWWs.Remove(www);
		}

		protected internal WWWManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WWWManager)GCHandledObjects.GCHandleToObject(instance)).ActiveRequestCount());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			WWWManager.Add((WWW)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((WWWManager)GCHandledObjects.GCHandleToObject(instance)).CancelAll();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(WWWManager.Remove((WWW)GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
