using StaRTS.Main.Models.Squads.War;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1871 : $UnityType
	{
		public unsafe $UnityType1871()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 707080) = ldftn($Invoke0);
			*(data + 707108) = ldftn($Invoke1);
			*(data + 707136) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new SquadWarBuffBaseData((UIntPtr)0);
		}
	}
}
