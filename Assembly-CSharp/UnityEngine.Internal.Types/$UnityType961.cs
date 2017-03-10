using StaRTS.Main.Controllers.VictoryConditions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType961 : $UnityType
	{
		public unsafe $UnityType961()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 599728) = ldftn($Invoke0);
			*(data + 599756) = ldftn($Invoke1);
			*(data + 599784) = ldftn($Invoke2);
			*(data + 599812) = ldftn($Invoke3);
			*(data + 599840) = ldftn($Invoke4);
			*(data + 599868) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new DestroyBuildingUidCondition((UIntPtr)0);
		}
	}
}
