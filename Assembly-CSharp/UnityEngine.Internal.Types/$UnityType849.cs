using StaRTS.Main.Controllers.Missions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType849 : $UnityType
	{
		public unsafe $UnityType849()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 576936) = ldftn($Invoke0);
			*(data + 576964) = ldftn($Invoke1);
			*(data + 576992) = ldftn($Invoke2);
			*(data + 577020) = ldftn($Invoke3);
			*(data + 577048) = ldftn($Invoke4);
			*(data + 577076) = ldftn($Invoke5);
			*(data + 577104) = ldftn($Invoke6);
			*(data + 577132) = ldftn($Invoke7);
			*(data + 577160) = ldftn($Invoke8);
			*(data + 577188) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new AbstractMissionProcessor((UIntPtr)0);
		}
	}
}
