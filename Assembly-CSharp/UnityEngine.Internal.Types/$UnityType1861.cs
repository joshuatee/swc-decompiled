using StaRTS.Main.Models.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1861 : $UnityType
	{
		public unsafe $UnityType1861()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 705624) = ldftn($Invoke0);
			*(data + 705652) = ldftn($Invoke1);
			*(data + 705680) = ldftn($Invoke2);
			*(data + 705708) = ldftn($Invoke3);
			*(data + 705736) = ldftn($Invoke4);
			*(data + 705764) = ldftn($Invoke5);
			*(data + 705792) = ldftn($Invoke6);
			*(data + 705820) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new SquadInvite((UIntPtr)0);
		}
	}
}
