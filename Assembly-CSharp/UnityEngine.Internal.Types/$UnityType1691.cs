using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1691 : $UnityType
	{
		public unsafe $UnityType1691()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 667488) = ldftn($Invoke0);
			*(data + 667516) = ldftn($Invoke1);
			*(data + 667544) = ldftn($Invoke2);
			*(data + 667572) = ldftn($Invoke3);
			*(data + 667600) = ldftn($Invoke4);
			*(data + 667628) = ldftn($Invoke5);
			*(data + 667656) = ldftn($Invoke6);
			*(data + 667684) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new BuildingAnimationComponent((UIntPtr)0);
		}
	}
}
