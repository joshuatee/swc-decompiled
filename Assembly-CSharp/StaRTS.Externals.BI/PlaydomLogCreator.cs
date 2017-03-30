using System;
using System.Collections.Generic;
using System.Text;

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
			StringBuilder stringBuilder = new StringBuilder((!log.UseSecondaryUrl) ? this.primaryURL : this.secondaryNoProxyURL);
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
	}
}
