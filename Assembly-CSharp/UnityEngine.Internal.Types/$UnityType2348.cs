using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2348 : $UnityType
	{
		public unsafe $UnityType2348()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 931164) = ldftn($Invoke0);
			*(data + 931192) = ldftn($Invoke1);
			*(data + 931220) = ldftn($Invoke2);
			*(data + 931248) = ldftn($Invoke3);
			*(data + 931276) = ldftn($Invoke4);
			*(data + 931304) = ldftn($Invoke5);
			*(data + 931332) = ldftn($Invoke6);
			*(data + 931360) = ldftn($Invoke7);
			*(data + 931388) = ldftn($Invoke8);
			*(data + 931416) = ldftn($Invoke9);
			*(data + 931444) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new SquadWarInfoScreen((UIntPtr)0);
		}
	}
}
