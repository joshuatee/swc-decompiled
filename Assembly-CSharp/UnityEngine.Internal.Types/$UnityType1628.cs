using StaRTS.Main.Models.Commands.Squads.Requests;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1628 : $UnityType
	{
		public unsafe $UnityType1628()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 661384) = ldftn($Invoke0);
			*(data + 661412) = ldftn($Invoke1);
			*(data + 661440) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new FriendLBIDRequest((UIntPtr)0);
		}
	}
}
