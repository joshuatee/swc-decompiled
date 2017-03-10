using StaRTS.Main.Views.World.Targeting;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2526 : $UnityType
	{
		public unsafe $UnityType2526()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 987248) = ldftn($Invoke0);
			*(data + 987276) = ldftn($Invoke1);
			*(data + 987304) = ldftn($Invoke2);
			*(data + 987332) = ldftn($Invoke3);
			*(data + 987360) = ldftn($Invoke4);
			*(data + 987388) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new HeroIdentifier((UIntPtr)0);
		}
	}
}
