using Net.RichardLord.Ash.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models
{
	public class CurrencyCollectionTag
	{
		public Entity Building
		{
			get;
			set;
		}

		public CurrencyType Type
		{
			get;
			set;
		}

		public int Delta
		{
			get;
			set;
		}

		public CurrencyCollectionTag()
		{
		}

		protected internal CurrencyCollectionTag(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyCollectionTag)GCHandledObjects.GCHandleToObject(instance)).Building);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyCollectionTag)GCHandledObjects.GCHandleToObject(instance)).Delta);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyCollectionTag)GCHandledObjects.GCHandleToObject(instance)).Type);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((CurrencyCollectionTag)GCHandledObjects.GCHandleToObject(instance)).Building = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((CurrencyCollectionTag)GCHandledObjects.GCHandleToObject(instance)).Delta = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((CurrencyCollectionTag)GCHandledObjects.GCHandleToObject(instance)).Type = (CurrencyType)(*(int*)args);
			return -1L;
		}
	}
}
