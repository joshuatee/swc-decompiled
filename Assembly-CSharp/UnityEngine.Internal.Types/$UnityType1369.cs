using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1369 : $UnityType
	{
		public unsafe $UnityType1369()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 651528) = ldftn($Invoke0);
			*(data + 651556) = ldftn($Invoke1);
			*(data + 651584) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new CheatStartWarRequest((UIntPtr)0);
		}
	}
}
