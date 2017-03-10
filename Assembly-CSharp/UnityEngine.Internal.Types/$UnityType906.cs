using StaRTS.Main.Controllers.SquadWar;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType906 : $UnityType
	{
		public unsafe $UnityType906()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 590320) = ldftn($Invoke0);
			*(data + 590348) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new SquadWarBuffManager((UIntPtr)0);
		}
	}
}
