using StaRTS.Main.Models.Commands.Player.Building.Rearm;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1498 : $UnityType
	{
		public unsafe $UnityType1498()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 657268) = ldftn($Invoke0);
			*(data + 657296) = ldftn($Invoke1);
			*(data + 657324) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new RearmTrapRequest((UIntPtr)0);
		}
	}
}
