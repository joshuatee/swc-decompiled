using StaRTS.Main.Controllers.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType982 : $UnityType
	{
		public unsafe $UnityType982()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 603060) = ldftn($Invoke0);
			*(data + 603088) = ldftn($Invoke1);
			*(data + 603116) = ldftn($Invoke2);
			*(data + 603144) = ldftn($Invoke3);
			*(data + 603172) = ldftn($Invoke4);
			*(data + 603200) = ldftn($Invoke5);
			*(data + 603228) = ldftn($Invoke6);
			*(data + 603256) = ldftn($Invoke7);
			*(data + 603284) = ldftn($Invoke8);
			*(data + 603312) = ldftn($Invoke9);
			*(data + 603340) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new ReplayMapDataLoader((UIntPtr)0);
		}
	}
}
