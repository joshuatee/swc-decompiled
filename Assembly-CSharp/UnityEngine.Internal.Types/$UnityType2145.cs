using StaRTS.Main.Views;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2145 : $UnityType
	{
		public unsafe $UnityType2145()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 860716) = ldftn($Invoke0);
			*(data + 860744) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new VictoryStarAnimation((UIntPtr)0);
		}
	}
}
