using StaRTS.Main.Models.Player.Misc;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1828 : $UnityType
	{
		public unsafe $UnityType1828()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 699660) = ldftn($Invoke0);
			*(data + 699688) = ldftn($Invoke1);
			*(data + 699716) = ldftn($Invoke2);
			*(data + 699744) = ldftn($Invoke3);
			*(data + 699772) = ldftn($Invoke4);
			*(data + 699800) = ldftn($Invoke5);
			*(data + 699828) = ldftn($Invoke6);
			*(data + 699856) = ldftn($Invoke7);
			*(data + 699884) = ldftn($Invoke8);
			*(data + 699912) = ldftn($Invoke9);
			*(data + 699940) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new Tournament((UIntPtr)0);
		}
	}
}
