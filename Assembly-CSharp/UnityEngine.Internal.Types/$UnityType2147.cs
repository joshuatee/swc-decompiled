using StaRTS.Main.Views.Animations;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2147 : $UnityType
	{
		public unsafe $UnityType2147()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 860772) = ldftn($Invoke0);
			*(data + 860800) = ldftn($Invoke1);
			*(data + 860828) = ldftn($Invoke2);
			*(data + 860856) = ldftn($Invoke3);
			*(data + 860884) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new AnimUXPosition((UIntPtr)0);
		}
	}
}
