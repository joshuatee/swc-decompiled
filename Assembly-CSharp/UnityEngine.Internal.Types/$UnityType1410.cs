using StaRTS.Main.Models.Commands.Missions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1410 : $UnityType
	{
		public unsafe $UnityType1410()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 652760) = ldftn($Invoke0);
			*(data + 652788) = ldftn($Invoke1);
			*(data + 652816) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new RaidMissionIdRequest((UIntPtr)0);
		}
	}
}
