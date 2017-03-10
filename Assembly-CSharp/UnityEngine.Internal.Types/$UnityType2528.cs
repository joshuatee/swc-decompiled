using StaRTS.Main.Views.World.Targeting;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2528 : $UnityType
	{
		public unsafe $UnityType2528()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 987444) = ldftn($Invoke0);
			*(data + 987472) = ldftn($Invoke1);
			*(data + 987500) = ldftn($Invoke2);
			*(data + 987528) = ldftn($Invoke3);
			*(data + 987556) = ldftn($Invoke4);
			*(data + 987584) = ldftn($Invoke5);
			*(data + 987612) = ldftn($Invoke6);
			*(data + 987640) = ldftn($Invoke7);
			*(data + 987668) = ldftn($Invoke8);
			*(data + 987696) = ldftn($Invoke9);
			*(data + 987724) = ldftn($Invoke10);
			*(data + 987752) = ldftn($Invoke11);
			*(data + 987780) = ldftn($Invoke12);
			*(data + 987808) = ldftn($Invoke13);
			*(data + 987836) = ldftn($Invoke14);
			*(data + 987864) = ldftn($Invoke15);
		}

		public override object CreateInstance()
		{
			return new TargetReticle((UIntPtr)0);
		}
	}
}
