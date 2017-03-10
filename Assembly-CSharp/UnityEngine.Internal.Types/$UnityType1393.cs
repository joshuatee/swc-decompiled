using StaRTS.Main.Models.Commands.Crates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1393 : $UnityType
	{
		public unsafe $UnityType1393()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 652032) = ldftn($Invoke0);
			*(data + 652060) = ldftn($Invoke1);
			*(data + 652088) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new OpenCrateResponse((UIntPtr)0);
		}
	}
}
