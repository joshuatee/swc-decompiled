using StaRTS.Main.Models.Commands.Squads.Requests;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1638 : $UnityType
	{
		public unsafe $UnityType1638()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 662224) = ldftn($Invoke0);
			*(data + 662252) = ldftn($Invoke1);
			*(data + 662280) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new SquadIDRequest((UIntPtr)0);
		}
	}
}
