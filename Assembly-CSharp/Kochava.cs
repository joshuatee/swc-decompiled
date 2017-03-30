using JsonFx.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

[ExecuteInEditMode]
public class Kochava : MonoBehaviour
{
	public enum KochSessionTracking
	{
		full,
		basic,
		minimal,
		none
	}

	public enum KochLogLevel
	{
		error,
		warning,
		debug
	}

	public class LogEvent
	{
		public string text;

		public float time;

		public Kochava.KochLogLevel level;

		public LogEvent(string text, Kochava.KochLogLevel level)
		{
			this.text = text;
			this.time = Time.time;
			this.level = level;
		}
	}

	public class QueuedEvent
	{
		public float eventTime;

		public Dictionary<string, object> eventData;
	}

	public delegate void AttributionCallback(string callbackString);

	private const string CURRENCY_DEFAULT = "USD";

	public const string KOCHAVA_VERSION = "20160914";

	public const string KOCHAVA_PROTOCOL_VERSION = "4";

	private const int MAX_LOG_SIZE = 50;

	private const int MAX_QUEUE_SIZE = 75;

	private const int MAX_POST_TIME = 15;

	private const int POST_FAIL_RETRY_DELAY = 30;

	private const int QUEUE_KVINIT_WAIT_DELAY = 15;

	private const string API_URL = "https://control.kochava.com";

	private const string TRACKING_URL = "https://control.kochava.com/track/kvTracker?v4";

	private const string INIT_URL = "https://control.kochava.com/track/kvinit";

	private const string QUERY_URL = "https://control.kochava.com/track/kvquery";

	private const string KOCHAVA_QUEUE_STORAGE_KEY = "kochava_queue_storage";

	private const int KOCHAVA_ATTRIBUTION_INITIAL_TIMER = 7;

	private const int KOCHAVA_ATTRIBUTION_DEFAULT_TIMER = 60;

	public string kochavaAppId = string.Empty;

	public string kochavaAppIdIOS = string.Empty;

	public string kochavaAppIdAndroid = string.Empty;

	public string kochavaAppIdKindle = string.Empty;

	public string kochavaAppIdBlackberry = string.Empty;

	public string kochavaAppIdWindowsPhone = string.Empty;

	public bool debugMode;

	public bool incognitoMode;

	public bool requestAttribution;

	private bool retrieveAttribution;

	private bool debugServer;

	[HideInInspector]
	public string appVersion = string.Empty;

	[HideInInspector]
	public string partnerName = string.Empty;

	public bool appLimitAdTracking;

	[HideInInspector]
	public string userAgent = string.Empty;

	public bool adidSupressed;

	private static int device_id_delay = 60;

	private string whitelist;

	private static bool adidBlacklisted = false;

	private static Kochava.AttributionCallback attributionCallback;

	private string appIdentifier = string.Empty;

	private string appPlatform = "desktop";

	private string kochavaDeviceId = string.Empty;

	private string attributionDataStr = string.Empty;

	private List<string> devIdBlacklist = new List<string>();

	private List<string> eventNameBlacklist = new List<string>();

	public string appCurrency = "USD";

	public Kochava.KochSessionTracking sessionTracking;

	private int KVTRACKER_WAIT = 60;

	private List<Kochava.LogEvent> _EventLog = new List<Kochava.LogEvent>();

	private Dictionary<string, object> hardwareIdentifierData = new Dictionary<string, object>();

	private Dictionary<string, object> hardwareIntegrationData = new Dictionary<string, object>();

	private Dictionary<string, object> appData;

	private Queue<Kochava.QueuedEvent> eventQueue = new Queue<Kochava.QueuedEvent>();

	private float processQueueKickstartTime;

	private bool queueIsProcessing;

	private float _eventPostingTime;

	private bool doReportLocation;

	private int locationAccuracy = 50;

	private int locationTimeout = 15;

	private int locationStaleness = 15;

	private int iAdAttributionAttempts = 3;

	private int iAdAttributionWait = 20;

	private int iAdRetryWait = 10;

	private bool send_id_updates;

	private static Kochava _S;

	private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	private static float uptimeDelta;

	private static float uptimeDeltaUpdate;

	public static bool DebugMode
	{
		get
		{
			return Kochava._S.debugMode;
		}
		set
		{
			Kochava._S.debugMode = value;
		}
	}

	public static bool IncognitoMode
	{
		get
		{
			return Kochava._S.incognitoMode;
		}
		set
		{
			Kochava._S.incognitoMode = value;
		}
	}

	public static bool RequestAttribution
	{
		get
		{
			return Kochava._S.requestAttribution;
		}
		set
		{
			Kochava._S.requestAttribution = value;
		}
	}

	public static bool AppLimitAdTracking
	{
		get
		{
			return Kochava._S.appLimitAdTracking;
		}
		set
		{
			Kochava._S.appLimitAdTracking = value;
		}
	}

	public static bool AdidSupressed
	{
		get
		{
			return Kochava._S.adidSupressed;
		}
		set
		{
			Kochava._S.adidSupressed = value;
		}
	}

	public static string AttributionDataStr
	{
		get
		{
			return Kochava._S.attributionDataStr;
		}
		set
		{
			Kochava._S.attributionDataStr = value;
		}
	}

	public static List<string> DevIdBlacklist
	{
		get
		{
			return Kochava._S.devIdBlacklist;
		}
		set
		{
			Kochava._S.devIdBlacklist = value;
		}
	}

	public static List<string> EventNameBlacklist
	{
		get
		{
			return Kochava._S.eventNameBlacklist;
		}
		set
		{
			Kochava._S.eventNameBlacklist = value;
		}
	}

	public static Kochava.KochSessionTracking SessionTracking
	{
		get
		{
			return Kochava._S.sessionTracking;
		}
		set
		{
			Kochava._S.sessionTracking = value;
		}
	}

	public static List<Kochava.LogEvent> EventLog
	{
		get
		{
			return Kochava._S._EventLog;
		}
	}

	public static int eventQueueLength
	{
		get
		{
			return Kochava._S.eventQueue.Count;
		}
	}

	public static float eventPostingTime
	{
		get
		{
			return Kochava._S._eventPostingTime;
		}
	}

	public static void SetAttributionCallback(Kochava.AttributionCallback callback)
	{
		Kochava.attributionCallback = callback;
	}

	public void Awake()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (Kochava._S)
		{
			this.Log("detected two concurrent integration objects - please place your integration object in a scene which will not be reloaded.");
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (Kochava._S == null)
		{
			Kochava._S = this;
		}
		base.gameObject.name = "_Kochava Analytics";
		this.Log("Kochava SDK Initialized.\nVersion: 20160914\nProtocol Version: 4", Kochava.KochLogLevel.debug);
		if (this.kochavaAppId.Length == 0 && this.kochavaAppIdIOS.Length == 0 && this.kochavaAppIdAndroid.Length == 0 && this.kochavaAppIdKindle.Length == 0 && this.kochavaAppIdBlackberry.Length == 0 && this.kochavaAppIdWindowsPhone.Length == 0 && this.partnerName.Length == 0)
		{
			this.Log("No Kochava App Id or Partner Name - SDK will terminate");
			UnityEngine.Object.Destroy(base.gameObject);
		}
		this.loadQueue();
	}

	public void Start()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (Kochava._S == null)
		{
			Kochava._S = this;
		}
		this.Init();
	}

	public void OnEnable()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (Kochava._S == null)
		{
			Kochava._S = this;
		}
	}

	private void Init()
	{
		try
		{
			try
			{
				AndroidJNIHelper.debug = true;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
					AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getApplicationContext", new object[0]);
					AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.kochava.android.tracker.lite.KochavaSDKLite");
					string text = androidJavaClass2.CallStatic<string>("GetExternalKochavaDeviceIdentifiers_Android", new object[]
					{
						androidJavaObject,
						Kochava.AdidSupressed
					});
					this.Log("Hardware Integration Diagnostics: " + text);
					this.hardwareIdentifierData = JsonReader.Deserialize<Dictionary<string, object>>(text);
					this.Log(string.Concat(new object[]
					{
						"Received (",
						this.hardwareIdentifierData.Count,
						") parameters from Hardware Integration Library (identifiers): ",
						text
					}));
				}
			}
			catch (Exception arg)
			{
				this.Log("Failed GetExternalKochavaDeviceIdentifiers_Android: " + arg, Kochava.KochLogLevel.warning);
			}
			if (this.hardwareIdentifierData.ContainsKey("user_agent"))
			{
				this.userAgent = this.hardwareIdentifierData["user_agent"].ToString();
				this.Log("userAgent set to: " + this.userAgent, Kochava.KochLogLevel.debug);
			}
			if (this.userAgent.Contains("kindle") || this.userAgent.Contains("silk"))
			{
				this.appPlatform = "kindle";
				if (this.kochavaAppIdKindle != string.Empty)
				{
					this.kochavaAppId = this.kochavaAppIdKindle;
				}
			}
			else
			{
				this.appPlatform = "android";
				if (this.kochavaAppIdAndroid != string.Empty)
				{
					this.kochavaAppId = this.kochavaAppIdAndroid;
				}
			}
			if (this.hardwareIdentifierData.ContainsKey("package"))
			{
				this.appIdentifier = this.hardwareIdentifierData["package"].ToString();
				this.Log("appIdentifier set to: " + this.appIdentifier, Kochava.KochLogLevel.debug);
			}
			if (PlayerPrefs.HasKey("kochava_app_id"))
			{
				this.kochavaAppId = PlayerPrefs.GetString("kochava_app_id");
				this.Log("Loaded kochava_app_id from persistent storage: " + this.kochavaAppId, Kochava.KochLogLevel.debug);
			}
			if (PlayerPrefs.HasKey("kochava_device_id"))
			{
				this.kochavaDeviceId = PlayerPrefs.GetString("kochava_device_id");
				this.Log("Loaded kochava_device_id from persistent storage: " + this.kochavaDeviceId, Kochava.KochLogLevel.debug);
			}
			else if (this.incognitoMode)
			{
				this.kochavaDeviceId = "KA" + Guid.NewGuid().ToString().Replace("-", string.Empty);
				this.Log("Using autogenerated \"incognito\" kochava_device_id: " + this.kochavaDeviceId, Kochava.KochLogLevel.debug);
			}
			else
			{
				string a = string.Empty;
				if (PlayerPrefs.HasKey("data_orig_kochava_device_id"))
				{
					a = PlayerPrefs.GetString("data_orig_kochava_device_id");
				}
				if (a != string.Empty)
				{
					this.kochavaDeviceId = a;
					this.Log("Using \"orig\" kochava_device_id: " + this.kochavaDeviceId, Kochava.KochLogLevel.debug);
				}
				else
				{
					this.kochavaDeviceId = "KU" + Guid.NewGuid().ToString().Replace("-", string.Empty);
					this.Log("Using autogenerated kochava_device_id: " + this.kochavaDeviceId, Kochava.KochLogLevel.debug);
				}
			}
			if (!PlayerPrefs.HasKey("data_orig_kochava_app_id") && this.kochavaAppId != string.Empty)
			{
				PlayerPrefs.SetString("data_orig_kochava_app_id", this.kochavaAppId);
			}
			if (!PlayerPrefs.HasKey("data_orig_kochava_device_id") && this.kochavaDeviceId != string.Empty)
			{
				PlayerPrefs.SetString("data_orig_kochava_device_id", this.kochavaDeviceId);
			}
			if (!PlayerPrefs.HasKey("data_orig_session_tracking"))
			{
				PlayerPrefs.SetString("data_orig_session_tracking", this.sessionTracking.ToString());
			}
			if (!PlayerPrefs.HasKey("data_orig_currency") && this.appCurrency != string.Empty)
			{
				PlayerPrefs.SetString("data_orig_currency", this.appCurrency);
			}
			if (PlayerPrefs.HasKey("currency"))
			{
				this.appCurrency = PlayerPrefs.GetString("currency");
				this.Log("Loaded currency from persistent storage: " + this.appCurrency, Kochava.KochLogLevel.debug);
			}
			if (PlayerPrefs.HasKey("blacklist"))
			{
				try
				{
					string @string = PlayerPrefs.GetString("blacklist");
					this.devIdBlacklist = new List<string>();
					string[] array = JsonReader.Deserialize<string[]>(@string);
					for (int i = array.Length - 1; i >= 0; i--)
					{
						this.devIdBlacklist.Add(array[i]);
					}
					this.Log("Loaded device_id blacklist from persistent storage: " + @string, Kochava.KochLogLevel.debug);
				}
				catch (Exception arg2)
				{
					this.Log("Failed loading device_id blacklist from persistent storage: " + arg2, Kochava.KochLogLevel.warning);
				}
			}
			if (PlayerPrefs.HasKey("attribution"))
			{
				try
				{
					this.attributionDataStr = PlayerPrefs.GetString("attribution");
					this.Log("Loaded attribution data from persistent storage: " + this.attributionDataStr, Kochava.KochLogLevel.debug);
				}
				catch (Exception arg3)
				{
					this.Log("Failed loading attribution data from persistent storage: " + arg3, Kochava.KochLogLevel.warning);
				}
			}
			if (PlayerPrefs.HasKey("session_tracking"))
			{
				try
				{
					string string2 = PlayerPrefs.GetString("session_tracking");
					this.sessionTracking = (Kochava.KochSessionTracking)((int)Enum.Parse(typeof(Kochava.KochSessionTracking), string2, true));
					this.Log("Loaded session tracking mode from persistent storage: " + string2, Kochava.KochLogLevel.debug);
				}
				catch (Exception arg4)
				{
					this.Log("Failed loading session tracking mode from persistent storage: " + arg4, Kochava.KochLogLevel.warning);
				}
			}
			if (!PlayerPrefs.HasKey("kvinit_wait"))
			{
				PlayerPrefs.SetString("kvinit_wait", "60");
			}
			if (!PlayerPrefs.HasKey("kvinit_last_sent"))
			{
				PlayerPrefs.SetString("kvinit_last_sent", "0");
			}
			if (!PlayerPrefs.HasKey("kvtracker_wait"))
			{
				PlayerPrefs.SetString("kvtracker_wait", "60");
			}
			if (!PlayerPrefs.HasKey("last_location_time"))
			{
				PlayerPrefs.SetString("last_location_time", "0");
			}
			double num = double.Parse(PlayerPrefs.GetString("kvinit_last_sent"));
			double num2 = Kochava.CurrentTime();
			double num3 = double.Parse(PlayerPrefs.GetString("kvinit_wait"));
			if (num2 - num > num3)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>
				{
					{
						"partner_name",
						this.partnerName
					},
					{
						"package",
						this.appIdentifier
					},
					{
						"platform",
						this.appPlatform
					},
					{
						"session_tracking",
						this.sessionTracking.ToString()
					},
					{
						"currency",
						(this.appCurrency != null && !(this.appCurrency == string.Empty)) ? this.appCurrency : "USD"
					},
					{
						"os_version",
						SystemInfo.operatingSystem
					}
				};
				if (this.requestAttribution && !PlayerPrefs.HasKey("attribution"))
				{
					this.retrieveAttribution = true;
				}
				this.Log("retrieve attrib: " + this.retrieveAttribution);
				if (this.hardwareIdentifierData.ContainsKey("IDFA"))
				{
					dictionary.Add("idfa", this.hardwareIdentifierData["IDFA"]);
				}
				if (this.hardwareIdentifierData.ContainsKey("IDFV"))
				{
					dictionary.Add("idfv", this.hardwareIdentifierData["IDFV"]);
				}
				Dictionary<string, object> value = new Dictionary<string, object>
				{
					{
						"kochava_app_id",
						PlayerPrefs.GetString("data_orig_kochava_app_id")
					},
					{
						"kochava_device_id",
						PlayerPrefs.GetString("data_orig_kochava_device_id")
					},
					{
						"session_tracking",
						PlayerPrefs.GetString("data_orig_session_tracking")
					},
					{
						"currency",
						PlayerPrefs.GetString("data_orig_currency")
					}
				};
				Dictionary<string, object> value2 = new Dictionary<string, object>
				{
					{
						"action",
						"init"
					},
					{
						"data",
						dictionary
					},
					{
						"data_orig",
						value
					},
					{
						"kochava_app_id",
						this.kochavaAppId
					},
					{
						"kochava_device_id",
						this.kochavaDeviceId
					},
					{
						"sdk_version",
						"Unity3D-20160914"
					},
					{
						"sdk_protocol",
						"4"
					}
				};
				base.StartCoroutine(this.Init_KV(JsonWriter.Serialize(value2)));
			}
			else
			{
				this.appData = new Dictionary<string, object>
				{
					{
						"kochava_app_id",
						this.kochavaAppId
					},
					{
						"kochava_device_id",
						this.kochavaDeviceId
					},
					{
						"sdk_version",
						"Unity3D-20160914"
					},
					{
						"sdk_protocol",
						"4"
					}
				};
				if (PlayerPrefs.HasKey("eventname_blacklist"))
				{
					string[] array2 = JsonReader.Deserialize<string[]>(PlayerPrefs.GetString("eventname_blacklist"));
					List<string> list = new List<string>();
					for (int j = 0; j < array2.Length; j++)
					{
						list.Add(array2[j]);
					}
					this.eventNameBlacklist = list;
				}
			}
		}
		catch (Exception arg5)
		{
			this.Log("Overall failure in init: " + arg5, Kochava.KochLogLevel.warning);
		}
	}

	[DebuggerHidden]
	private IEnumerator Init_KV(string postData)
	{
		Kochava.<Init_KV>c__IteratorC <Init_KV>c__IteratorC = new Kochava.<Init_KV>c__IteratorC();
		<Init_KV>c__IteratorC.postData = postData;
		<Init_KV>c__IteratorC.<$>postData = postData;
		<Init_KV>c__IteratorC.<>f__this = this;
		return <Init_KV>c__IteratorC;
	}

	private void DeviceInformationCallback(string deviceInfo)
	{
		try
		{
			this.hardwareIntegrationData = JsonReader.Deserialize<Dictionary<string, object>>(deviceInfo);
			this.Log(string.Concat(new object[]
			{
				"Received (",
				this.hardwareIntegrationData.Count,
				") parameters from Hardware Integration Library (device info): ",
				deviceInfo
			}));
		}
		catch (Exception arg)
		{
			this.Log("Failed Deserialize hardwareIntegrationData: " + arg, Kochava.KochLogLevel.warning);
		}
		if (!PlayerPrefs.HasKey("watchlistProperties"))
		{
			this.initInitial();
		}
		else
		{
			this.ScanWatchlistChanges();
		}
	}

	public static void InitInitial()
	{
		Kochava._S.initInitial();
	}

	private void initInitial()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		try
		{
			dictionary.Add("device", SystemInfo.deviceModel);
			if (this.hardwareIntegrationData.ContainsKey("package"))
			{
				dictionary.Add("package", this.hardwareIntegrationData["package"]);
			}
			else
			{
				dictionary.Add("package", this.appIdentifier);
			}
			if (this.hardwareIntegrationData.ContainsKey("app_version"))
			{
				dictionary.Add("app_version", this.hardwareIntegrationData["app_version"]);
			}
			else
			{
				dictionary.Add("app_version", this.appVersion);
			}
			if (this.hardwareIntegrationData.ContainsKey("app_short_string"))
			{
				dictionary.Add("app_short_string", this.hardwareIntegrationData["app_short_string"]);
			}
			else
			{
				dictionary.Add("app_short_string", this.appVersion);
			}
			dictionary.Add("currency", (!(this.appCurrency == string.Empty)) ? this.appCurrency : "USD");
			if (!this.devIdBlacklist.Contains("screen_size"))
			{
				dictionary.Add("disp_h", Screen.height);
				dictionary.Add("disp_w", Screen.width);
			}
			if (!this.devIdBlacklist.Contains("device_orientation") && this.hardwareIntegrationData.ContainsKey("device_orientation"))
			{
				dictionary.Add("device_orientation", this.hardwareIntegrationData["device_orientation"]);
			}
			if (!this.devIdBlacklist.Contains("screen_brightness") && this.hardwareIntegrationData.ContainsKey("screen_brightness"))
			{
				dictionary.Add("screen_brightness", this.hardwareIntegrationData["screen_brightness"]);
			}
			if (!this.devIdBlacklist.Contains("network_conn_type"))
			{
				bool flag = false;
				bool flag2 = false;
				NetworkReachability internetReachability = Application.internetReachability;
				if (internetReachability != NetworkReachability.ReachableViaCarrierDataNetwork)
				{
					if (internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
					{
						flag2 = true;
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					dictionary.Add("network_conn_type", "cellular");
				}
				else if (flag2)
				{
					dictionary.Add("network_conn_type", "wifi");
				}
			}
			dictionary.Add("os_version", SystemInfo.operatingSystem);
			dictionary.Add("app_limit_tracking", this.appLimitAdTracking);
			if (!this.devIdBlacklist.Contains("hardware"))
			{
				dictionary.Add("device_processor", SystemInfo.processorType);
				dictionary.Add("device_cores", SystemInfo.processorCount);
				dictionary.Add("device_memory", SystemInfo.systemMemorySize);
				dictionary.Add("graphics_memory_size", SystemInfo.graphicsMemorySize);
				dictionary.Add("graphics_device_name", SystemInfo.graphicsDeviceName);
				dictionary.Add("graphics_device_vendor", SystemInfo.graphicsDeviceVendor);
				dictionary.Add("graphics_device_id", SystemInfo.graphicsDeviceID);
				dictionary.Add("graphics_device_vendor_id", SystemInfo.graphicsDeviceVendorID);
				dictionary.Add("graphics_device_version", SystemInfo.graphicsDeviceVersion);
				dictionary.Add("graphics_shader_level", SystemInfo.graphicsShaderLevel);
			}
			if (!this.devIdBlacklist.Contains("is_genuine") && Application.genuineCheckAvailable)
			{
				dictionary.Add("is_genuine", (!Application.genuine) ? "0" : "1");
			}
			if (!this.devIdBlacklist.Contains("idfa") && this.hardwareIntegrationData.ContainsKey("IDFA"))
			{
				dictionary.Add("idfa", this.hardwareIntegrationData["IDFA"]);
			}
			if (!this.devIdBlacklist.Contains("idfv") && this.hardwareIntegrationData.ContainsKey("IDFV"))
			{
				dictionary.Add("idfv", this.hardwareIntegrationData["IDFV"]);
			}
			if (!this.devIdBlacklist.Contains("udid") && this.hardwareIntegrationData.ContainsKey("UDID"))
			{
				dictionary.Add("udid", this.hardwareIntegrationData["UDID"]);
			}
			if (!this.devIdBlacklist.Contains("iad_attribution") && this.hardwareIntegrationData.ContainsKey("iad_attribution"))
			{
				dictionary.Add("iad_attribution", this.hardwareIntegrationData["iad_attribution"]);
			}
			if (!this.devIdBlacklist.Contains("app_purchase_date") && this.hardwareIntegrationData.ContainsKey("app_purchase_date"))
			{
				dictionary.Add("app_purchase_date", this.hardwareIntegrationData["app_purchase_date"]);
			}
			if (!this.devIdBlacklist.Contains("iad_impression_date") && this.hardwareIntegrationData.ContainsKey("iad_impression_date"))
			{
				dictionary.Add("iad_impression_date", this.hardwareIntegrationData["iad_impression_date"]);
			}
			if (!this.devIdBlacklist.Contains("iad_attribution_details") && this.hardwareIntegrationData.ContainsKey("iad_attribution_details"))
			{
				dictionary.Add("iad_attribution_details", this.hardwareIntegrationData["iad_attribution_details"]);
			}
			if (!this.devIdBlacklist.Contains("android_id") && this.hardwareIntegrationData.ContainsKey("android_id"))
			{
				dictionary.Add("android_id", this.hardwareIntegrationData["android_id"]);
			}
			if (!this.devIdBlacklist.Contains("adid") && this.hardwareIntegrationData.ContainsKey("adid"))
			{
				dictionary.Add("adid", this.hardwareIntegrationData["adid"]);
			}
			if (!this.devIdBlacklist.Contains("fb_attribution_id") && this.hardwareIntegrationData.ContainsKey("fb_attribution_id"))
			{
				dictionary.Add("fb_attribution_id", this.hardwareIntegrationData["fb_attribution_id"]);
			}
			if (this.hardwareIntegrationData.ContainsKey("device_limit_tracking"))
			{
				dictionary.Add("device_limit_tracking", this.hardwareIntegrationData["device_limit_tracking"]);
			}
			if (!this.devIdBlacklist.Contains("bssid") && this.hardwareIntegrationData.ContainsKey("bssid"))
			{
				dictionary.Add("bssid", this.hardwareIntegrationData["bssid"]);
			}
			if (!this.devIdBlacklist.Contains("carrier_name") && this.hardwareIntegrationData.ContainsKey("carrier_name"))
			{
				dictionary.Add("carrier_name", this.hardwareIntegrationData["carrier_name"]);
			}
			if (!this.devIdBlacklist.Contains("volume") && this.hardwareIntegrationData.ContainsKey("volume"))
			{
				dictionary.Add("volume", this.hardwareIntegrationData["volume"]);
			}
			if (this.hardwareIntegrationData.ContainsKey("language"))
			{
				dictionary.Add("language", this.hardwareIntegrationData["language"]);
			}
			if (this.hardwareIntegrationData.ContainsKey("ids"))
			{
				dictionary.Add("ids", this.hardwareIntegrationData["ids"]);
			}
			if (this.hardwareIntegrationData.ContainsKey("conversion_type"))
			{
				dictionary.Add("conversion_type", this.hardwareIntegrationData["conversion_type"]);
			}
			if (this.hardwareIntegrationData.ContainsKey("conversion_data"))
			{
				dictionary.Add("conversion_data", this.hardwareIntegrationData["conversion_data"]);
			}
			dictionary.Add("usertime", (uint)Kochava.CurrentTime());
			if ((uint)Time.time > 0u)
			{
				dictionary.Add("uptime", (uint)Time.time);
			}
			float num = Kochava.UptimeDelta();
			if (num >= 1f)
			{
				dictionary.Add("updelta", (uint)num);
			}
		}
		catch (Exception arg)
		{
			this.Log("Error preparing initial event: " + arg, Kochava.KochLogLevel.error);
		}
		finally
		{
			this._fireEvent("initial", dictionary);
			if (this.retrieveAttribution)
			{
				int num2 = 7;
				if (PlayerPrefs.HasKey("getattribution_wait"))
				{
					string @string = PlayerPrefs.GetString("getattribution_wait");
					num2 = int.Parse(@string);
				}
				this.Log("Will check for attribution in: " + num2);
				base.StartCoroutine("KochavaAttributionTimerFired", num2);
			}
		}
		try
		{
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			if (this.hardwareIntegrationData.ContainsKey("device_limit_tracking"))
			{
				dictionary2.Add("device_limit_tracking", this.hardwareIntegrationData["device_limit_tracking"].ToString());
			}
			dictionary2.Add("os_version", SystemInfo.operatingSystem);
			dictionary2.Add("app_limit_tracking", this.appLimitAdTracking);
			if (this.hardwareIntegrationData.ContainsKey("language"))
			{
				dictionary2.Add("language", this.hardwareIntegrationData["language"].ToString());
			}
			if (this.hardwareIntegrationData.ContainsKey("app_version"))
			{
				dictionary2.Add("app_version", this.hardwareIntegrationData["app_version"].ToString());
			}
			else
			{
				dictionary2.Add("app_version", this.appVersion);
			}
			if (this.hardwareIntegrationData.ContainsKey("app_short_string"))
			{
				dictionary2.Add("app_short_string", this.hardwareIntegrationData["app_short_string"].ToString());
			}
			else
			{
				dictionary2.Add("app_short_string", this.appVersion);
			}
			if (!this.devIdBlacklist.Contains("idfa") && this.hardwareIntegrationData.ContainsKey("IDFA"))
			{
				dictionary2.Add("idfa", this.hardwareIntegrationData["IDFA"].ToString());
			}
			if (!this.devIdBlacklist.Contains("adid") && this.hardwareIntegrationData.ContainsKey("adid"))
			{
				dictionary2.Add("adid", this.hardwareIntegrationData["adid"]);
			}
			string text = JsonWriter.Serialize(dictionary2);
			PlayerPrefs.SetString("watchlistProperties", text);
			this.Log("watchlistString: " + text);
		}
		catch (Exception arg2)
		{
			this.Log("Error setting watchlist: " + arg2, Kochava.KochLogLevel.error);
		}
	}

	public void ScanWatchlistChanges()
	{
		try
		{
			if (PlayerPrefs.HasKey("watchlistProperties"))
			{
				string @string = PlayerPrefs.GetString("watchlistProperties");
				this.Log("retrieve watchlist: " + @string);
				Dictionary<string, object> dictionary = JsonReader.Deserialize<Dictionary<string, object>>(@string);
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				if (dictionary.ContainsKey("app_version"))
				{
					if (this.hardwareIntegrationData.ContainsKey("app_version"))
					{
						if (dictionary["app_version"].ToString() != this.hardwareIntegrationData["app_version"].ToString())
						{
							dictionary2.Add("app_version", this.hardwareIntegrationData["app_version"].ToString());
							dictionary["app_version"] = this.hardwareIntegrationData["app_version"].ToString();
						}
					}
					else if (dictionary["app_version"].ToString() != this.appVersion)
					{
						dictionary2.Add("app_version", this.appVersion);
						dictionary["app_version"] = this.appVersion;
					}
				}
				if (dictionary.ContainsKey("app_short_string"))
				{
					if (this.hardwareIntegrationData.ContainsKey("app_short_string"))
					{
						if (dictionary["app_short_string"].ToString() != this.hardwareIntegrationData["app_short_string"].ToString())
						{
							dictionary2.Add("app_short_string", this.hardwareIntegrationData["app_short_string"].ToString());
							dictionary["app_short_string"] = this.hardwareIntegrationData["app_short_string"].ToString();
						}
					}
					else if (dictionary["app_short_string"].ToString() != this.appVersion)
					{
						dictionary2.Add("app_short_string", this.appVersion);
						dictionary["app_short_string"] = this.appVersion;
					}
				}
				if (dictionary.ContainsKey("os_version") && dictionary["os_version"].ToString() != SystemInfo.operatingSystem)
				{
					dictionary2.Add("os_version", SystemInfo.operatingSystem);
					dictionary["os_version"] = SystemInfo.operatingSystem;
				}
				if (dictionary.ContainsKey("language") && this.hardwareIntegrationData.ContainsKey("language") && dictionary["language"].ToString() != this.hardwareIntegrationData["language"].ToString())
				{
					dictionary2.Add("language", this.hardwareIntegrationData["language"].ToString());
					dictionary["language"] = this.hardwareIntegrationData["language"].ToString();
				}
				if (dictionary.ContainsKey("device_limit_tracking") && this.hardwareIntegrationData.ContainsKey("device_limit_tracking") && dictionary["device_limit_tracking"].ToString() != this.hardwareIntegrationData["device_limit_tracking"].ToString())
				{
					dictionary2.Add("device_limit_tracking", this.hardwareIntegrationData["device_limit_tracking"].ToString());
					dictionary["device_limit_tracking"] = this.hardwareIntegrationData["device_limit_tracking"].ToString();
				}
				if (dictionary.ContainsKey("app_limit_tracking") && bool.Parse(dictionary["app_limit_tracking"].ToString()) != this.appLimitAdTracking)
				{
					dictionary2.Add("app_limit_tracking", this.appLimitAdTracking);
					dictionary["app_limit_tracking"] = this.appLimitAdTracking;
				}
				if (this.send_id_updates)
				{
					if (!this.devIdBlacklist.Contains("idfa") && dictionary.ContainsKey("idfa") && this.hardwareIntegrationData.ContainsKey("IDFA") && dictionary["idfa"].ToString() != this.hardwareIntegrationData["IDFA"].ToString())
					{
						dictionary2.Add("idfa", this.hardwareIntegrationData["IDFA"].ToString());
						dictionary["idfa"] = this.hardwareIntegrationData["IDFA"].ToString();
					}
					if (!this.devIdBlacklist.Contains("adid") && dictionary.ContainsKey("adid") && this.hardwareIntegrationData.ContainsKey("adid") && dictionary["adid"].ToString() != this.hardwareIntegrationData["adid"].ToString())
					{
						dictionary2.Add("adid", this.hardwareIntegrationData["adid"].ToString());
						dictionary["adid"] = this.hardwareIntegrationData["adid"].ToString();
					}
				}
				if (dictionary2.Count > 0)
				{
					string text = JsonWriter.Serialize(dictionary);
					string str = JsonWriter.Serialize(dictionary2);
					this.Log("final watchlist: " + text);
					this.Log("changeData: " + str);
					PlayerPrefs.SetString("watchlistProperties", text);
					Kochava._S._fireEvent("update", dictionary2);
				}
				else
				{
					this.Log("No watchdata changed");
				}
			}
		}
		catch (Exception arg)
		{
			this.Log("Error scanning watchlist: " + arg, Kochava.KochLogLevel.error);
		}
	}

	public void Update()
	{
		if (Application.isPlaying)
		{
			if (this.processQueueKickstartTime != 0f && Time.time > this.processQueueKickstartTime)
			{
				this.processQueueKickstartTime = 0f;
				base.StartCoroutine("processQueue");
			}
		}
	}

	public static string GetKochavaDeviceId()
	{
		if (PlayerPrefs.HasKey("kochava_device_id"))
		{
			return PlayerPrefs.GetString("kochava_device_id");
		}
		return string.Empty;
	}

	public static void SetLimitAdTracking(bool appLimitTracking)
	{
		Kochava.AppLimitAdTracking = appLimitTracking;
		Kochava._S.ScanWatchlistChanges();
	}

	public static void FireEvent(Dictionary<string, object> properties)
	{
		Kochava._S._fireEvent("event", properties);
	}

	public static void FireEvent(Hashtable propHash)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (DictionaryEntry dictionaryEntry in propHash)
		{
			dictionary.Add((string)dictionaryEntry.Key, dictionaryEntry.Value);
		}
		Kochava._S._fireEvent("event", dictionary);
	}

	public static void FireEvent(string eventName, string eventData)
	{
		if (!Kochava.EventNameBlacklist.Contains(eventName))
		{
			Kochava._S._fireEvent("event", new Dictionary<string, object>
			{
				{
					"event_name",
					eventName
				},
				{
					"event_data",
					(eventData != null) ? eventData : string.Empty
				}
			});
		}
	}

	public static void FireEventStandard(FireEventParameters fireEventParameters)
	{
		if (fireEventParameters == null || fireEventParameters.eventName == null || fireEventParameters.eventName.Length < 1)
		{
			return;
		}
		string value = JsonWriter.Serialize(fireEventParameters.valuePayload) ?? string.Empty;
		if (!Kochava.EventNameBlacklist.Contains(fireEventParameters.eventName))
		{
			Kochava._S._fireEvent("event", new Dictionary<string, object>
			{
				{
					"event_name",
					fireEventParameters.eventName
				},
				{
					"event_data",
					value
				},
				{
					"event_standard",
					true.ToString()
				}
			});
		}
	}

	public static void FireSpatialEvent(string eventName, float x, float y)
	{
		Kochava.FireSpatialEvent(eventName, x, y, 0f, string.Empty);
	}

	public static void FireSpatialEvent(string eventName, float x, float y, string eventData)
	{
		Kochava.FireSpatialEvent(eventName, x, y, 0f, (eventData != null) ? eventData : string.Empty);
	}

	public static void FireSpatialEvent(string eventName, float x, float y, float z)
	{
		Kochava.FireSpatialEvent(eventName, x, y, z, string.Empty);
	}

	public static void FireSpatialEvent(string eventName, float x, float y, float z, string eventData)
	{
		if (!Kochava.EventNameBlacklist.Contains(eventName))
		{
			Kochava._S._fireEvent("spatial", new Dictionary<string, object>
			{
				{
					"event_name",
					eventName
				},
				{
					"event_data",
					eventData
				},
				{
					"x",
					x
				},
				{
					"y",
					y
				},
				{
					"z",
					z
				}
			});
		}
	}

	public static void IdentityLink(string key, string val)
	{
		Kochava._S._fireEvent("identityLink", new Dictionary<string, object>
		{
			{
				key,
				val
			}
		});
	}

	public static void IdentityLink(Dictionary<string, object> identities)
	{
		Kochava._S._fireEvent("identityLink", identities);
	}

	public static void DeeplinkEvent(string uri, string sourceApp)
	{
		Kochava._S._fireEvent("deeplink", new Dictionary<string, object>
		{
			{
				"uri",
				uri
			},
			{
				"source_app",
				sourceApp
			}
		});
	}

	private void _fireEvent(string eventAction, Dictionary<string, object> eventData)
	{
		if (eventData.ContainsKey("event_name") && (eventData["event_name"] == null || eventData["event_name"].Equals(string.Empty)))
		{
			this.Log("Cannot create event with null/empty event name.");
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		if (!eventData.ContainsKey("usertime"))
		{
			eventData.Add("usertime", (uint)Kochava.CurrentTime());
		}
		if (!eventData.ContainsKey("uptime") && (uint)Time.time > 0u)
		{
			eventData.Add("uptime", (uint)Time.time);
		}
		float num = Kochava.UptimeDelta();
		if (!eventData.ContainsKey("updelta") && num >= 1f)
		{
			eventData.Add("updelta", (uint)num);
		}
		dictionary.Add("action", eventAction);
		dictionary.Add("data", eventData);
		if (Kochava.eventPostingTime != 0f)
		{
			dictionary.Add("last_post_time", Kochava.eventPostingTime);
		}
		if (this.debugMode)
		{
			dictionary.Add("debug", true);
		}
		if (this.debugServer)
		{
			dictionary.Add("debugServer", true);
		}
		bool isInitial = false;
		if (eventAction == "initial")
		{
			isInitial = true;
		}
		this.postEvent(dictionary, isInitial);
	}

	private void postEvent(Dictionary<string, object> data, bool isInitial)
	{
		Kochava.QueuedEvent queuedEvent = new Kochava.QueuedEvent();
		queuedEvent.eventTime = Time.time;
		queuedEvent.eventData = data;
		this.eventQueue.Enqueue(queuedEvent);
		if (isInitial)
		{
			base.StartCoroutine("processQueue");
		}
		else if (this.eventQueue.Count >= 75)
		{
			base.StartCoroutine("processQueue");
		}
		else
		{
			this.processQueueKickstartTime = Time.time + (float)this.KVTRACKER_WAIT;
		}
	}

	private void LocationReportCallback(string locationInfo)
	{
		this.Log("location info: " + locationInfo);
		PlayerPrefs.SetString("last_location_time", Kochava.CurrentTime().ToString());
		try
		{
			Dictionary<string, object> value = new Dictionary<string, object>();
			value = JsonReader.Deserialize<Dictionary<string, object>>(locationInfo);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("location", value);
			Kochava._S._fireEvent("update", dictionary);
		}
		catch (Exception arg)
		{
			this.Log("Failed Deserialize hardwareIntegrationData: " + arg, Kochava.KochLogLevel.warning);
		}
	}

	[DebuggerHidden]
	private IEnumerator processQueue()
	{
		Kochava.<processQueue>c__IteratorD <processQueue>c__IteratorD = new Kochava.<processQueue>c__IteratorD();
		<processQueue>c__IteratorD.<>f__this = this;
		return <processQueue>c__IteratorD;
	}

	public void RequeuePostEvents(List<object> saveArray)
	{
		for (int i = 0; i < saveArray.Count; i++)
		{
			Kochava.QueuedEvent item = (Kochava.QueuedEvent)saveArray[i];
			this.eventQueue.Enqueue(item);
		}
	}

	public void OnApplicationPause(bool didPause)
	{
		if (this.sessionTracking == Kochava.KochSessionTracking.full && this.appData != null)
		{
			Kochava._S._fireEvent("session", new Dictionary<string, object>
			{
				{
					"state",
					(!didPause) ? "resume" : "pause"
				}
			});
		}
		if (didPause)
		{
			this.saveQueue();
		}
		else
		{
			this.Log("received - app resume");
			if (PlayerPrefs.HasKey("watchlistProperties"))
			{
				AndroidJNIHelper.debug = true;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
					AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getApplicationContext", new object[0]);
					AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.kochava.android.tracker.lite.KochavaSDKLite");
					androidJavaClass2.CallStatic<string>("GetExternalKochavaInfo_Android", new object[]
					{
						androidJavaObject,
						this.whitelist,
						Kochava.device_id_delay,
						PlayerPrefs.GetString("blacklist"),
						Kochava.AdidSupressed
					});
				}
				if (this.doReportLocation)
				{
					double num = Kochava.CurrentTime();
					double num2 = double.Parse(PlayerPrefs.GetString("last_location_time"));
					if (num - num2 > (double)(this.locationStaleness * 60))
					{
					}
				}
			}
		}
	}

	public void OnApplicationQuit()
	{
		if (this.sessionTracking == Kochava.KochSessionTracking.full || this.sessionTracking == Kochava.KochSessionTracking.basic || this.sessionTracking == Kochava.KochSessionTracking.minimal)
		{
			Kochava._S._fireEvent("session", new Dictionary<string, object>
			{
				{
					"state",
					"quit"
				}
			});
		}
		this.saveQueue();
	}

	private void saveQueue()
	{
		if (this.eventQueue.Count > 0)
		{
			try
			{
				string text = JsonWriter.Serialize(this.eventQueue);
				PlayerPrefs.SetString("kochava_queue_storage", text);
				this.Log("Event Queue saved: " + text, Kochava.KochLogLevel.debug);
			}
			catch (Exception arg)
			{
				this.Log("Failure saving event queue: " + arg, Kochava.KochLogLevel.error);
			}
		}
	}

	private void loadQueue()
	{
		try
		{
			if (PlayerPrefs.HasKey("kochava_queue_storage"))
			{
				string @string = PlayerPrefs.GetString("kochava_queue_storage");
				int num = 0;
				Kochava.QueuedEvent[] array = JsonReader.Deserialize<Kochava.QueuedEvent[]>(@string);
				Kochava.QueuedEvent[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					Kochava.QueuedEvent item = array2[i];
					if (!this.eventQueue.Contains(item))
					{
						this.eventQueue.Enqueue(item);
						num++;
					}
				}
				this.Log("Loaded (" + num + ") events from persistent storage", Kochava.KochLogLevel.debug);
				PlayerPrefs.DeleteKey("kochava_queue_storage");
				base.StartCoroutine("processQueue");
			}
		}
		catch (Exception arg)
		{
			this.Log("Failure loading event queue: " + arg, Kochava.KochLogLevel.debug);
		}
	}

	public static void ClearQueue()
	{
		Kochava._S.StartCoroutine("clearQueue");
	}

	[DebuggerHidden]
	private IEnumerator clearQueue()
	{
		Kochava.<clearQueue>c__IteratorE <clearQueue>c__IteratorE = new Kochava.<clearQueue>c__IteratorE();
		<clearQueue>c__IteratorE.<>f__this = this;
		return <clearQueue>c__IteratorE;
	}

	public void GetAd(int webView, int height, int width)
	{
		this.Log("Adserver Implementation Pending");
	}

	private static string[] Chop(string value, int length)
	{
		int num = value.Length;
		int num2 = (num + length - 1) / length;
		string[] array = new string[num2];
		for (int i = 0; i < num2; i++)
		{
			array[i] = value.Substring(i * length, Mathf.Min(length, num));
			num -= length;
		}
		return array;
	}

	private void Log(string msg)
	{
		this.Log(msg, Kochava.KochLogLevel.warning);
	}

	private void Log(string msg, Kochava.KochLogLevel level)
	{
		if (msg.Length > 1000)
		{
			string[] array = Kochava.Chop(msg, 1000);
			if (level == Kochava.KochLogLevel.error)
			{
				UnityEngine.Debug.Log("*** Kochava Error: ");
			}
			else if (this.debugMode)
			{
				UnityEngine.Debug.Log("Kochava: ");
			}
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string message = array2[i];
				UnityEngine.Debug.Log(message);
			}
		}
		else if (level == Kochava.KochLogLevel.error)
		{
			UnityEngine.Debug.Log("*** Kochava Error: " + msg + " ***");
		}
		else if (this.debugMode)
		{
			UnityEngine.Debug.Log("Kochava: " + msg);
		}
		if (this.debugMode || level == Kochava.KochLogLevel.error || level == Kochava.KochLogLevel.warning)
		{
			this._EventLog.Add(new Kochava.LogEvent(msg, level));
		}
		if (this._EventLog.Count > 50)
		{
			this._EventLog.RemoveAt(0);
		}
	}

	public static void ClearLog()
	{
		Kochava._S._EventLog.Clear();
	}

	protected internal static double CurrentTime()
	{
		return (DateTime.UtcNow - Kochava.Jan1st1970).TotalSeconds;
	}

	protected internal static float UptimeDelta()
	{
		Kochava.uptimeDelta = Time.time - Kochava.uptimeDeltaUpdate;
		Kochava.uptimeDeltaUpdate = Time.time;
		return Kochava.uptimeDelta;
	}

	private string CalculateMD5Hash(string input)
	{
		string result;
		try
		{
			MD5 mD = MD5.Create();
			byte[] bytes = Encoding.ASCII.GetBytes(input);
			byte[] array = mD.ComputeHash(bytes);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x2"));
			}
			result = stringBuilder.ToString();
		}
		catch (Exception arg)
		{
			this.Log("Failure calculating MD5 hash: " + arg, Kochava.KochLogLevel.error);
			result = string.Empty;
		}
		return result;
	}

	private string CalculateSHA1Hash(string input)
	{
		string result;
		try
		{
			byte[] array = new SHA1Managed().ComputeHash(Encoding.ASCII.GetBytes(input));
			string text = string.Empty;
			byte[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				byte b = array2[i];
				text += b.ToString("x2");
			}
			result = text;
		}
		catch (Exception arg)
		{
			this.Log("Failure calculating SHA1 hash: " + arg, Kochava.KochLogLevel.error);
			result = string.Empty;
		}
		return result;
	}

	[DebuggerHidden]
	public IEnumerator KochavaAttributionTimerFired(int delayTime)
	{
		Kochava.<KochavaAttributionTimerFired>c__IteratorF <KochavaAttributionTimerFired>c__IteratorF = new Kochava.<KochavaAttributionTimerFired>c__IteratorF();
		<KochavaAttributionTimerFired>c__IteratorF.delayTime = delayTime;
		<KochavaAttributionTimerFired>c__IteratorF.<$>delayTime = delayTime;
		<KochavaAttributionTimerFired>c__IteratorF.<>f__this = this;
		return <KochavaAttributionTimerFired>c__IteratorF;
	}

	public static string GetAttributionData()
	{
		return Kochava.AttributionDataStr;
	}
}
