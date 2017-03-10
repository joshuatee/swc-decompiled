using StaRTS.Main.Views.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2511 : $UnityType
	{
		public unsafe $UnityType2511()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 983356) = ldftn($Invoke0);
			*(data + 983384) = ldftn($Invoke1);
			*(data + 983412) = ldftn($Invoke2);
			*(data + 983440) = ldftn($Invoke3);
			*(data + 983468) = ldftn($Invoke4);
			*(data + 983496) = ldftn($Invoke5);
			*(data + 983524) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new Scaffolding((UIntPtr)0);
		}
	}
}
