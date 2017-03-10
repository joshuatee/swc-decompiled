using System;

namespace StaRTS.Main.Configs
{
	public static class ChatConstants
	{
		public const string FULL_SESSION_URL_FORMAT = "https://{0}/lp?session={1}";

		private const string URL_PREFIX = "https://startswin-prod-chat-manager.playdom.com/dsg/chat/v1/strtw/";

		public const string GAME_KEY = "87e278e1dd0a48649af0b77dc80a5ef1";

		private const string URL_POSTFIX = "?session=";

		public const string GET_CHANNEL_URL_FORMAT = "https://startswin-prod-chat-manager.playdom.com/dsg/chat/v1/strtw/getChannel?session={0}";

		public const string PUBLISH_URL_FORMAT = "https://startswin-prod-chat-manager.playdom.com/dsg/chat/v1/strtw/postMessage?session={0}&message={1}";

		public const string REPORT_URL_FORMAT = "https://startswin-prod-chat-manager.playdom.com/dsg/chat/v1/strtw/reportUser?session={0}&message={1}&reported_user_id={2}";

		public const string TAG_AND_TIME_FORMAT = "&tag={0}&time={1}";

		public const string OLD_DATE = "&tag=12&time=Mon%2C%2003%20Mar%202014%2001%3A41%3A22%20GMT";
	}
}
