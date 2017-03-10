using StaRTS.Main.Views.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2513 : $UnityType
	{
		public unsafe $UnityType2513()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 983832) = ldftn($Invoke0);
			*(data + 983860) = ldftn($Invoke1);
			*(data + 983888) = ldftn($Invoke2);
			*(data + 983916) = ldftn($Invoke3);
			*(data + 983944) = ldftn($Invoke4);
			*(data + 983972) = ldftn($Invoke5);
			*(data + 984000) = ldftn($Invoke6);
			*(data + 984028) = ldftn($Invoke7);
			*(data + 984056) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new ShadowAnim((UIntPtr)0);
		}
	}
}
