using StaRTS.Main.Controllers.Missions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType855 : $UnityType
	{
		public unsafe $UnityType855()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 578532) = ldftn($Invoke0);
			*(data + 578560) = ldftn($Invoke1);
			*(data + 578588) = ldftn($Invoke2);
			*(data + 578616) = ldftn($Invoke3);
			*(data + 578644) = ldftn($Invoke4);
			*(data + 578672) = ldftn($Invoke5);
			*(data + 578700) = ldftn($Invoke6);
			*(data + 578728) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new OwnMissionProcessor((UIntPtr)0);
		}
	}
}
