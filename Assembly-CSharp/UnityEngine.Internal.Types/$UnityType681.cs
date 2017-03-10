using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType681 : $UnityType
	{
		public unsafe $UnityType681()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 518584) = ldftn($Invoke0);
			*(data + 518612) = ldftn($Invoke1);
			*(data + 518640) = ldftn($Invoke2);
			*(data + 518668) = ldftn($Invoke3);
			*(data + 518696) = ldftn($Invoke4);
			*(data + 518724) = ldftn($Invoke5);
			*(data + 518752) = ldftn($Invoke6);
			*(data + 518780) = ldftn($Invoke7);
			*(data + 518808) = ldftn($Invoke8);
			*(data + 518836) = ldftn($Invoke9);
			*(data + 518864) = ldftn($Invoke10);
			*(data + 518892) = ldftn($Invoke11);
			*(data + 518920) = ldftn($Invoke12);
			*(data + 518948) = ldftn($Invoke13);
			*(data + 518976) = ldftn($Invoke14);
			*(data + 519004) = ldftn($Invoke15);
			*(data + 519032) = ldftn($Invoke16);
			*(data + 519060) = ldftn($Invoke17);
		}

		public override object CreateInstance()
		{
			return new ArmoryController((UIntPtr)0);
		}
	}
}
