using StaRTS.Main.Views.UX.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2471 : $UnityType
	{
		public unsafe $UnityType2471()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 970056) = ldftn($Invoke0);
			*(data + 970084) = ldftn($Invoke1);
			*(data + 970112) = ldftn($Invoke2);
			*(data + 970140) = ldftn($Invoke3);
			*(data + 970168) = ldftn($Invoke4);
			*(data + 970196) = ldftn($Invoke5);
			*(data + 970224) = ldftn($Invoke6);
			*(data + 970252) = ldftn($Invoke7);
			*(data + 970280) = ldftn($Invoke8);
			*(data + 970308) = ldftn($Invoke9);
			*(data + 970336) = ldftn($Invoke10);
			*(data + 970364) = ldftn($Invoke11);
			*(data + 970392) = ldftn($Invoke12);
			*(data + 970420) = ldftn($Invoke13);
		}

		public override object CreateInstance()
		{
			return new SquadWarFlyout((UIntPtr)0);
		}
	}
}
