using StaRTS.Main.Models.Commands.Squads.Responses;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1647 : $UnityType
	{
		public unsafe $UnityType1647()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 662924) = ldftn($Invoke0);
			*(data + 662952) = ldftn($Invoke1);
			*(data + 662980) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new FeaturedSquadsResponse((UIntPtr)0);
		}
	}
}
