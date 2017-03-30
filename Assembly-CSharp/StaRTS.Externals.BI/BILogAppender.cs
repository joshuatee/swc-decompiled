using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using UnityEngine;

namespace StaRTS.Externals.BI
{
	public class BILogAppender : ILogAppender
	{
		private UnityLogAppender unityLogAppender;

		public BILogAppender(UnityLogAppender unityLogAppender)
		{
			this.unityLogAppender = unityLogAppender;
			UnityUtils.RegisterLogCallback(typeof(BILogAppender).Name, new UnityUtils.OnUnityLogCallback(this.HandleUnityLog));
		}

		public void AddLogMessage(LogEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			this.LogBIMessage(entry.Level, entry.Message);
		}

		private void LogBIMessage(LogLevel logLevel, string message)
		{
			if (logLevel == LogLevel.Debug)
			{
				return;
			}
			if (Service.IsSet<BILoggingController>())
			{
				Service.Get<BILoggingController>().TrackError(logLevel, message);
			}
		}

		private void HandleUnityLog(string logString, string stackTrace, LogType type)
		{
			if (type == LogType.Exception)
			{
				this.LogBIMessage(LogLevel.Error, logString);
			}
			else if (this.unityLogAppender == null || !this.unityLogAppender.CurrentlyLogging)
			{
				switch (type)
				{
				case LogType.Error:
					this.LogBIMessage(LogLevel.Error, logString);
					break;
				case LogType.Warning:
					this.LogBIMessage(LogLevel.Warn, logString);
					break;
				}
			}
		}
	}
}
