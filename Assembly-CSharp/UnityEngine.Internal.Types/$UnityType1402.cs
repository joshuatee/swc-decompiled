using StaRTS.Main.Models.Commands.Holonet;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1402 : $UnityType
	{
		public unsafe $UnityType1402()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 652368) = ldftn($Invoke0);
			*(data + 652396) = ldftn($Invoke1);
			*(data + 652424) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new HolonetGetCommandCenterEntriesResponse((UIntPtr)0);
		}
	}
}
