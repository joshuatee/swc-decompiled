using StaRTS.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class HockeyAppIOS : MonoBehaviour
{
	protected const string HOCKEYAPP_CRASHESPATH = "apps/[APPID]/crashes/upload";

	protected const string HEADER_KEY = " FD 5F20D8F8-9411-45D7-ADAC-F186C5B3574C:72C6967910F6B3FD03DF0AAF9C692860409908D8AD8CCC9E";

	protected const int MAX_CHARS = 199800;

	protected const string LOG_FILE_DIR = "/logs/";

	private string serverURL = "https://api.disney.com/dmn/crash/v2";

	private string secret = string.Empty;

	private string authenticationType = "BITAuthenticatorIdentificationTypeAnonymous";

	public bool autoUpload;

	public bool exceptionLogging = true;

	public bool updateManager;

	private void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
		UnityUtils.RegisterLogCallback(base.GetInstanceID().ToString(), null);
	}

	private void OnDestroy()
	{
		UnityUtils.RegisterLogCallback(base.GetInstanceID().ToString(), null);
	}

	private void GameViewLoaded(string message)
	{
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
		HockeyAppIOS.<SendLogs>c__IteratorB <SendLogs>c__IteratorB = new HockeyAppIOS.<SendLogs>c__IteratorB();
		<SendLogs>c__IteratorB.logs = logs;
		<SendLogs>c__IteratorB.<$>logs = logs;
		<SendLogs>c__IteratorB.<>f__this = this;
		return <SendLogs>c__IteratorB;
	}

	protected virtual void WriteLogToDisk(string logString, string stackTrace)
	{
	}

	protected virtual string GetBaseURL()
	{
		string text = "https://api.disney.com/dmn/crash/v2";
		if (this.serverURL.Length > 0)
		{
			text = this.serverURL;
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

	private void Unused()
	{
		if (this.authenticationType != null || this.secret != null)
		{
			return;
		}
	}
}
