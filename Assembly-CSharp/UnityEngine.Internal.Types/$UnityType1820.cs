using StaRTS.Main.Models.Player.Misc;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1820 : $UnityType
	{
		public unsafe $UnityType1820()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 693640) = ldftn($Invoke0);
			*(data + 693668) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ArmoryInfo((UIntPtr)0);
		}
	}
}
