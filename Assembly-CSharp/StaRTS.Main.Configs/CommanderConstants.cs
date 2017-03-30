using StaRTS.Externals.FileManagement;
using System;

namespace StaRTS.Main.Configs
{
	public static class CommanderConstants
	{
		public const float WORLD_ROTATION = 90f;

		public const float GALAXY_OFFSET = 10000f;

		public const string JSON_EXT = ".json";

		public const string JOE_EXT = ".json.joe";

		public const string LOCAL_CDN_ROOT = "http://localhost/";

		public const string DMO_ANALYTICS_KEY_IOS = "F8C4C3AD-4C93-490D-886F-79480DA2D5EB";

		public const string DMO_ANALYTICS_SECRET_IOS = "ED4AFB49-5227-4207-9E9C-B210DDED2FAA";

		public const string DMO_ANALYTICS_KEY_ANDROID = "CBA6B997-F072-4C23-978E-39A23D947BD4";

		public const string DMO_ANALYTICS_SECRET_ANDROID = "5DF2AAE8-5FD8-4373-BDB0-C2D7BBABD8B3";

		public const string DMO_ANALYTICS_KEY_AMAZON = "734233CF-327C-4D99-A7F6-DF2C338C1005";

		public const string DMO_ANALYTICS_SECRET_AMAZON = "9B41EFC2-C094-485F-873B-2F8E52061078";

		public const string HOCKEYAPP_IOS_APPID = "85b893ab43c4f8d23bb16377feb98c54";

		public const string HOCKEYAPP_ANDROID_APPID = "b37770ed61cf8efeba453416565e3a9b";

		public const string HOCKEYAPP_AMAZON_APPID = "03b72468af4214cb2ba830da6d5b0039";

		public const string HOCKEYAPP_AMAZON_PACKAGEID = "com.disney.starts_ama";

		public const string HOCKEYAPP_ANDROID_PACKAGEID = "com.lucasarts.starts_goo";

		public const string GOOGLE_PLAY_API_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAvR0OFW4ydPpr27ptG5U7LG5v6XHsIeGmRv46oHk/RP+V4NjuQrrWj/LWz/uoH/7B5bSiiZPFTXpCmmD9Zqi4EIn79A6IZf1l9oMKX0H/PqNp3PyJOwh+Egkp10UrM7KBjbiDf5YZhRKG3L8FpQl/Y9TBvfUyxb4HLAkmYUaqMgscN4GTSMHUVcjuSGkgmURYKMLSWYa1leDE8vZZ5vZCoB20Kh6PN1IcvUq/FZE1NxV1cNX44lE3DIzQuUJy+VB1Mg5aCIk6A9/GTD+BdeDKAgtf6ktiLK2oJRwe2c5quhI7cLNX3+jQJFcEdz5+pmcNeRLkkFelAd5vYU1c8WWotwIDAQAB";

		public const string TAPJOY_APPKEY_ANDROID = "64348e6c-f353-41f8-8929-34564747f2d3";

		public const string TAPJOY_SECRET_ANDROID = "CbTh6tz00S9ugMuDSbE1";

		public const string TAPJOY_APPKEY_IOS = "6b3d41d5-5581-480f-8060-f1a4af818c47";

		public const string TAPJOY_SECRET_IOS = "yvywZcB94CQF4sEXcOPi";

		public const string BI_APP_ID = "starts";

		public const string BI_URL = "https://n7-starts-client-bi.playdom.com/bi?";

		public const string NO_PROXY_BI_URL = "https://weblogger.data.disney.com/cp?";

		public const string EVENT_2_CLIENT_BI_LOGGING_URL = "https://n7-starts-client-bi.playdom.com/bi_event2";

		public const string EVENT_2_NO_PROXY_CLIENT_BI_LOGGING_URL = "https://api.disney.com/datatech/log/v1/batch";

		public const string EVENT_2_AUTHORIZATION_URL = "FD B276DCA8-9CD5-4493-85D6-75D2E500BCC9:19042B0A0628EF36039165C24A240F78F6EDBDFB8BD86BA3";

		public const string CELLOPHANE_UUID = "";

		public const string CELLOPHANE_SECRET = "";

		public const string AMAZON_STORE_URL = "amzn://apps/android?asin=B013J7KLJU";

		public const string APP_STORE_URL = "https://itunes.apple.com/app/id847985808?mt=8";

		public const string ITUNES_APP_ID = "847985808";

		public const string GOOGLE_PLAY_URL = "market://details?id=com.lucasarts.starts_goo";

		public const string STEP_TIMING_START = "start";

		public const string STEP_TIMING_INTERMEDIATE = "inter";

		public const string STEP_TIMING_END = "end";

		public const string LOCAL_DIR = "/src/maps";

		public const string DEFAULT_CEE_FILE = "cee.json";

		public const string DEFAULT_REPLAY_FILE = "replay.json";

		public const string DEFAULT_BATTLE_RECORD_FILE = "battleRecord.json";

		public const string CMS_BASE_PATCH_FILE = "patches/base.json";

		public const string FEEDBACK_FORM_URL = "";

		public const int BUILDING_SMALL = 1;

		public const int BUILDING_MEDIUM = 2;

		public const int BUILDING_LARGE = 4;

		public const int MINIMUM_BUILDING_GAP = 1;

		public const int ARROW_PADDING = 10;

		public const int ARROW_BUILDING_OFFSET = 75;

		public const FmsMode FMS_MODE = FmsMode.Versioned;

		public const string BATTLE_GAMEPLAY_VERSION = "22.0";

		public const string PREF_PLAYER_ID = "prefPlayerId";

		public const string PREF_PLAYER_SECRET = "prefPlayerSecret";

		public const string CURRENT_ASSET_BUNDLE_CACHE_CLEAN_VERSION = "cacheCleanVersion";

		public const int LOCAL_BUNDLE_VERSION = 28;

		public const string TIMED_EVENT_DATE_FORMAT = "HH:mm,dd-MM-yyyy";

		public const string CRYPTO_ALGORITHM_SHA256 = "HmacSHA256";
	}
}
