using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;

namespace StaRTS.Main.Models
{
	public class AudienceCondition
	{
		private const char DELIMITER = ':';

		private const string INVALID = "Invalid";

		public string ConditionType;

		public string ConditionValue;

		public AudienceCondition(string source)
		{
			this.ConditionType = "Invalid";
			this.ConditionValue = "Invalid";
			base..ctor();
			string[] array = source.Split(new char[]
			{
				':'
			});
			if (array.Length < 2)
			{
				Service.Get<StaRTSLogger>().WarnFormat("Could not define AudienceCondition from {0}", new object[]
				{
					source
				});
				return;
			}
			this.ConditionType = array[0];
			this.ConditionValue = array[1];
		}

		protected internal AudienceCondition(UIntPtr dummy)
		{
		}
	}
}
