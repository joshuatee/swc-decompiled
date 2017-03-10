using StaRTS.Main.Controllers.VictoryConditions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType956 : $UnityType
	{
		public unsafe $UnityType956()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 598888) = ldftn($Invoke0);
			*(data + 598916) = ldftn($Invoke1);
			*(data + 598944) = ldftn($Invoke2);
			*(data + 598972) = ldftn($Invoke3);
			*(data + 599000) = ldftn($Invoke4);
			*(data + 599028) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new DeployUnitIdCondition((UIntPtr)0);
		}
	}
}
