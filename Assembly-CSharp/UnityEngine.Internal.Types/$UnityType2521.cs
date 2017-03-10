using StaRTS.Main.Views.World.Deploying;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2521 : $UnityType
	{
		public unsafe $UnityType2521()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 986072) = ldftn($Invoke0);
			*(data + 986100) = ldftn($Invoke1);
			*(data + 986128) = ldftn($Invoke2);
			*(data + 986156) = ldftn($Invoke3);
			*(data + 986184) = ldftn($Invoke4);
			*(data + 986212) = ldftn($Invoke5);
			*(data + 986240) = ldftn($Invoke6);
			*(data + 986268) = ldftn($Invoke7);
			*(data + 986296) = ldftn($Invoke8);
			*(data + 986324) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new HeroDeployer((UIntPtr)0);
		}
	}
}
