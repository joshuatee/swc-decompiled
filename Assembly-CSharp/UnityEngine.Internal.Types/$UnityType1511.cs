using StaRTS.Main.Models.Commands.Player.Deployable.Upgrade.Start;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1511 : $UnityType
	{
		public unsafe $UnityType1511()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 657884) = ldftn($Invoke0);
			*(data + 657912) = ldftn($Invoke1);
			*(data + 657940) = ldftn($Invoke2);
			*(data + 657968) = ldftn($Invoke3);
			*(data + 657996) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new DeployableUpgradeStartRequest((UIntPtr)0);
		}
	}
}
