using StaRTS.Main.Models.Commands;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1109 : $UnityType
	{
		public unsafe $UnityType1109()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 640972) = ldftn($Invoke0);
			*(data + 641000) = ldftn($Invoke1);
			*(data + 641028) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new AddPatchRequest((UIntPtr)0);
		}
	}
}
