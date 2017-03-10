using StaRTS.Main.Controllers.Entities.Systems;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType821 : $UnityType
	{
		public unsafe $UnityType821()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 572624) = ldftn($Invoke0);
			*(data + 572652) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new TrackingSystem((UIntPtr)0);
		}
	}
}
