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

		public const string HOCKEYAPP_IOS_APPID = "502f875772d00996ccc30ce6d0f21766";

		public const string HOCKEYAPP_ANDROID_APPID = "187beabd55cb38d5567ef4788384968e";

		public const string HOCKEYAPP_AMAZON_APPID = "856535b5913c6292be955b07c91b0b42";

		public const string HOCKEYAPP_AMAZON_PACKAGEID = "com.disney.starts_ama";

		public const string HOCKEYAPP_ANDROID_PACKAGEID = "com.lucasarts.starts_goo";

		public const string GOOGLE_PLAY_API_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAhzTmZoQS0UXCBGd7Sh6w6PfH5LWv36WcZWlRuJWoKv+0Gk0rEL3+/HoFa5d6OpBwLCm7/yeKkl+doTaqKWKEkQjRDRKF9000c1xx6hNyr917cATZK3fL+D+hVlU7GhQtpRdbhUUx10mtuej15XsbeiQRg54XHOBqjZ+p+dTYsxymNVeNm/jj7qEYCRFEkrmsq+urUQ/hD5Oa3riYxUniqNi3JVgvpzzNNTHnnu09lpGv/EGJZg/Vc+uwFjyuk3pVsaIx1UgHR2kuTHsDuLA1LMJkrEowukebdeAmz1Bjby5i8/5JPImQJl8DS+7vOXe1/5G1SfTq+voqmAqVmfouWQIDAQAB";

		public const string TAPJOY_APPKEY_ANDROID = "706e08a7-97d4-4664-a26d-9e60049a7378";

		public const string TAPJOY_SECRET_ANDROID = "hjixYQXutN6jDANqtGBS";

		public const string TAPJOY_APPKEY_IOS = "5a3eeace-b104-4e6e-a6c7-78165bca5b27";

		public const string TAPJOY_SECRET_IOS = "29L5gFitdR10ahGEae7H";

		public const string BI_APP_ID = "qa_starts";

		public const string BI_URL = "https://n7-starts-client-bi.playdom.com/bi?";

		public const string NO_PROXY_BI_URL = "https://stage.api.disney.com/datatech/serverlog/v1/cp?";

		public const string EVENT_2_CLIENT_BI_LOGGING_URL = "http://n7vgd1strtsbil01.general.disney.private/bi_event2_qa";

		public const string EVENT_2_NO_PROXY_CLIENT_BI_LOGGING_URL = "https://qa.api.disney.com/datatech/log/v1/batch";

		public const string EVENT_2_AUTHORIZATION_URL = "FD B63762CD-C185-4CBF-9798-448A7AE79C7E:CD515CAC9CFE3C0C4FB05F7D7ED514B17AA3B9A0D8184337";

		public const string CELLOPHANE_UUID = "0740BD33-E5E9-4CB3-8D46-97D2AFC306E0";

		public const string CELLOPHANE_SECRET = "18AB7595FC6717FEE9A81431BC66476B9B5E06AAB9BE83EB";

		public const string AMAZON_STORE_URL = "amzn://apps/android?asin=B013J7KLJU";

		public const string APP_STORE_URL = "https://itunes.apple.com/app/id847985808?mt=8";

		public const string ITUNES_APP_ID = "847985808";

		public const string GOOGLE_PLAY_URL = "market://details?id=com.lucasarts.starts_goo";

		public const string METRO_STORE_URL = "ms-windows-store:PDP?PFN=Disney.StarWarsCommander_6rarf9sa4v8jt";

		public const string WP8_APP_ID = "ms-windows-store:reviewapp?appid=d30b584c-050d-4995-90a9-8a30bd388ed6";

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

		public const string BATTLE_GAMEPLAY_VERSION = "21.0";

		public const string PREF_PLAYER_ID = "prefPlayerId";

		public const string PREF_PLAYER_SECRET = "prefPlayerSecret";

		public const string CURRENT_ASSET_BUNDLE_CACHE_CLEAN_VERSION = "cacheCleanVersion";

		public const int LOCAL_BUNDLE_VERSION = 28;

		public const string TIMED_EVENT_DATE_FORMAT = "HH:mm,dd-MM-yyyy";

		public const string CRYPTO_ALGORITHM_SHA256 = "HmacSHA256";
	}
}
