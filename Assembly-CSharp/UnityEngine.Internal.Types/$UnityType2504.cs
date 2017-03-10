using StaRTS.Main.Views.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2504 : $UnityType
	{
		public unsafe $UnityType2504()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 979632) = ldftn($Invoke0);
			*(data + 979660) = ldftn($Invoke1);
			*(data + 979688) = ldftn($Invoke2);
			*(data + 979716) = ldftn($Invoke3);
			*(data + 979744) = ldftn($Invoke4);
			*(data + 979772) = ldftn($Invoke5);
			*(data + 979800) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new LandingTakeOffEffectAnim((UIntPtr)0);
		}
	}
}
