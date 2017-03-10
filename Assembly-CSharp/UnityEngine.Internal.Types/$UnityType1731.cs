using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1731 : $UnityType
	{
		public unsafe $UnityType1731()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 673452) = ldftn($Invoke0);
			*(data + 673480) = ldftn($Invoke1);
			*(data + 673508) = ldftn($Invoke2);
			*(data + 673536) = ldftn($Invoke3);
			*(data + 673564) = ldftn($Invoke4);
			*(data + 673592) = ldftn($Invoke5);
			*(data + 673620) = ldftn($Invoke6);
			*(data + 673648) = ldftn($Invoke7);
			*(data + 673676) = ldftn($Invoke8);
			*(data + 673704) = ldftn($Invoke9);
			*(data + 673732) = ldftn($Invoke10);
			*(data + 673760) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new SupportViewComponent((UIntPtr)0);
		}
	}
}
