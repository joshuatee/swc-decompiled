using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1326 : $UnityType
	{
		public unsafe $UnityType1326()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 650184) = ldftn($Invoke0);
			*(data + 650212) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new CheatFastForwardContractsRequest((UIntPtr)0);
		}
	}
}
