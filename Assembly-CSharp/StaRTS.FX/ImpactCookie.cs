using StaRTS.Main.Models;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.FX
{
	public class ImpactCookie
	{
		public GameObject ImpactGameObject
		{
			get;
			set;
		}

		public FactionType ImpactFaction
		{
			get;
			set;
		}

		public ImpactCookie()
		{
		}

		protected internal ImpactCookie(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ImpactCookie)GCHandledObjects.GCHandleToObject(instance)).ImpactFaction);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ImpactCookie)GCHandledObjects.GCHandleToObject(instance)).ImpactGameObject);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ImpactCookie)GCHandledObjects.GCHandleToObject(instance)).ImpactFaction = (FactionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ImpactCookie)GCHandledObjects.GCHandleToObject(instance)).ImpactGameObject = (GameObject)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
