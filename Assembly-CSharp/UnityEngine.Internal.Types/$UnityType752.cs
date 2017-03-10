using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType752 : $UnityType
	{
		public unsafe $UnityType752()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 548292) = ldftn($Invoke0);
			*(data + 548320) = ldftn($Invoke1);
			*(data + 548348) = ldftn($Invoke2);
			*(data + 548376) = ldftn($Invoke3);
			*(data + 548404) = ldftn($Invoke4);
			*(data + 548432) = ldftn($Invoke5);
			*(data + 548460) = ldftn($Invoke6);
			*(data + 548488) = ldftn($Invoke7);
			*(data + 548516) = ldftn($Invoke8);
			*(data + 548544) = ldftn($Invoke9);
			*(data + 548572) = ldftn($Invoke10);
			*(data + 548600) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new RewardManager((UIntPtr)0);
		}
	}
}
