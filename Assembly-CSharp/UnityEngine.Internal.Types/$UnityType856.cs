using StaRTS.Main.Controllers.Missions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType856 : $UnityType
	{
		public unsafe $UnityType856()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 578756) = ldftn($Invoke0);
			*(data + 578784) = ldftn($Invoke1);
			*(data + 578812) = ldftn($Invoke2);
			*(data + 578840) = ldftn($Invoke3);
			*(data + 578868) = ldftn($Invoke4);
			*(data + 578896) = ldftn($Invoke5);
			*(data + 578924) = ldftn($Invoke6);
			*(data + 578952) = ldftn($Invoke7);
			*(data + 578980) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new PvpStarsMissionProcessor((UIntPtr)0);
		}
	}
}
