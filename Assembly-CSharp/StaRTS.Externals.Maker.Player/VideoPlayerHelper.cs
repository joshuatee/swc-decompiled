using System;

namespace StaRTS.Externals.Maker.Player
{
	public class VideoPlayerHelper : IVideoPlayerHelper
	{
		public void SetAdTagURL(string url)
		{
		}

		public void SetAdLanguage(string language)
		{
		}

		public void SetAdZoneId(string zoneId)
		{
		}

		public void EnableAds(bool enable)
		{
		}

		public void SetLoadingText(string text)
		{
		}

		public void SetLoadingTextTime(float loadingTextTime)
		{
		}

		public void SetDoneButtonText(string text)
		{
		}

		public void Play(string videoURL, bool isOfficial, string videoId, string action)
		{
			VideoPlayerKeepAlive.Instance.EndPlayback("0");
		}
	}
}
