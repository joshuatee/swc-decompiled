using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1365 : $UnityType
	{
		public unsafe $UnityType1365()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 651388) = ldftn($Invoke0);
			*(data + 651416) = ldftn($Invoke1);
			*(data + 651444) = ldftn($Invoke2);
			*(data + 651472) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new CheatSquadWarTimeTravelRequest((UIntPtr)0);
		}
	}
}
