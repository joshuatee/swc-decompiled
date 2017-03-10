using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2346 : $UnityType
	{
		public unsafe $UnityType2346()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 930856) = ldftn($Invoke0);
			*(data + 930884) = ldftn($Invoke1);
			*(data + 930912) = ldftn($Invoke2);
			*(data + 930940) = ldftn($Invoke3);
			*(data + 930968) = ldftn($Invoke4);
			*(data + 930996) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new SquadWarBuffBaseDetails((UIntPtr)0);
		}
	}
}
