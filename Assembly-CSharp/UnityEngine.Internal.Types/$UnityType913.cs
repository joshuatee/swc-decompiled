using StaRTS.Main.Controllers.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType913 : $UnityType
	{
		public unsafe $UnityType913()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 594240) = ldftn($Invoke0);
			*(data + 594268) = ldftn($Invoke1);
			*(data + 594296) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new SquadNotifAdapter((UIntPtr)0);
		}
	}
}
