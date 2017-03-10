using StaRTS.Main.Models.Commands.Player.Deployable;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1506 : $UnityType
	{
		public unsafe $UnityType1506()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 657492) = ldftn($Invoke0);
			*(data + 657520) = ldftn($Invoke1);
			*(data + 657548) = ldftn($Invoke2);
			*(data + 657576) = ldftn($Invoke3);
			*(data + 657604) = ldftn($Invoke4);
			*(data + 657632) = ldftn($Invoke5);
			*(data + 657660) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new DeployableContractRequest((UIntPtr)0);
		}
	}
}
