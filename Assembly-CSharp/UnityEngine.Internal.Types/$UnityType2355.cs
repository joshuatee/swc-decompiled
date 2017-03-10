using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2355 : $UnityType
	{
		public unsafe $UnityType2355()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 931612) = ldftn($Invoke0);
			*(data + 931640) = ldftn($Invoke1);
			*(data + 931668) = ldftn($Invoke2);
			*(data + 931696) = ldftn($Invoke3);
			*(data + 931724) = ldftn($Invoke4);
			*(data + 931752) = ldftn($Invoke5);
			*(data + 931780) = ldftn($Invoke6);
			*(data + 931808) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new SquadWarPlayerDetailsScreen((UIntPtr)0);
		}
	}
}
