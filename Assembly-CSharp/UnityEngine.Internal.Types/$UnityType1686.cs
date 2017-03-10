using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1686 : $UnityType
	{
		public unsafe $UnityType1686()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 666592) = ldftn($Invoke0);
			*(data + 666620) = ldftn($Invoke1);
			*(data + 666648) = ldftn($Invoke2);
			*(data + 666676) = ldftn($Invoke3);
			*(data + 666704) = ldftn($Invoke4);
			*(data + 666732) = ldftn($Invoke5);
			*(data + 666760) = ldftn($Invoke6);
			*(data + 666788) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new AssetComponent((UIntPtr)0);
		}
	}
}
