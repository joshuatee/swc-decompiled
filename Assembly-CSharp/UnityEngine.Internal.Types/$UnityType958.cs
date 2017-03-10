using StaRTS.Main.Controllers.VictoryConditions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType958 : $UnityType
	{
		public unsafe $UnityType958()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 599224) = ldftn($Invoke0);
			*(data + 599252) = ldftn($Invoke1);
			*(data + 599280) = ldftn($Invoke2);
			*(data + 599308) = ldftn($Invoke3);
			*(data + 599336) = ldftn($Invoke4);
			*(data + 599364) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new DeployUnitUidCondition((UIntPtr)0);
		}
	}
}
