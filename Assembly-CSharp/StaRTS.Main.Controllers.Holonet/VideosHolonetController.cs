using StaRTS.Utils.Core;
using System;

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
	}
}
