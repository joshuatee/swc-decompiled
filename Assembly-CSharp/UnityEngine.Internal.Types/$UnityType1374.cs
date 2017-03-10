using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1374 : $UnityType
	{
		public unsafe $UnityType1374()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 651640) = ldftn($Invoke0);
			*(data + 651668) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new PromoCodeTestRequest((UIntPtr)0);
		}
	}
}
