using StaRTS.Main.Models.Commands.Objectives;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1412 : $UnityType
	{
		public unsafe $UnityType1412()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 652844) = ldftn($Invoke0);
			*(data + 652872) = ldftn($Invoke1);
			*(data + 652900) = ldftn($Invoke2);
			*(data + 652928) = ldftn($Invoke3);
			*(data + 652956) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new ClaimObjectiveRequest((UIntPtr)0);
		}
	}
}
