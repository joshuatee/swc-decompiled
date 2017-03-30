using StaRTS.Externals.BI;
using StaRTS.Utils.Core;
using System;
using UnityEngine;

namespace StaRTS.Externals.Maker.Player
{
	public class VideoPlayerHelperAndroid : IVideoPlayerHelper
	{
		private const string CLASSNAME_UNITYPLAYER = "com.unity3d.player.UnityPlayer";

		private const string METHOD_CURRENTACTIVITY = "currentActivity";

		private const string CLASSNAME_MAKERVIDEOPLAYER = "com.disney.maker.MakerVideoPlayer";

		private const string METHOD_INIT = "init";

		private const string METHOD_SETADTAGURL = "setAdTagURL";

		private const string METHOD_SETADLANGUAGE = "setAdLanguage";

		private const string METHOD_SETADZONEID = "setAdZoneId";

		private const string METHOD_SETENABLEADS = "setEnableAds";

		private const string METHOD_SETLOADINGTEXT = "setLoadingText";

		private const string METHOD_SETLOADINGTEXTTIME = "setLoadingTextTime";

		private const string METHOD_SETISOFFICIAL = "setIsOfficial";

		private const string METHOD_SETLOGINFO = "setLogInfo";

		private const string METHOD_PLAY = "play";

		private AndroidJavaClass videoPlayer;

		public VideoPlayerHelperAndroid()
		{
			this.Init();
		}

		public void SetAdTagURL(string url)
		{
			if (this.videoPlayer == null)
			{
				return;
			}
			this.videoPlayer.CallStatic("setAdTagURL", new object[]
			{
				url
			});
		}

		public void SetAdLanguage(string language)
		{
			if (this.videoPlayer == null)
			{
				return;
			}
			this.videoPlayer.CallStatic("setAdLanguage", new object[]
			{
				language
			});
		}

		public void SetAdZoneId(string zoneId)
		{
			if (this.videoPlayer == null)
			{
				return;
			}
			this.videoPlayer.CallStatic("setAdZoneId", new object[]
			{
				zoneId
			});
		}

		public void EnableAds(bool enable)
		{
			if (this.videoPlayer == null)
			{
				return;
			}
			this.videoPlayer.CallStatic("setEnableAds", new object[]
			{
				enable
			});
		}

		public void SetLoadingText(string text)
		{
			if (this.videoPlayer == null)
			{
				return;
			}
			this.videoPlayer.CallStatic("setLoadingText", new object[]
			{
				text
			});
		}

		public void SetLoadingTextTime(float loadingTextTime)
		{
			if (this.videoPlayer == null)
			{
				return;
			}
			this.videoPlayer.CallStatic("setLoadingTextTime", new object[]
			{
				loadingTextTime
			});
		}

		public void SetDoneButtonText(string text)
		{
		}

		public void Play(string videoURL, bool isOfficial, string videoId, string playAction)
		{
			if (this.videoPlayer == null)
			{
				return;
			}
			if (Service.IsSet<BILoggingController>())
			{
				Service.Get<BILoggingController>().TrackGameAction("video_play", playAction, videoId, null);
			}
			this.videoPlayer.CallStatic("setIsOfficial", new object[]
			{
				isOfficial
			});
			this.videoPlayer.CallStatic("play", new object[]
			{
				videoURL
			});
		}

		private void Init()
		{
			if (this.videoPlayer != null)
			{
				return;
			}
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			this.videoPlayer = new AndroidJavaClass("com.disney.maker.MakerVideoPlayer");
			this.videoPlayer.CallStatic("init", new object[]
			{
				@static
			});
		}
	}
}
