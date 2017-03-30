using StaRTS.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public class HockeyAppAndroid : MonoBehaviour
{
	protected const string HEADER_KEY = " FD 5F20D8F8-9411-45D7-ADAC-F186C5B3574C:72C6967910F6B3FD03DF0AAF9C692860409908D8AD8CCC9E";

	private const string HOCKEYAPP_CRASHESPATH = "apps/[APPID]/crashes/upload";

	private const int MAX_CHARS = 199800;

	protected const string LOG_FILE_DIR = "/logs/";

	private string serverURL = "https://api.disney.com/dmn/crash/v2";

	public bool autoUpload;

	public bool exceptionLogging = true;

	public bool updateManager = true;

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (this.exceptionLogging && this.IsConnected())
		{
			List<string> logFiles = this.GetLogFiles();
			if (logFiles.Count > 0)
			{
				base.StartCoroutine(this.SendLogs(this.GetLogFiles()));
			}
		}
		string baseURL = this.GetBaseURL();
		this.StartCrashManager(baseURL, "b37770ed61cf8efeba453416565e3a9b", this.updateManager, this.autoUpload);
	}

	public void OnEnable()
	{
		if (this.exceptionLogging)
		{
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.OnHandleUnresolvedException);
			UnityUtils.RegisterLogCallback(base.GetInstanceID().ToString(), new UnityUtils.OnUnityLogCallback(this.OnHandleLogCallback));
		}
	}

	public void OnDisable()
	{
		UnityUtils.RegisterLogCallback(base.GetInstanceID().ToString(), null);
	}

	private void OnDestroy()
	{
		UnityUtils.RegisterLogCallback(base.GetInstanceID().ToString(), null);
	}

	protected void StartCrashManager(string urlString, string appID, bool updateManagerEnabled, bool autoSendEnabled)
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("net.hockeyapp.unity.HockeyUnityPlugin");
		androidJavaClass2.CallStatic("startHockeyAppManager", new object[]
		{
			@static,
			urlString,
			appID,
			updateManagerEnabled,
			autoSendEnabled
		});
	}

	private string GetVersion()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("net.hockeyapp.unity.HockeyUnityPlugin");
		return androidJavaClass.CallStatic<string>("getAppVersion", new object[0]);
	}

	protected virtual List<string> GetLogHeaders()
	{
		List<string> list = new List<string>();
		list.Add("Package: com.lucasarts.starts_goo");
		string version = this.GetVersion();
		list.Add("Version: " + version);
		string[] array = SystemInfo.operatingSystem.Split(new char[]
		{
			'/'
		});
		string item = "Android: " + array[0].Replace("Android OS ", string.Empty);
		list.Add(item);
		list.Add("Model: " + SystemInfo.deviceModel);
		list.Add("Date: " + DateTime.UtcNow.ToString("ddd MMM dd HH:mm:ss {}zzzz yyyy").Replace("{}", "GMT"));
		return list;
	}

	protected virtual WWWForm CreateForm(string log)
	{
		WWWForm wWWForm = new WWWForm();
		byte[] array = null;
		using (FileStream fileStream = File.OpenRead(log))
		{
			if (fileStream.Length > 199800L)
			{
				string text = null;
				using (StreamReader streamReader = new StreamReader(fileStream))
				{
					streamReader.BaseStream.Seek(fileStream.Length - 199800L, SeekOrigin.Begin);
					text = streamReader.ReadToEnd();
				}
				List<string> logHeaders = this.GetLogHeaders();
				string str = string.Empty;
				foreach (string current in logHeaders)
				{
					str = str + current + "\n";
				}
				text = str + "\n[...]" + text;
				try
				{
					array = Encoding.Default.GetBytes(text);
				}
				catch (ArgumentException)
				{
				}
			}
			else
			{
				try
				{
					array = File.ReadAllBytes(log);
				}
				catch (SystemException)
				{
				}
			}
		}
		if (array != null)
		{
			wWWForm.AddBinaryData("log", array, log, "text/plain");
		}
		return wWWForm;
	}

	protected virtual List<string> GetLogFiles()
	{
		List<string> list = new List<string>();
		string path = Application.persistentDataPath + "/logs/";
		try
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			FileInfo[] files = directoryInfo.GetFiles();
			if (files.Length > 0)
			{
				FileInfo[] array = files;
				for (int i = 0; i < array.Length; i++)
				{
					FileInfo fileInfo = array[i];
					if (fileInfo.Extension == ".log")
					{
						list.Add(fileInfo.FullName);
					}
					else
					{
						File.Delete(fileInfo.FullName);
					}
				}
			}
		}
		catch (Exception)
		{
		}
		return list;
	}

	[DebuggerHidden]
	protected virtual IEnumerator SendLogs(List<string> logs)
	{
		HockeyAppAndroid.<SendLogs>c__IteratorA <SendLogs>c__IteratorA = new HockeyAppAndroid.<SendLogs>c__IteratorA();
		<SendLogs>c__IteratorA.logs = logs;
		<SendLogs>c__IteratorA.<$>logs = logs;
		<SendLogs>c__IteratorA.<>f__this = this;
		return <SendLogs>c__IteratorA;
	}

	protected virtual void WriteLogToDisk(string logString, string stackTrace)
	{
		string str = DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss_fff");
		string text = logString.Replace("\n", " ");
		string[] array = stackTrace.Split(new char[]
		{
			'\n'
		});
		text = "\n" + text + "\n";
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string text2 = array2[i];
			if (text2.Length > 0)
			{
				text = text + "  at " + text2 + "\n";
			}
		}
		List<string> logHeaders = this.GetLogHeaders();
		using (StreamWriter streamWriter = new StreamWriter(Application.persistentDataPath + "/logs/LogFile_" + str + ".log", true))
		{
			foreach (string current in logHeaders)
			{
				streamWriter.WriteLine(current);
			}
			streamWriter.WriteLine(text);
		}
	}

	protected virtual string GetBaseURL()
	{
		string text = "https://api.disney.com/dmn/crash/v2";
		string text2 = this.serverURL.Trim();
		if (text2.Length > 0)
		{
			text = text2;
			if (!text[text.Length - 1].Equals("/"))
			{
				text += "/";
			}
		}
		return text;
	}

	protected virtual bool IsConnected()
	{
		bool result = false;
		if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
		{
			result = true;
		}
		return result;
	}

	protected virtual void HandleException(string logString, string stackTrace)
	{
		this.WriteLogToDisk(logString, stackTrace);
	}

	public void OnHandleLogCallback(string logString, string stackTrace, LogType type)
	{
		if (type != LogType.Assert && type != LogType.Exception)
		{
			return;
		}
		this.HandleException(logString, stackTrace);
	}

	public void OnHandleUnresolvedException(object sender, UnhandledExceptionEventArgs args)
	{
		if (args == null || args.ExceptionObject == null)
		{
			return;
		}
		if (args.ExceptionObject.GetType() != typeof(Exception))
		{
			return;
		}
		Exception ex = (Exception)args.ExceptionObject;
		this.HandleException(ex.Source, ex.StackTrace);
	}
}
