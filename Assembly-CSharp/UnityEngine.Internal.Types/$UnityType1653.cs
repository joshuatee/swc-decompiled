using StaRTS.Main.Models.Commands.Squads.Responses;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1653 : $UnityType
	{
		public unsafe $UnityType1653()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 664212) = ldftn($Invoke0);
			*(data + 664240) = ldftn($Invoke1);
			*(data + 664268) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new SquadNotifsResponse((UIntPtr)0);
		}
	}
}
