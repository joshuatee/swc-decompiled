using StaRTS.Main.Models.Battle;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1088 : $UnityType
	{
		public unsafe $UnityType1088()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 635484) = ldftn($Invoke0);
			*(data + 635512) = ldftn($Invoke1);
			*(data + 635540) = ldftn($Invoke2);
			*(data + 635568) = ldftn($Invoke3);
			*(data + 635596) = ldftn($Invoke4);
			*(data + 635624) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new DeploymentRecord((UIntPtr)0);
		}
	}
}
