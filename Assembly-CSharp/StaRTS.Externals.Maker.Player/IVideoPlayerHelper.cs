using System;

namespace StaRTS.Externals.Maker.Player
{
	public interface IVideoPlayerHelper
	{
		void SetAdTagURL(string url);

		void SetAdLanguage(string language);

		void SetAdZoneId(string zoneId);

		void EnableAds(bool enable);

		void SetLoadingText(string text);

		void SetLoadingTextTime(float loadingTextTime);

		void SetDoneButtonText(string text);

		void Play(string videoURL, bool isOfficial, string videoId, string action);
	}
}
