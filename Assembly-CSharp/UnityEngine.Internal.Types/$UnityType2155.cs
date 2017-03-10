using StaRTS.Main.Views.Animations;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2155 : $UnityType
	{
		public unsafe $UnityType2155()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 862648) = ldftn($Invoke0);
			*(data + 862676) = ldftn($Invoke1);
			*(data + 862704) = ldftn($Invoke2);
			*(data + 862732) = ldftn($Invoke3);
			*(data + 862760) = ldftn($Invoke4);
			*(data + 862788) = ldftn($Invoke5);
			*(data + 862816) = ldftn($Invoke6);
			*(data + 862844) = ldftn($Invoke7);
			*(data + 862872) = ldftn($Invoke8);
			*(data + 862900) = ldftn($Invoke9);
			*(data + 862928) = ldftn($Invoke10);
			*(data + 862956) = ldftn($Invoke11);
			*(data + 862984) = ldftn($Invoke12);
		}

		public override object CreateInstance()
		{
			return new TransitionVisuals((UIntPtr)0);
		}
	}
}
