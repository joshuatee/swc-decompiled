using StaRTS.Main.Views.World.Deploying;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2522 : $UnityType
	{
		public unsafe $UnityType2522()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 986352) = ldftn($Invoke0);
			*(data + 986380) = ldftn($Invoke1);
			*(data + 986408) = ldftn($Invoke2);
			*(data + 986436) = ldftn($Invoke3);
			*(data + 986464) = ldftn($Invoke4);
			*(data + 986492) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new SpecialAttackDeployer((UIntPtr)0);
		}
	}
}
