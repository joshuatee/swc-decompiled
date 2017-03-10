using StaRTS.Main.Models.Commands.Squads.Requests;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1645 : $UnityType
	{
		public unsafe $UnityType1645()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 662588) = ldftn($Invoke0);
			*(data + 662616) = ldftn($Invoke1);
			*(data + 662644) = ldftn($Invoke2);
			*(data + 662672) = ldftn($Invoke3);
			*(data + 662700) = ldftn($Invoke4);
			*(data + 662728) = ldftn($Invoke5);
			*(data + 662756) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new TroopDonateRequest((UIntPtr)0);
		}
	}
}
