using StaRTS.Main.Models.ValueObjects;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class RewardTag
	{
		public RewardVO Vo
		{
			get;
			set;
		}

		public RewardManager.SuccessCallback GlobalSuccess
		{
			get;
			set;
		}

		public object Cookie
		{
			get;
			set;
		}

		public RewardTag()
		{
		}

		protected internal RewardTag(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RewardTag)GCHandledObjects.GCHandleToObject(instance)).Cookie);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RewardTag)GCHandledObjects.GCHandleToObject(instance)).GlobalSuccess);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RewardTag)GCHandledObjects.GCHandleToObject(instance)).Vo);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((RewardTag)GCHandledObjects.GCHandleToObject(instance)).Cookie = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((RewardTag)GCHandledObjects.GCHandleToObject(instance)).GlobalSuccess = (RewardManager.SuccessCallback)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((RewardTag)GCHandledObjects.GCHandleToObject(instance)).Vo = (RewardVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
