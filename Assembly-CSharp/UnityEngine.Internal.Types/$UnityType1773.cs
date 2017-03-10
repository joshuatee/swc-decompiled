using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1773 : $UnityType
	{
		public unsafe $UnityType1773()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 680564) = ldftn($Invoke0);
			*(data + 680592) = ldftn($Invoke1);
			*(data + 680620) = ldftn($Invoke2);
			*(data + 680648) = ldftn($Invoke3);
			*(data + 680676) = ldftn($Invoke4);
			*(data + 680704) = ldftn($Invoke5);
			*(data + 680732) = ldftn($Invoke6);
			*(data + 680760) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new LootNode((UIntPtr)0);
		}
	}
}
