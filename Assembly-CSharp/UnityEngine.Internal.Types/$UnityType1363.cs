using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1363 : $UnityType
	{
		public unsafe $UnityType1363()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 651304) = ldftn($Invoke0);
			*(data + 651332) = ldftn($Invoke1);
			*(data + 651360) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new CheatSetWarRatingRequest((UIntPtr)0);
		}
	}
}
