using StaRTS.Externals.Maker;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType436 : $UnityType
	{
		public unsafe $UnityType436()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 447436) = ldftn($Invoke0);
			*(data + 447464) = ldftn($Invoke1);
			*(data + 447492) = ldftn($Invoke2);
			*(data + 447520) = ldftn($Invoke3);
			*(data + 447548) = ldftn($Invoke4);
			*(data + 447576) = ldftn($Invoke5);
			*(data + 447604) = ldftn($Invoke6);
			*(data + 447632) = ldftn($Invoke7);
			*(data + 447660) = ldftn($Invoke8);
			*(data + 447688) = ldftn($Invoke9);
			*(data + 447716) = ldftn($Invoke10);
			*(data + 447744) = ldftn($Invoke11);
			*(data + 447772) = ldftn($Invoke12);
			*(data + 447800) = ldftn($Invoke13);
			*(data + 447828) = ldftn($Invoke14);
			*(data + 447856) = ldftn($Invoke15);
			*(data + 447884) = ldftn($Invoke16);
		}

		public override object CreateInstance()
		{
			return new VideoSummaryData((UIntPtr)0);
		}
	}
}
