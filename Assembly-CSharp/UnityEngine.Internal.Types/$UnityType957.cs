using StaRTS.Main.Controllers.VictoryConditions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType957 : $UnityType
	{
		public unsafe $UnityType957()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 599056) = ldftn($Invoke0);
			*(data + 599084) = ldftn($Invoke1);
			*(data + 599112) = ldftn($Invoke2);
			*(data + 599140) = ldftn($Invoke3);
			*(data + 599168) = ldftn($Invoke4);
			*(data + 599196) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new DeployUnitTypeCondition((UIntPtr)0);
		}
	}
}
