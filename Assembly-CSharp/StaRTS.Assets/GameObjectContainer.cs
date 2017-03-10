using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Assets
{
	public class GameObjectContainer
	{
		public GameObject GameObj
		{
			get;
			private set;
		}

		public bool Flagged
		{
			get;
			set;
		}

		public GameObjectContainer(GameObject gameObject)
		{
			this.GameObj = gameObject;
			this.Flagged = false;
		}

		protected internal GameObjectContainer(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameObjectContainer)GCHandledObjects.GCHandleToObject(instance)).Flagged);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameObjectContainer)GCHandledObjects.GCHandleToObject(instance)).GameObj);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((GameObjectContainer)GCHandledObjects.GCHandleToObject(instance)).Flagged = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((GameObjectContainer)GCHandledObjects.GCHandleToObject(instance)).GameObj = (GameObject)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
