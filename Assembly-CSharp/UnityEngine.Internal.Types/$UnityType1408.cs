using StaRTS.Main.Models.Commands.Missions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1408 : $UnityType
	{
		public unsafe $UnityType1408()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 652536) = ldftn($Invoke0);
			*(data + 652564) = ldftn($Invoke1);
			*(data + 652592) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new GetMissionMapResponse((UIntPtr)0);
		}
	}
}
