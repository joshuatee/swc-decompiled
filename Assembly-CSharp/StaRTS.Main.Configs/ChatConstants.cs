using System;

namespace StaRTS.Main.Configs
{
	public static class ChatConstants
	{
		public const string FULL_SESSION_URL_FORMAT = "https://{0}/lp?session={1}";

		private const string URL_PREFIX = "https://n7-starts-prod-chat-manager.playdom.com/dsg/chat/v1/starts/";

		public const string GAME_KEY = "e168217158d6a34d73be5220d166f312";

		private const string URL_POSTFIX = "?session=";

		public const string GET_CHANNEL_URL_FORMAT = "https://n7-starts-prod-chat-manager.playdom.com/dsg/chat/v1/starts/getChannel?session={0}";

		public const string PUBLISH_URL_FORMAT = "https://n7-starts-prod-chat-manager.playdom.com/dsg/chat/v1/starts/postMessage?session={0}&message={1}";

		public const string REPORT_URL_FORMAT = "https://n7-starts-prod-chat-manager.playdom.com/dsg/chat/v1/starts/reportUser?session={0}&message={1}&reported_user_id={2}";

		public const string TAG_AND_TIME_FORMAT = "&tag={0}&time={1}";

		public const string OLD_DATE = "&tag=12&time=Mon%2C%2003%20Mar%202014%2001%3A41%3A22%20GMT";
	}
}
