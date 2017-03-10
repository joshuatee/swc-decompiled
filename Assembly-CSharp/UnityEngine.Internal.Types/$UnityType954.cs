using StaRTS.Main.Controllers.VictoryConditions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType954 : $UnityType
	{
		public unsafe $UnityType954()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 598748) = ldftn($Invoke0);
			*(data + 598776) = ldftn($Invoke1);
			*(data + 598804) = ldftn($Invoke2);
			*(data + 598832) = ldftn($Invoke3);
			*(data + 598860) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new CountEventsCondition((UIntPtr)0);
		}
	}
}
