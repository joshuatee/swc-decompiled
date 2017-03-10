using StaRTS.Main.Models.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1812 : $UnityType
	{
		public unsafe $UnityType1812()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 688488) = ldftn($Invoke0);
			*(data + 688516) = ldftn($Invoke1);
			*(data + 688544) = ldftn($Invoke2);
			*(data + 688572) = ldftn($Invoke3);
			*(data + 688600) = ldftn($Invoke4);
			*(data + 688628) = ldftn($Invoke5);
			*(data + 688656) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new ActiveArmory((UIntPtr)0);
		}
	}
}
