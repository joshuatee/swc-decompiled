using StaRTS.Main.Models;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1063 : $UnityType
	{
		public unsafe $UnityType1063()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 626944) = ldftn($Invoke0);
			*(data + 626972) = ldftn($Invoke1);
			*(data + 627000) = ldftn($Invoke2);
			*(data + 627028) = ldftn($Invoke3);
			*(data + 627056) = ldftn($Invoke4);
			*(data + 627084) = ldftn($Invoke5);
			*(data + 627112) = ldftn($Invoke6);
			*(data + 627140) = ldftn($Invoke7);
			*(data + 627168) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new SortableEquipment((UIntPtr)0);
		}
	}
}
