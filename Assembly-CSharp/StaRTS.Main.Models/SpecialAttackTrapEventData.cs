using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;

namespace StaRTS.Main.Models
{
	public class SpecialAttackTrapEventData : ITrapEventData
	{
		public string SpecialAttackName
		{
			get;
			private set;
		}

		public ITrapEventData Init(string rawData)
		{
			if (string.IsNullOrEmpty(rawData))
			{
				Service.Get<Logger>().Error("All SpecialAttack Traps must list the uid of the special attack");
				return null;
			}
			this.SpecialAttackName = rawData.TrimEnd(new char[]
			{
				' '
			});
			return this;
		}
	}
}
