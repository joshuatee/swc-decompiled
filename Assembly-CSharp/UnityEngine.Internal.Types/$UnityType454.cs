using StaRTS.Externals.Maker.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType454 : $UnityType
	{
		public unsafe $UnityType454()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 452560) = ldftn($Invoke0);
			*(data + 452588) = ldftn($Invoke1);
			*(data + 452616) = ldftn($Invoke2);
			*(data + 452644) = ldftn($Invoke3);
			*(data + 452672) = ldftn($Invoke4);
			*(data + 452700) = ldftn($Invoke5);
			*(data + 452728) = ldftn($Invoke6);
			*(data + 452756) = ldftn($Invoke7);
			*(data + 452784) = ldftn($Invoke8);
			*(data + 452812) = ldftn($Invoke9);
			*(data + 452840) = ldftn($Invoke10);
			*(data + 452868) = ldftn($Invoke11);
			*(data + 452896) = ldftn($Invoke12);
			*(data + 452924) = ldftn($Invoke13);
			*(data + 452952) = ldftn($Invoke14);
			*(data + 452980) = ldftn($Invoke15);
			*(data + 453008) = ldftn($Invoke16);
			*(data + 453036) = ldftn($Invoke17);
			*(data + 453064) = ldftn($Invoke18);
			*(data + 453092) = ldftn($Invoke19);
			*(data + 453120) = ldftn($Invoke20);
		}

		public override object CreateInstance()
		{
			return new VideoPlayerKeepAlive((UIntPtr)0);
		}
	}
}
