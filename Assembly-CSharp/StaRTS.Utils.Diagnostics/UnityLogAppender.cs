using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Utils.Diagnostics
{
	public class UnityLogAppender : BaseLogAppender
	{
		private const int MAX_LENGTH = 10000;

		public bool CurrentlyLogging
		{
			get;
			set;
		}

		protected override void Trace(string formattedMessage)
		{
			this.CurrentlyLogging = true;
			if (formattedMessage.get_Length() > 10000)
			{
				formattedMessage = formattedMessage.Substring(0, 10000);
			}
			LogLevel level = this.entry.Level;
			if (level != LogLevel.Error)
			{
				if (level != LogLevel.Warn)
				{
					Debug.Log(formattedMessage);
				}
				else
				{
					Debug.LogWarning(formattedMessage);
				}
			}
			else
			{
				Debug.LogError(formattedMessage);
			}
			this.CurrentlyLogging = false;
		}

		public UnityLogAppender()
		{
		}

		protected internal UnityLogAppender(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((UnityLogAppender)GCHandledObjects.GCHandleToObject(instance)).CurrentlyLogging);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((UnityLogAppender)GCHandledObjects.GCHandleToObject(instance)).CurrentlyLogging = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((UnityLogAppender)GCHandledObjects.GCHandleToObject(instance)).Trace(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}
	}
}
