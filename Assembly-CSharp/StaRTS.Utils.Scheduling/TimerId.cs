using System;

namespace StaRTS.Utils.Scheduling
{
	public class TimerId
	{
		public const uint INVALID = 0u;

		public static uint GetNext(ref uint idLast)
		{
			uint num = idLast + 1u;
			idLast = num;
			if (num == 0u)
			{
				throw new Exception("Timer id rollover has occurred");
			}
			return idLast;
		}

		public TimerId()
		{
		}

		protected internal TimerId(UIntPtr dummy)
		{
		}
	}
}
