using StaRTS.Main.Models.Player.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1838 : $UnityType
	{
		public unsafe $UnityType1838()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 702908) = ldftn($Invoke0);
			*(data + 702936) = ldftn($Invoke1);
			*(data + 702964) = ldftn($Invoke2);
			*(data + 702992) = ldftn($Invoke3);
			*(data + 703020) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new Building((UIntPtr)0);
		}
	}
}
