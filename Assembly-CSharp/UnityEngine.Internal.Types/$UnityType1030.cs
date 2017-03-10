using StaRTS.Main.Models;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1030 : $UnityType
	{
		public unsafe $UnityType1030()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 608828) = ldftn($Invoke0);
			*(data + 608856) = ldftn($Invoke1);
			*(data + 608884) = ldftn($Invoke2);
			*(data + 608912) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new DeployableInfoActionButtonTag((UIntPtr)0);
		}
	}
}
