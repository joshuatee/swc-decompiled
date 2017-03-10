using StaRTS.Main.Models.Commands.TransferObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1679 : $UnityType
	{
		public unsafe $UnityType1679()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 666032) = ldftn($Invoke0);
			*(data + 666060) = ldftn($Invoke1);
			*(data + 666088) = ldftn($Invoke2);
			*(data + 666116) = ldftn($Invoke3);
			*(data + 666144) = ldftn($Invoke4);
			*(data + 666172) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new Position((UIntPtr)0);
		}
	}
}
