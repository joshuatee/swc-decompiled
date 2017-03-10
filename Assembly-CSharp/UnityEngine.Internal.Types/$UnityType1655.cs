using StaRTS.Main.Models.Commands.Squads.Responses;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1655 : $UnityType
	{
		public unsafe $UnityType1655()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 664436) = ldftn($Invoke0);
			*(data + 664464) = ldftn($Invoke1);
			*(data + 664492) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new SquadWarBuffBaseResponse((UIntPtr)0);
		}
	}
}
