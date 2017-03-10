using Net.RichardLord.Ash.Core;
using StaRTS.Assets;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.World
{
	public class ScaffoldingData
	{
		public Entity Building
		{
			get;
			private set;
		}

		public int Offset
		{
			get;
			private set;
		}

		public bool Flip
		{
			get;
			private set;
		}

		public AssetHandle Handle
		{
			get;
			set;
		}

		public GameObject GameObj
		{
			get;
			set;
		}

		public ScaffoldingData(Entity building, int offset, bool flip)
		{
			this.Handle = AssetHandle.Invalid;
			this.Building = building;
			this.Offset = offset;
			this.Flip = flip;
		}

		protected internal ScaffoldingData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScaffoldingData)GCHandledObjects.GCHandleToObject(instance)).Building);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScaffoldingData)GCHandledObjects.GCHandleToObject(instance)).Flip);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScaffoldingData)GCHandledObjects.GCHandleToObject(instance)).GameObj);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScaffoldingData)GCHandledObjects.GCHandleToObject(instance)).Handle);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScaffoldingData)GCHandledObjects.GCHandleToObject(instance)).Offset);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ScaffoldingData)GCHandledObjects.GCHandleToObject(instance)).Building = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((ScaffoldingData)GCHandledObjects.GCHandleToObject(instance)).Flip = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((ScaffoldingData)GCHandledObjects.GCHandleToObject(instance)).GameObj = (GameObject)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((ScaffoldingData)GCHandledObjects.GCHandleToObject(instance)).Handle = (AssetHandle)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((ScaffoldingData)GCHandledObjects.GCHandleToObject(instance)).Offset = *(int*)args;
			return -1L;
		}
	}
}
