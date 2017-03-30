using StaRTS.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class HockeyAppAmazon : MonoBehaviour
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
	}

	public void OnEnable()
	{
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
	}

	private string GetVersion()
	{
		return null;
	}

	protected virtual List<string> GetLogHeaders()
	{
		return new List<string>();
	}

	protected virtual WWWForm CreateForm(string log)
	{
		return new WWWForm();
	}

	protected virtual List<string> GetLogFiles()
	{
		return new List<string>();
	}

	[DebuggerHidden]
	protected virtual IEnumerator SendLogs(List<string> logs)
	{
		HockeyAppAmazon.<SendLogs>c__Iterator9 <SendLogs>c__Iterator = new HockeyAppAmazon.<SendLogs>c__Iterator9();
		<SendLogs>c__Iterator.logs = logs;
		<SendLogs>c__Iterator.<$>logs = logs;
		<SendLogs>c__Iterator.<>f__this = this;
		return <SendLogs>c__Iterator;
	}

	protected virtual void WriteLogToDisk(string logString, string stackTrace)
	{
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
		return false;
	}

	protected virtual void HandleException(string logString, string stackTrace)
	{
	}

	public void OnHandleLogCallback(string logString, string stackTrace, LogType type)
	{
	}

	public void OnHandleUnresolvedException(object sender, UnhandledExceptionEventArgs args)
	{
	}
}
