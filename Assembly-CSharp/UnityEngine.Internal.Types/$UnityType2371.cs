using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2371 : $UnityType
	{
		public unsafe $UnityType2371()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 937156) = ldftn($Invoke0);
			*(data + 937184) = ldftn($Invoke1);
			*(data + 937212) = ldftn($Invoke2);
			*(data + 937240) = ldftn($Invoke3);
			*(data + 937268) = ldftn($Invoke4);
			*(data + 937296) = ldftn($Invoke5);
			*(data + 937324) = ldftn($Invoke6);
			*(data + 937352) = ldftn($Invoke7);
			*(data + 937380) = ldftn($Invoke8);
			*(data + 937408) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new TrainingUpgradeScreen((UIntPtr)0);
		}
	}
}
