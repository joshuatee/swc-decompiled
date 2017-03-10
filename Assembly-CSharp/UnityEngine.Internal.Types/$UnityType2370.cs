using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2370 : $UnityType
	{
		public unsafe $UnityType2370()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 936988) = ldftn($Invoke0);
			*(data + 937016) = ldftn($Invoke1);
			*(data + 937044) = ldftn($Invoke2);
			*(data + 937072) = ldftn($Invoke3);
			*(data + 937100) = ldftn($Invoke4);
			*(data + 937128) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new TrainingInfoScreen((UIntPtr)0);
		}
	}
}
