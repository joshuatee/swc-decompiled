using StaRTS.Main.Models.Commands.Squads.Requests;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1634 : $UnityType
	{
		public unsafe $UnityType1634()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 661832) = ldftn($Invoke0);
			*(data + 661860) = ldftn($Invoke1);
			*(data + 661888) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new SearchSquadsByNameRequest((UIntPtr)0);
		}
	}
}
