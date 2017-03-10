using StaRTS.Main.Models.Commands.Player.Building.Construct;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1485 : $UnityType
	{
		public unsafe $UnityType1485()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 656512) = ldftn($Invoke0);
			*(data + 656540) = ldftn($Invoke1);
			*(data + 656568) = ldftn($Invoke2);
			*(data + 656596) = ldftn($Invoke3);
			*(data + 656624) = ldftn($Invoke4);
			*(data + 656652) = ldftn($Invoke5);
			*(data + 656680) = ldftn($Invoke6);
			*(data + 656708) = ldftn($Invoke7);
			*(data + 656736) = ldftn($Invoke8);
			*(data + 656764) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new BuildingConstructRequest((UIntPtr)0);
		}
	}
}
