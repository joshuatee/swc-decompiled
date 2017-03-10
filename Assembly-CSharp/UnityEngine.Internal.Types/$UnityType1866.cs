using StaRTS.Main.Models.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1866 : $UnityType
	{
		public unsafe $UnityType1866()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 706716) = ldftn($Invoke0);
			*(data + 706744) = ldftn($Invoke1);
			*(data + 706772) = ldftn($Invoke2);
			*(data + 706800) = ldftn($Invoke3);
			*(data + 706828) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new SquadPerks((UIntPtr)0);
		}
	}
}
