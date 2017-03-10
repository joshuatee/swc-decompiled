using StaRTS.Main.Views.World.Targeting;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2525 : $UnityType
	{
		public unsafe $UnityType2525()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 986968) = ldftn($Invoke0);
			*(data + 986996) = ldftn($Invoke1);
			*(data + 987024) = ldftn($Invoke2);
			*(data + 987052) = ldftn($Invoke3);
			*(data + 987080) = ldftn($Invoke4);
			*(data + 987108) = ldftn($Invoke5);
			*(data + 987136) = ldftn($Invoke6);
			*(data + 987164) = ldftn($Invoke7);
			*(data + 987192) = ldftn($Invoke8);
			*(data + 987220) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new HeroDecal((UIntPtr)0);
		}
	}
}
