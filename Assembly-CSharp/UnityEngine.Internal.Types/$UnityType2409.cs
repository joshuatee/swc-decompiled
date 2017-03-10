using StaRTS.Main.Views.UX.Screens.ScreenHelpers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2409 : $UnityType
	{
		public unsafe $UnityType2409()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 949336) = ldftn($Invoke0);
			*(data + 949364) = ldftn($Invoke1);
			*(data + 949392) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new SquadWarBuffIconHelper((UIntPtr)0);
		}
	}
}
