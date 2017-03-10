using StaRTS.Audio;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType365 : $UnityType
	{
		public unsafe $UnityType365()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 431868) = ldftn($Invoke0);
			*(data + 431896) = ldftn($Invoke1);
			*(data + 431924) = ldftn($Invoke2);
			*(data + 431952) = ldftn($Invoke3);
			*(data + 431980) = ldftn($Invoke4);
			*(data + 432008) = ldftn($Invoke5);
			*(data + 432036) = ldftn($Invoke6);
			*(data + 432064) = ldftn($Invoke7);
			*(data + 432092) = ldftn($Invoke8);
			*(data + 432120) = ldftn($Invoke9);
			*(data + 432148) = ldftn($Invoke10);
			*(data + 432176) = ldftn($Invoke11);
			*(data + 432204) = ldftn($Invoke12);
			*(data + 432232) = ldftn($Invoke13);
			*(data + 432260) = ldftn($Invoke14);
			*(data + 432288) = ldftn($Invoke15);
			*(data + 432316) = ldftn($Invoke16);
			*(data + 432344) = ldftn($Invoke17);
			*(data + 432372) = ldftn($Invoke18);
			*(data + 432400) = ldftn($Invoke19);
			*(data + 432428) = ldftn($Invoke20);
			*(data + 432456) = ldftn($Invoke21);
			*(data + 432484) = ldftn($Invoke22);
		}

		public override object CreateInstance()
		{
			return new AudioEventManager((UIntPtr)0);
		}
	}
}
