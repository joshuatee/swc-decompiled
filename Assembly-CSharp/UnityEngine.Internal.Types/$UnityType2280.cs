using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2280 : $UnityType
	{
		public unsafe $UnityType2280()
		{
			*(UnityEngine.Internal.$Metadata.data + 908960) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new ArmoryUpgradeScreen((UIntPtr)0);
		}
	}
}
