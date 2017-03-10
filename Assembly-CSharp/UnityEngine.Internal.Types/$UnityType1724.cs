using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1724 : $UnityType
	{
		public unsafe $UnityType1724()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 671940) = ldftn($Invoke0);
			*(data + 671968) = ldftn($Invoke1);
			*(data + 671996) = ldftn($Invoke2);
			*(data + 672024) = ldftn($Invoke3);
			*(data + 672052) = ldftn($Invoke4);
			*(data + 672080) = ldftn($Invoke5);
			*(data + 672108) = ldftn($Invoke6);
			*(data + 672136) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new ShieldGeneratorComponent((UIntPtr)0);
		}
	}
}
