using StaRTS.Main.Views.World.Deploying;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2524 : $UnityType
	{
		public unsafe $UnityType2524()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 986632) = ldftn($Invoke0);
			*(data + 986660) = ldftn($Invoke1);
			*(data + 986688) = ldftn($Invoke2);
			*(data + 986716) = ldftn($Invoke3);
			*(data + 986744) = ldftn($Invoke4);
			*(data + 986772) = ldftn($Invoke5);
			*(data + 986800) = ldftn($Invoke6);
			*(data + 986828) = ldftn($Invoke7);
			*(data + 986856) = ldftn($Invoke8);
			*(data + 986884) = ldftn($Invoke9);
			*(data + 986912) = ldftn($Invoke10);
			*(data + 986940) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new TroopDeployer((UIntPtr)0);
		}
	}
}
