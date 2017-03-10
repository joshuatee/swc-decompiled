using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Holonet
{
	public class VideosHolonetController : IHolonetContoller
	{
		public HolonetControllerType ControllerType
		{
			get
			{
				return HolonetControllerType.Videos;
			}
		}

		public void PrepareContent(int lastTimeViewed)
		{
			Service.Get<HolonetController>().ContentPrepared(this, 0);
		}

		public VideosHolonetController()
		{
		}

		protected internal VideosHolonetController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((VideosHolonetController)GCHandledObjects.GCHandleToObject(instance)).ControllerType);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((VideosHolonetController)GCHandledObjects.GCHandleToObject(instance)).PrepareContent(*(int*)args);
			return -1L;
		}
	}
}
