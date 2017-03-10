using StaRTS.Main.Controllers.Entities.Systems;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType812 : $UnityType
	{
		public unsafe $UnityType812()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 571784) = ldftn($Invoke0);
			*(data + 571812) = ldftn($Invoke1);
			*(data + 571840) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new DroidSystem((UIntPtr)0);
		}
	}
}
