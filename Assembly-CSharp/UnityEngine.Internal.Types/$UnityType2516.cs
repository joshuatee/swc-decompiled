using StaRTS.Main.Views.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2516 : $UnityType
	{
		public unsafe $UnityType2516()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 984896) = ldftn($Invoke0);
			*(data + 984924) = ldftn($Invoke1);
			*(data + 984952) = ldftn($Invoke2);
			*(data + 984980) = ldftn($Invoke3);
			*(data + 985008) = ldftn($Invoke4);
			*(data + 985036) = ldftn($Invoke5);
			*(data + 985064) = ldftn($Invoke6);
			*(data + 985092) = ldftn($Invoke7);
			*(data + 985120) = ldftn($Invoke8);
			*(data + 985148) = ldftn($Invoke9);
			*(data + 985176) = ldftn($Invoke10);
			*(data + 985204) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new SpawnProtectionView((UIntPtr)0);
		}
	}
}
