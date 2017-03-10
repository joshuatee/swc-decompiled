using StaRTS.Main.Views.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2494 : $UnityType
	{
		public unsafe $UnityType2494()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 976692) = ldftn($Invoke0);
			*(data + 976720) = ldftn($Invoke1);
			*(data + 976748) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new BattleMessageView((UIntPtr)0);
		}
	}
}
