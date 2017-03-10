using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1729 : $UnityType
	{
		public unsafe $UnityType1729()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 673340) = ldftn($Invoke0);
			*(data + 673368) = ldftn($Invoke1);
			*(data + 673396) = ldftn($Invoke2);
			*(data + 673424) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new StorageComponent((UIntPtr)0);
		}
	}
}
