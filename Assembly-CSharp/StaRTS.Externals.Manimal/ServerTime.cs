using StaRTS.Utils.Core;
using System;

namespace StaRTS.Externals.Manimal
{
	public class ServerTime
	{
		public static uint Time
		{
			get
			{
				return Service.Get<ServerAPI>().ServerTime;
			}
		}

		public ServerTime()
		{
		}

		protected internal ServerTime(UIntPtr dummy)
		{
		}
	}
}
