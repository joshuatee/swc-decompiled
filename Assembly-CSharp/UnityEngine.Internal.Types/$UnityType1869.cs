using StaRTS.Main.Models.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1869 : $UnityType
	{
		public unsafe $UnityType1869()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 706856) = ldftn($Invoke0);
			*(data + 706884) = ldftn($Invoke1);
			*(data + 706912) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new SquadWarHistoryEntry((UIntPtr)0);
		}
	}
}
