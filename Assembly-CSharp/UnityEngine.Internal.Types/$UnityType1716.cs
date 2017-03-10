using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1716 : $UnityType
	{
		public unsafe $UnityType1716()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 671156) = ldftn($Invoke0);
			*(data + 671184) = ldftn($Invoke1);
			*(data + 671212) = ldftn($Invoke2);
			*(data + 671240) = ldftn($Invoke3);
			*(data + 671268) = ldftn($Invoke4);
			*(data + 671296) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new LootComponent((UIntPtr)0);
		}
	}
}
