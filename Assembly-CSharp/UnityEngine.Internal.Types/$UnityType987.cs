using StaRTS.Main.Controllers.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType987 : $UnityType
	{
		public unsafe $UnityType987()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 603816) = ldftn($Invoke0);
			*(data + 603844) = ldftn($Invoke1);
			*(data + 603872) = ldftn($Invoke2);
			*(data + 603900) = ldftn($Invoke3);
			*(data + 603928) = ldftn($Invoke4);
			*(data + 603956) = ldftn($Invoke5);
			*(data + 603984) = ldftn($Invoke6);
			*(data + 604012) = ldftn($Invoke7);
			*(data + 604040) = ldftn($Invoke8);
			*(data + 604068) = ldftn($Invoke9);
			*(data + 604096) = ldftn($Invoke10);
			*(data + 604124) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new WorldController((UIntPtr)0);
		}
	}
}
