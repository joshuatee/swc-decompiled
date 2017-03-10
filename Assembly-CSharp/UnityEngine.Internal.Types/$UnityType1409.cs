using StaRTS.Main.Models.Commands.Missions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1409 : $UnityType
	{
		public unsafe $UnityType1409()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 652620) = ldftn($Invoke0);
			*(data + 652648) = ldftn($Invoke1);
			*(data + 652676) = ldftn($Invoke2);
			*(data + 652704) = ldftn($Invoke3);
			*(data + 652732) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new MissionIdRequest((UIntPtr)0);
		}
	}
}
