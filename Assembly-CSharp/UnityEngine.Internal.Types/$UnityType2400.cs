using StaRTS.Main.Views.UX.Screens.ScreenHelpers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2400 : $UnityType
	{
		public unsafe $UnityType2400()
		{
			*(UnityEngine.Internal.$Metadata.data + 947348) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new EquipmentTabHelper((UIntPtr)0);
		}
	}
}
