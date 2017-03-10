using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2344 : $UnityType
	{
		public unsafe $UnityType2344()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 930296) = ldftn($Invoke0);
			*(data + 930324) = ldftn($Invoke1);
			*(data + 930352) = ldftn($Invoke2);
			*(data + 930380) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new SquadUpgradeScreen((UIntPtr)0);
		}
	}
}
