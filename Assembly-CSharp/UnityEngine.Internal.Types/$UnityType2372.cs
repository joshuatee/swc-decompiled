using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2372 : $UnityType
	{
		public unsafe $UnityType2372()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 937436) = ldftn($Invoke0);
			*(data + 937464) = ldftn($Invoke1);
			*(data + 937492) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new TrapInfoScreen((UIntPtr)0);
		}
	}
}
