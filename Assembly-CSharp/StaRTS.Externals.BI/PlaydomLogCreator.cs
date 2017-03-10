using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using WinRTBridge;

namespace StaRTS.Externals.BI
{
	public class PlaydomLogCreator : ILogCreator
	{
		private string primaryURL;

		private string secondaryNoProxyURL;

		public PlaydomLogCreator(string primaryURL, string secondaryNoProxyURL)
		{
			this.primaryURL = primaryURL;
			this.secondaryNoProxyURL = secondaryNoProxyURL;
		}

		public void SetURL(string primaryURL, string secondaryNoProxyURL)
		{
			this.primaryURL = primaryURL;
			this.secondaryNoProxyURL = secondaryNoProxyURL;
		}

		public BILogData CreateWWWDataFromBILog(BILog log)
		{
			string url = this.ToURL(log);
			return new BILogData
			{
				url = url
			};
		}

		public string ToURL(BILog log)
		{
			StringBuilder stringBuilder = new StringBuilder(log.UseSecondaryUrl ? this.secondaryNoProxyURL : this.primaryURL);
			Dictionary<string, string> paramDict = log.GetParamDict();
			foreach (string current in paramDict.Keys)
			{
				stringBuilder.Append("&");
				stringBuilder.Append(current);
				stringBuilder.Append("=");
				stringBuilder.Append(paramDict[current]);
			}
			return stringBuilder.ToString();
		}

		protected internal PlaydomLogCreator(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlaydomLogCreator)GCHandledObjects.GCHandleToObject(instance)).CreateWWWDataFromBILog((BILog)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PlaydomLogCreator)GCHandledObjects.GCHandleToObject(instance)).SetURL(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlaydomLogCreator)GCHandledObjects.GCHandleToObject(instance)).ToURL((BILog)GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
