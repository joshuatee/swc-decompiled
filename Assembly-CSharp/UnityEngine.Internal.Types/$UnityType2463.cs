using StaRTS.Main.Views.UX.Screens.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2463 : $UnityType
	{
		public unsafe $UnityType2463()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 966444) = ldftn($Invoke0);
			*(data + 966472) = ldftn($Invoke1);
			*(data + 966500) = ldftn($Invoke2);
			*(data + 966528) = ldftn($Invoke3);
			*(data + 966556) = ldftn($Invoke4);
			*(data + 966584) = ldftn($Invoke5);
			*(data + 966612) = ldftn($Invoke6);
			*(data + 966640) = ldftn($Invoke7);
			*(data + 966668) = ldftn($Invoke8);
			*(data + 966696) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new SquadTroopRequestScreen((UIntPtr)0);
		}
	}
}
