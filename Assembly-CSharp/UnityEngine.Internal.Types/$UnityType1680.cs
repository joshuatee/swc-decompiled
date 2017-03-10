using StaRTS.Main.Models.Commands.TransferObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1680 : $UnityType
	{
		public unsafe $UnityType1680()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 666200) = ldftn($Invoke0);
			*(data + 666228) = ldftn($Invoke1);
			*(data + 666256) = ldftn($Invoke2);
			*(data + 666284) = ldftn($Invoke3);
			*(data + 666312) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new PositionMap((UIntPtr)0);
		}
	}
}
