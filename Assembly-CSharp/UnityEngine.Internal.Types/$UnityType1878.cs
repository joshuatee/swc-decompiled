using StaRTS.Main.Models.Squads.War;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1878 : $UnityType
	{
		public unsafe $UnityType1878()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 707444) = ldftn($Invoke0);
			*(data + 707472) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new SquadWarSquadData((UIntPtr)0);
		}
	}
}
