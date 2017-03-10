using StaRTS.Main.Models.Player.Objectives;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1831 : $UnityType
	{
		public unsafe $UnityType1831()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 700388) = ldftn($Invoke0);
			*(data + 700416) = ldftn($Invoke1);
			*(data + 700444) = ldftn($Invoke2);
			*(data + 700472) = ldftn($Invoke3);
			*(data + 700500) = ldftn($Invoke4);
			*(data + 700528) = ldftn($Invoke5);
			*(data + 700556) = ldftn($Invoke6);
			*(data + 700584) = ldftn($Invoke7);
			*(data + 700612) = ldftn($Invoke8);
			*(data + 700640) = ldftn($Invoke9);
			*(data + 700668) = ldftn($Invoke10);
			*(data + 700696) = ldftn($Invoke11);
			*(data + 700724) = ldftn($Invoke12);
			*(data + 700752) = ldftn($Invoke13);
		}

		public override object CreateInstance()
		{
			return new ObjectiveGroup((UIntPtr)0);
		}
	}
}
