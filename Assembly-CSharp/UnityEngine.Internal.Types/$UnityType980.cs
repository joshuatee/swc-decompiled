using StaRTS.Main.Controllers.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType980 : $UnityType
	{
		public unsafe $UnityType980()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 602500) = ldftn($Invoke0);
			*(data + 602528) = ldftn($Invoke1);
			*(data + 602556) = ldftn($Invoke2);
			*(data + 602584) = ldftn($Invoke3);
			*(data + 602612) = ldftn($Invoke4);
			*(data + 602640) = ldftn($Invoke5);
			*(data + 602668) = ldftn($Invoke6);
			*(data + 602696) = ldftn($Invoke7);
			*(data + 602724) = ldftn($Invoke8);
			*(data + 602752) = ldftn($Invoke9);
			*(data + 602780) = ldftn($Invoke10);
			*(data + 602808) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new NpcMapDataLoader((UIntPtr)0);
		}
	}
}
